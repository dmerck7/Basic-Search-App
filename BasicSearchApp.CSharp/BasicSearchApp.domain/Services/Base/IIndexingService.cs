
using BasicSearchApp.Entities;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace BasicSearchApp.Services
{
    public interface IIndexingService
    {
        void ClearIndex();

        void IndexAll();

        void IndexAddDocument(Document document);

        void IndexRemoveDocument(Document document);
    }
}
