using BasicSearchApp.Entities;
using System.Collections.Generic;

namespace BasicSearchApp.Repositories
{
    public interface IDocumentRepository : IRepository<Document>
    {
        IEnumerable<Document> GetAllByPatientId(long patientId);
    }
}
