using BasicSearchApp.Services;
using System.Web.Http;

namespace BasicSearchAppCSharp.Controllers
{
    /// <summary>
    /// Created 5/8/2020
    /// Author - David Merck
    /// 
    /// Used by Ember, hopfully Angular to search/filter documents
    /// </summary>
    [RoutePrefix("documents")]
    public class DocumentsController : ApiController
    {
        private readonly IBasicSearchService basicSearchService;

       public DocumentsController(IBasicSearchService basicSearchService)
        {
            this.basicSearchService = basicSearchService;
        }

        /// <summary>
        /// Recieving a query from the front end return associated document headers
        /// </summary>
        /// <param name="query"></param>
        /// <returns>Document header list in a wrapper for Ember</returns>
        [HttpGet]
        public dynamic Get(string query)
        {
            return this.basicSearchService.Filter(query);
        }


    }
}
