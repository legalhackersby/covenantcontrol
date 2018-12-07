using src.Data;
using src.Models;
using System;
using System.Threading.Tasks;

namespace src.Service
{
    public class DocumentService : IDocumentService
    {
        public IDocumentProvider _documentProvider;

        public DocumentService(IDocumentProvider documentProvider)
        {
            _documentProvider = documentProvider;
        }

        public async Task<string> CreateDocument(File file)
        {
            var document = new Document
            {
                File = file
            };

            return await _documentProvider.InsertDocumentAsync(document);
        }
    }
}
