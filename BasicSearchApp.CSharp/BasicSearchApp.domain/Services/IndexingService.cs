
using BasicSearchApp.Entities;
using BasicSearchApp.Repositories;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using BasicSearchApp.data.Context;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace BasicSearchApp.Services
{
    /// <summary>
    /// Created 5/8/2020
    /// Author - David Merck
    /// 
    /// Utilizing a produce/consume methology, we create/update a search index (concurrent dictionary)
    /// We can clear the index, index all documents, or add/remove documents one at a time
    /// The index(Concurrent Dictionary) is currently in DbContext.cs
    /// The current indexed state of the resource folderis stored in an index.json file in the resource folder
    /// Loaded on the start of the program
    /// 
    /// TODO - Persist the index in a DB
    /// TODO - Test add/remove single documents
    /// 
    /// Each entry under a word represents a sighting of that word in the index
    /// Example of index entry:
    /// 
    /// "right": [
    ///      {
    ///          "patientId": 1000001,
    ///          "documentId": 2000002,
    ///          "lineIndex": 29,
    ///          "wordIndex": 12,
    ///          "lineLength": 13
    ///     },
    ///        {
    ///          "patientId": 1000001,
    ///          "documentId": 2000002,
    ///          "lineIndex": 77,
    ///          "wordIndex": 0,
    ///          "lineLength": 1
    ///     }
    /// ]
    /// </summary>
    public class IndexingService : IIndexingService
    {
        private readonly IDocumentRepository documentRepository;

        public IndexingService(IDocumentRepository documentRepository)
        {
            this.documentRepository = documentRepository;
        }

        private String[] WordSeparators = { " " };
        private string[] LineSeparators = new string[] { "\r\n" };

        // Empty out the index(concurrent dictionary)
        public void ClearIndex()
        {
            DbContext.Index.Clear();
            DbContext.Index = new ConcurrentDictionary<string, List<dynamic>>();
        }

        // Add word sighting information to the index(concurrent dictionary) for each documet in the resources folder
        // Currently grabs fro the Document repo
        // TODO - Work out how to dymacally watch for added docs
        public void IndexAll()
        {
            DbContext.Index.Clear();
            DbContext.Index = new ConcurrentDictionary<string, List<dynamic>>();

            IEnumerable<Document> documents = this.documentRepository.GetAll();
            Parallel.ForEach(documents, (document) =>
            {
                IndexAddDocument(document);
            });
        }

        // The following dcuments the process used to produce and consume in the building of index data:
        // https://www.nimaara.com/practical-parallelization-with-map-reduce-in-c/
        // Built based on proformance cosiderations
        // Currently index by document
        // TODO - Add multiple pages(pageIndex) in a document and capture the lines-per-page(pageLength=lineCount) for phrases spanning pages
        public void IndexAddDocument(Document document)
        {
            const int WorkerCount = 12;
            const int BoundedCapacity = 10000;

            // Setup the queue
            var blockingCollection = new BlockingCollection<Line>(BoundedCapacity);

            // Declare the worker
            Action<Line> work = line =>
            {
                // Handle the words and enter their sightings in the concurrent dictionary
                IList<string> words = line.Content.Split(this.WordSeparators, StringSplitOptions.RemoveEmptyEntries).ToList();
                int wordIndex = 0;
                foreach (var word in words)
                {
                    dynamic location = new ExpandoObject();
                    location.patientId = document.PatientId;
                    location.documentId = document.Id;
                    location.lineIndex = line.Index;
                    location.wordIndex = wordIndex;
                    location.lineLength = words.Count - 1;
                    
                    var strippedWord = word;// this.removePunctuationAtEnds(word);
                    if (DbContext.Index.ContainsKey(strippedWord))
                    {
                        List<dynamic> list = DbContext.Index[strippedWord];
                        if (list.Contains(location) == false)
                        {
                            list.Add(location);
                        }
                    }
                    else
                    {
                        List<dynamic> list = new List<dynamic>();
                        list.Add(location);
                        DbContext.Index.TryAdd(strippedWord, list);
                    }
                    wordIndex++;
                }
            };

            Task.Run(() =>
            {
                // Begin producing
                // Read content line by line
                using (StreamReader reader = new StreamReader(@"c:\resources\documents\" + document.Filename + ".txt"))
                {
                    uint lineIndex = 0;
                    // call ReadLine until null.
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        Line l = new Line();
                        l.Content = line.ToString().ToLower();
                        l.Index = lineIndex;

                        blockingCollection.Add(l);
                        lineIndex++;
                    }
                }
                blockingCollection.CompleteAdding();
            });

            // Start consuming
            blockingCollection
                .GetConsumingEnumerable()
                .AsParallel()
                .WithDegreeOfParallelism(WorkerCount)
                .WithMergeOptions(ParallelMergeOptions.NotBuffered)
                .ForAll(work);
        }

        // Remove a document's word sightings fromm the index
        // TODO - Needs to be tested
        public void IndexRemoveDocument(Document document)
        {
            const int WorkerCount = 12;
            const int BoundedCapacity = 10000;

            // Setup the queue
            var blockingCollection = new BlockingCollection<Line>(BoundedCapacity);

            // Declare the worker
            Action<Line> work = line =>
            {
                dynamic location = new ExpandoObject();
                location.patientId = document.PatientId;
                location.documentId = document.Id;
                location.lineIndex = line.Index;

                var words = line.Content.Split(this.WordSeparators, StringSplitOptions.RemoveEmptyEntries);
                foreach (var word in words)
                {
                    var strippedWord = this.removePunctuationAtEnds(word);
                    if (DbContext.Index.ContainsKey(word))
                    {
                        List<dynamic> list = DbContext.Index[strippedWord];
                        if (list.Contains(location) == true)
                        {
                            list.Remove(location);

                            if (list.Count == 0)
                            {
                                dynamic value = null;
                                DbContext.Index.TryRemove(strippedWord, value);
                            }
                        }
                    }
                }
            };

            Task.Run(() =>
            {
                // Begin producing
                using (StreamReader reader = new StreamReader(@"c:\resources\documents\" + document.Filename + ".txt"))
                {
                    uint lineIndex = 0;
                    // call ReadLine until null.
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        Line l = new Line();
                        l.Content = line.ToString().ToLower();
                        l.Index = lineIndex;

                        blockingCollection.Add(l);
                        lineIndex++;
                    }
                }
                blockingCollection.CompleteAdding();
            });

            // Start consuming
            blockingCollection
                .GetConsumingEnumerable()
                .AsParallel()
                .WithDegreeOfParallelism(WorkerCount)
                .WithMergeOptions(ParallelMergeOptions.NotBuffered)
                .ForAll(work);
        }

        // May have problems with abbreviations but will capture words at the end of sentences
        private string removePunctuationAtEnds(string word)
        {
            // Remove preceding punctuaction
            word = Regex.Replace(word, "^[^a-zA-Z]+", "");

            //Remove trailing punctuation
            word = Regex.Replace(word, "[^a-zA-Z]+$", "");

            return word;
        }



    }


    public class Line
    {
        public string Content { get; set; }
        public uint Index { get; set; }
    }


}
