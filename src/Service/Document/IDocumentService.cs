using src.Data;
using System;
using System.Threading.Tasks;

namespace src.Service
{
    public interface IDocumentService
    {
        Task<string> ReadDocument(string  documentId);
    }
}
