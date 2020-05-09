
using BasicSearchApp.Entities;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace BasicSearchApp.Services
{
    public interface IDocumentService
    {
        void AddDocument(Document document, bool? updateIndex=true);

        void RemoveDocument(Document document, bool? updateIndex=false);

        string loadDocumentContent(long documentId);
    }
}
