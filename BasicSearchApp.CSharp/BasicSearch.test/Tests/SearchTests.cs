using BasicSearchApp.data;
using BasicSearchApp.data.Context;
using BasicSearchApp.Repositories;
using BasicSearchApp.Services;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests
{
    [TestFixture]
    public class SearchTests
    {
        private IPatientRepository patientRepository;
        private IDocumentRepository documentRepository;
        private IIndexingService indexingService;
        private Mock<IConfigurationService> configurationService;
        private IBasicSearchService basicSearchService;
        private InternalDatabase intenralDatabase;

        [SetUp]
        public void Setup()
        {
            patientRepository = new PatientRepository();
            documentRepository = new DocumentRepository();
            indexingService = new IndexingService(documentRepository);
            basicSearchService = new BasicSearchService(patientRepository, documentRepository);
            configurationService = new Mock<IConfigurationService>();
        }

        [Test]
        public void SearchBlankShouldReturnAll()
        {
            configurationService.Setup(cm => cm.GetAppSetting("ResourcePath")).Returns(@"c:\resources");

            intenralDatabase = new InternalDatabase(patientRepository, documentRepository, indexingService, configurationService.Object);
            intenralDatabase.Seed();

            var query = string.Empty;
            int count = basicSearchService.Filter(query).Documents.Count;

            Assert.IsTrue(count > 0);
        }

        [Test]
        public void SearchForCommonShouldReturnMany()
        {
            configurationService.Setup(cm => cm.GetAppSetting("ResourcePath")).Returns(@"c:\resources");

            var query = "The";
            int count = basicSearchService.Filter(query).Documents.Count;

            Assert.IsTrue(count > 0);
        }

        [Test]
        public void SearchForOddShouldReturnNone()
        {
            configurationService.Setup(cm => cm.GetAppSetting("ResourcePath")).Returns(@"c:\resources");

            var query = "Arbez"; // Zebra backwards
            int count = basicSearchService.Filter(query).Documents.Count;

            Assert.IsTrue(count == 0);
        }

        [Test]
        public void SearchMultipleWordsShouldReturnResults()
        {
            configurationService.Setup(cm => cm.GetAppSetting("ResourcePath")).Returns(@"c:\resources");

            var query = "The To";
            int count = basicSearchService.Filter(query).Documents.Count;

            Assert.IsTrue(count > 0);
        }

        [Test]
        public void SearchQuotedPhraseShouldReturnResults()
        {
            configurationService.Setup(cm => cm.GetAppSetting("ResourcePath")).Returns(@"c:\resources");

            var query = "\"denies any\"";
            int count = basicSearchService.Filter(query).Documents.Count;

            Assert.IsTrue(count > 0);
        }
    }

}
