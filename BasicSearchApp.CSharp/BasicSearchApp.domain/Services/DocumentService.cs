using BasicSearchApp.Entities;
using BasicSearchApp.Repositories;

namespace BasicSearchApp.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentRepository documentRepository;
        private readonly IIndexingService indexingService;
        private readonly IResourceService resourceService;

        /// <summary>
        /// Created 5/8/2020
        /// Author - David Merck
        /// 
        /// This is designed for future use.  It would allow the app to add/remove documents, updating the search index accordingly
        /// This spreads out the indexing processing needs
        /// </summary>
        public DocumentService(IPatientRepository patientRepository, IDocumentRepository documentRepository, IIndexingService indexingService, IResourceService resourceService)
        {
            this.documentRepository = documentRepository;
            this.indexingService = indexingService;
            this.resourceService = resourceService;
        }

        // Adds a document to the repository and optionally indexs it
        public void AddDocument(Document document, bool? updateIndex = true)
        {
            this.documentRepository.Create(document);
            if ((bool)updateIndex)
            {
                this.indexingService.IndexAddDocument(document);
            }
        }

        // Removes a document from the repository and optionally updates the index
        public void RemoveDocument(Document document, bool? updateIndex = true)
        {
            this.documentRepository.Delete(document);
            if ((bool)updateIndex)
            {
                this.indexingService.IndexRemoveDocument(document);
            }
        }

        // Loads document content for the purpose of indexing it.  Includes document name and date
        public string loadDocumentContent(long documentId)
        {
            Document document = this.documentRepository.GetByID(documentId);

            return document.Name + ":::" + document.Date + ":::" + this.resourceService.LoadResource(document.Filename);
        }





    }
}
