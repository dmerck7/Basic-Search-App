using BasicSearchApp.data.Context;
using BasicSearchApp.Entities;
using BasicSearchApp.Repositories;
using BasicSearchApp.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Configuration;

namespace BasicSearchApp.data
{    
    /// <summary>
    /// Created 5/8/2020
    /// Author - David Merck
    /// 
    /// This is used to seed the internal database of the app
    /// This should be replaced by a DB
    /// The wouldbe DB is abstracted by the data layer using repos
    /// Missing details about the source of documents
    /// TODO - Connect and use a relational DB
    /// </summary>
    public class InternalDatabase
    {
        private readonly IPatientRepository patientRepository;
        private readonly IDocumentRepository documentRepository;
        private readonly IConfigurationService configurationService;
        private readonly IIndexingService indexingService;

        public InternalDatabase(IPatientRepository patientRepository, IDocumentRepository documentRepository, IIndexingService indexingService, IConfigurationService configurationService)
        {
            this.patientRepository = patientRepository;
            this.documentRepository = documentRepository;
            this.indexingService = indexingService;
            this.configurationService = configurationService;
        }

        // Loads initial resource folder index state from persisted index.json file
        // TODO - Store the index in a DB
        public void LoadInitialIndex()
        {
            var jsonString = File.ReadAllText(this.configurationService.GetAppSetting("ResourcePath") + @"\index.json");
            DbContext.Index = JsonConvert.DeserializeObject<ConcurrentDictionary<string, List<dynamic>>>(jsonString);
        }

        // Called at te start of the app to populate the DbContext for use with the data repositories
        public void Seed()
        {

            this.documentRepository.Create(
               new Document()
               {
                   Id = 2000000L,
                   PatientId = 1000000L,
                   Name = "Patient Note",
                   Date = new DateTime(2010, 6, 20),
                   Filename = "Mary_1"
               }
           );

            this.documentRepository.Create(
               new Document()
               {
                   Id = 2000001L,
                   PatientId = 1000000L,
                   Name = "Patient Note",
                   Date = new DateTime(2010, 6, 20),
                   Filename = "Mary_2"
               }
            );

            this.patientRepository.Create(
                new Patient()
                {
                    Id = 1000000L,
                    Name = "Mary TestPerson",
                    Documents = this.documentRepository.GetAllByPatientId(1000000L).Select(document => new Document()
                    {
                        Id = document.Id,
                        PatientId = document.PatientId,
                        Filename = document.Filename,
                        Date = document.Date,
                    }).ToList()
                }
            );


            this.documentRepository.Create(
                new Document()
                {
                    Id = 2000002L,
                    PatientId = 1000001L,
                    Name = "Clinical Note",
                    Date = new DateTime(2010, 4, 6),
                    Filename = "Joe_1"
                }
            );
            this.documentRepository.Create(
               new Document()
               {
                   Id = 2000003L,
                   PatientId = 1000001L,
                   Name = "Summary",
                   Date = new DateTime(2010, 7, 2),
                   Filename = "Joe_2"
               }
            );

            this.patientRepository.Create(
                new Patient()
                {
                    Id = 1000001L,
                    Name = "Joe TestPerson",
                    Documents = this.documentRepository.GetAllByPatientId(1000001L).Select(document => new Document()
                    {
                        Id = document.Id,
                        PatientId = document.PatientId,
                        Filename = document.Filename,
                        Date = document.Date,
                    }).ToList()
                }
            );


            this.documentRepository.Create(
                new Document()
                {
                    Id = 2000004L,
                    PatientId = 1000002L,
                    Name = "Clinical Note",
                    Date = new DateTime(2010, 8, 3),
                    Filename = "Sam_1"
                }
            );

            this.patientRepository.Create(
               new Patient()
               {
                   Id = 1000002L,
                   Name = "Sam TestPerson",
                   Documents = this.documentRepository.GetAllByPatientId(1000002L).Select(document => new Document()
                   {
                       Id = document.Id,
                       PatientId = document.PatientId,
                       Filename = document.Filename,
                       Date = document.Date,
                   }).ToList()
               }
            );

            // We load a pre-cached json file instead of indexing everytime we launch the program
            //indexingService.IndexAll();

            this.LoadInitialIndex();
        }




    }
}
