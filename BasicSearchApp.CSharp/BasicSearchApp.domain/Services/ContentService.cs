using BasicSearchApp.Entities;
using BasicSearchApp.Repositories;
using System.Dynamic;

namespace BasicSearchApp.Services
{
    /// <summary>
    /// Created 5/8/2020
    /// Author - David Merck
    /// 
    /// Given a document Id, load and return the content using the resource service
    /// </summary>
    public class ContentService : IContentService
    {
        IDocumentRepository documentRepository;
        IResourceService resourceService;

        public ContentService(IDocumentRepository documentRepository, IResourceService resourceService)
        {
            this.documentRepository = documentRepository;
            this.resourceService = resourceService;
        }

        // Load document content upon request
        public dynamic GetContent(long id)
        {
            Document document = this.documentRepository.GetByID(id);

            dynamic content = new System.Dynamic.ExpandoObject();
            content.Id = id;
            content.Txt = this.resourceService.LoadResource(document.Filename);

            dynamic wrapper = new ExpandoObject();
            wrapper.content = content;

            return wrapper;
        }



    }
}
