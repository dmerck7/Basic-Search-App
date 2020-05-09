using BasicSearchApp.Services;
using System.Web.Http;

namespace BasicSearchAppCSharp.Controllers
{
    /// <summary>
    /// Created 5/8/2020
    /// Author - David Merck
    /// 
    /// Once the UI selects a document header, the document content is returned
    /// </summary>
    [RoutePrefix("contents")]
    public class ContentsController : ApiController
    {
        private readonly IContentService contentService;

        public ContentsController(IContentService contentService)
        {
            this.contentService = contentService;
        }

        /// <summary>
        /// Return document content based on document Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns document content in a wrapper for Ember</returns>
        [HttpGet]
        public dynamic Get(long id)
        {
            return this.contentService.GetContent(id);
        }


    }
}
