
using BasicSearchApp.data.Context;
using BasicSearchApp.Entities;
using BasicSearchApp.Repositories;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text.RegularExpressions;

namespace BasicSearchApp.Services
{
    /// <summary>
    /// Created 5/8/2020
    /// Author - David Merck
    /// 
    /// This service recieves a query through the filter method.  
    /// It then queries the search index for results
    /// Finally it consolidates the results into a document header view model for the client
    /// </summary>
    public class BasicSearchService : IBasicSearchService
    {
        private readonly IPatientRepository patientRepository;
        private readonly IDocumentRepository documentRepository;

        public BasicSearchService(IPatientRepository patientRepository, IDocumentRepository documentRepository)
        {
            this.patientRepository = patientRepository;
            this.documentRepository = documentRepository;
        }

        /// <summary>
        /// Given a query return header results for the query else return all documents
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public dynamic Filter(string query)
        {
            dynamic wrapper = new ExpandoObject();
            List<dynamic> results = new List<dynamic>();
            // If query is given parse through results
            if (query != null && query != string.Empty)
            {
                // Query can have multiple words or phrases in quotes so handle accordingly
                List<dynamic> queryResults = GetQueryIndexResults(query);
                
                // Single punctuation will return null result
                if (queryResults != null)
                {
                    // Index sightings can have more than one per document so remove duplicate documents 
                    var documentIds = queryResults.Select(d => d.documentId).Distinct();

                    // Build document headers
                    var documentList = new List<dynamic>();
                    foreach (var documentId in documentIds)
                    {
                        Document document = this.documentRepository.GetByID((long)documentId);
                        Patient patient = this.patientRepository.GetByID((long)document.PatientId);

                        // TODO - Calc relavancy based on multiple words and phrases so weights can be added (will be needed for negative values)
                        // Determine relevancy for document based on search
                        List<dynamic> lineIndexList = queryResults.Where(r => r.patientId == patient.Id && r.documentId == document.Id).Select(l => l.lineIndex).ToList();

                        results.Add(this.buildResult(document, patient, lineIndexList.Count));
                    }
                    results.OrderByDescending(x => x.RelevantCount);
                }
            }
            // No query given so return evry document header
            else
            {
                // Build document headers
                var documentList = this.documentRepository.GetAll().OrderByDescending(d => d.Date);
                foreach (var document in documentList)
                {
                    Patient patient = this.patientRepository.GetByID((long)document.PatientId);

                    results.Add(this.buildResult(document, patient, 0));
                }
            }
            wrapper.Documents = results;
            return wrapper;
        }

        // Create document header view model from indexing data and repos
        private dynamic buildResult(Document document, Patient patient, int count) {
             
            dynamic item = new ExpandoObject();
            item.patientId = patient.Id;
            item.PatientName = patient.Name;
            item.DocumentName = document.Name;
            item.DocumentDate = document.Date;
            item.DocumentDateDisplay = document.Date.ToString("MMMM d, yyyy");
            item.DocumentRelevantCount = count;
            item.Id = document.Id;

            return item;
        }

        // Query can be word, multiple words, or phases contained in quotes
        private List<dynamic> GetQueryIndexResults(string query)
        {
            List<dynamic> queryResults = null;

            // Split the query into tokens, words or word sequences/phases(based on in quotes) based on RegExp
            IEnumerable<string> queryTokens = SplitQuery(query);

            // Cycle though tokens (word or phrase)
            foreach (var queryToken in queryTokens)
            {
                string[] wordSeparators = { " " };
                IEnumerable<string> queryWords = queryToken.Split(wordSeparators, StringSplitOptions.RemoveEmptyEntries);

                List<dynamic> indexResults = new List<dynamic>();
                if (queryWords.ToList().Count > 1)
                {
                    // Handle sequence/phrase
                    var wordList = queryWords.ToList();
                    indexResults = GetSequenceResults(wordList);
                }
                else
                {
                    // Handle single word
                    var queryWord = queryWords.ToList().First();
                    indexResults = this.getIndexValue(queryWord);
                }
                queryResults = (queryResults == null) ? indexResults : queryResults.Concat(indexResults).ToList();
            }

            return queryResults;
        }

        // Handle sequence/phase results from search indexing
        // Must verify the words word found together in he right order
        private List<dynamic> GetSequenceResults(List<string> wordList)
        {
            List<dynamic> result = new List<dynamic>();

            // Cycle through each word in the phrase
            for (var i = 0; i < wordList.Count; i++)
            {
                var queryWord = wordList[i];
                var prevWord = (i == 0) ? null : wordList[i - 1];
                var nextWord = (i == wordList.Count-1) ? null : wordList[i + 1];

                // Get the indexing data(lists) or the current word, the one before it, and the next word
                List<dynamic> items = this.getIndexValue(queryWord, true);
                List<dynamic> prevs = this.getIndexValue(prevWord, true);
                List<dynamic> nexts = this.getIndexValue(nextWord, true);

                // Use joins with a where cause to only get valid(next to each other and in the right order) results regarding the current word and the previous word
                var validPrevResults = from item in items
                                       join prev in prevs
                                       on new { item.documentId } equals new { prev.documentId}
                                       where (item.lineIndex == prev.lineIndex && item.wordIndex - 1 == prev.wordIndex) // Handle on the same line 
                                       || (item.wordIndex==0 && prev.WordIndex==prev.lineLength && prev.lineIndex==item.lineIndex-1)  // handle spanning multiple lines
                                       select item;

                // Use joins with a where cause to only get valid(next to each other and in the right order) results regarding the current word and the next word
                var validNextResults = from item in items
                                       join next in nexts
                                       on item.documentId equals next.documentId
                                       where (item.lineIndex == next.lineIndex && item.wordIndex + 1 == next.wordIndex)
                                       || (item.wordIndex == item.lineLength && next.wordIndex == 0 && next.lineIndex == item.lineIndex + 1)
                                       select item;

                // Consolidate based on first word, last word, or somewhere in the middle)
                if (i == 0)
                {
                    result = result.Concat(validNextResults).ToList();
                } 
                else if (i == wordList.Count - 1)
                {
                    result = result.Concat(validPrevResults).ToList();
                }
                else
                {
                    result = result.Concat(validPrevResults.Intersect(validNextResults)).ToList();
                }
            }

            return result;
        }

        // Utility function used to remove quotes from query and return the quoted phrases and single words in an array
        private IEnumerable<string> SplitQuery(string query)
        {
            var re = new Regex("(?<=\")[^\"]*(?=\")|[^\" ]+");
            IEnumerable<string> results = re.Matches(query).Cast<Match>().Select(m => m.Value);

            return results;
        }


        // Removes beginning and ending puctuation from words
        private string removePunctuationAtEnds(string word)
        {
            if (word!=null)
            {
                // Remove preceding punctuaction
                word = Regex.Replace(word, "^[^a-zA-Z]+", "");

                //Remove trailing punctuation
                word = Regex.Replace(word, "[^a-zA-Z]+$", "");
            }

            return word;
        }

       // Get the value(list of sightings) from the search index based on word
       // If word was not found in the documents(key not found) return a empty list
       // If null key(used for first and last words in phrase handling) return empty list
       private List<dynamic> getIndexValue(string key, bool? sequence = false)
        {
            key = this.removePunctuationAtEnds(key);

            List<dynamic> results = new List<dynamic>();

            if (key == null)
            {
                return new List<dynamic>();
            }

            // Do not partial match sequence words
            if ((bool)sequence)
            {
                if (DbContext.Index.ContainsKey(key.ToLower()))
                {
                    results = results.Concat(DbContext.Index[key.ToLower()]).ToList();
                }
            }
            else
            {
                // Handle partial matching
                IEnumerable<string> fullMatchingKeys = DbContext.Index.Keys.Where(currentKey => currentKey.Contains(key.ToLower()));
                if (fullMatchingKeys.ToList().Count == 0)
                {
                    return results;
                }

                foreach (var fullKey in fullMatchingKeys)
                {
                    results = results.Concat(DbContext.Index[fullKey.ToLower()]).ToList();
                }
            } 

            return results;
        }


    }
}
