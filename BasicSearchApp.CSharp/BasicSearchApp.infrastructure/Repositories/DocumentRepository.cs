using BasicSearchApp.data.Context;
using BasicSearchApp.Entities;
using System.Collections.Generic;
using System.Linq;

namespace BasicSearchApp.Repositories
{
    /// <summary>
    /// Created 5/8/2020
    /// Author - David Merck
    /// 
    /// Basic repo functions abstracting the data source
    /// </summary>
    public class DocumentRepository : IDocumentRepository
    {
        public DocumentRepository()
        { }

        public Document GetByID(long id)
        {
            return DbContext.Documents.Where(x => x.Id == id).SingleOrDefault<Document>();
        }

        public IEnumerable<Document> GetAllByPatientId(long patientId)
        {
            return DbContext.Documents.Where(d => d.PatientId == patientId).ToList();
        }

        public IEnumerable<Document> GetAll()
        {
            return DbContext.Documents;
        }

        public void Create(Document document) {
            DbContext.Documents.Add(document);
        }

        public void Update(Document document)
        {
            int index = DbContext.Documents.ToList().FindIndex(m => m.Id == document.Id);
            if (index >= 0)
                DbContext.Documents.ToList()[index] = document;
        }

        public void Delete(Document document)
        {
            DbContext.Documents.Remove(document);
        }


    }
}
