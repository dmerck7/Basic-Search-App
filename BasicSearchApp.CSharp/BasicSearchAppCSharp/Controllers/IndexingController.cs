using BasicSearchApp.data.Context;
using BasicSearchApp.Entities;
using BasicSearchApp.Services;
using System.Web.Http;

namespace BasicSearchAppCSharp.Controllers
{
    [RoutePrefix("indexing")]
    public class IndexingController : ApiController
    {
        private readonly IIndexingService indexingService;

        /// <summary>
        /// Created 5/8/2020
        /// Author - David Merck
        /// 
        /// Used by extenal tools for modifiing the search index for the documents recources forlder
        /// Currently index is a Static varialble found in DbContext and read initially from index.json
        /// TODO - move index to DB for persistent storage
        ///</summary>
        /// <param name="indexingService"></param>
        public IndexingController(IIndexingService indexingService)
        {
            this.indexingService = indexingService;
        }

        /// <summary>
        /// External interface to rebuild the documents index for the resources folder.  This can be used for speed tests
        /// This can be called form postman and saved to index.json in the resources forlder for the ininial load
        /// http://localhost:65522/indexing/IndexAll
        /// </summary>
        /// <returns>SearchIndex</returns>
        [HttpGet]
        [ActionName("IndexAll")]
        [Route("IndexAll")]
        public IHttpActionResult IndexAll()
        {
            this.indexingService.IndexAll();

            return Ok(DbContext.Index);
        }

        /// <summary>
        /// Utilized by external applications adding documents to the resources folder.
        /// Updates the document index for searching
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("IndexAddDocument")]
        [Route("IndexAddDocument")]
        public IHttpActionResult IndexAddDocument(Document document)
        {
            this.indexingService.IndexAddDocument(document);

            //return Ok(DbContext.Index);
            return Ok();
        }

        /// <summary>
        /// Utilized by external applications that remove a document from the document resources folder and need update the index
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        [HttpDelete]
        [ActionName("IndexRemoveDocument")]
        [Route("IndexRemoveDocument")]
        public IHttpActionResult IndexRemoveDocument(Document document)
        {
            this.indexingService.IndexRemoveDocument(document);

            //return Ok(DbContext.Index);
            return Ok();
        }

        /// <summary>
        /// Used to delete the current search index whn there is a need to rebuild from scratch
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [ActionName("ClearIndex")]
        [Route("ClearIndex")]
        public IHttpActionResult ClearIndex()
        {
            this.indexingService.ClearIndex();

            //return Ok(DbContext.Index);
            return Ok();
        }


    }
}
