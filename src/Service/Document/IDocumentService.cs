using src.Data;
using src.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace src.Service
{
    public interface IDocumentService
    {
        Task<(string,List<CovenantSearchResult>)> ReadDocument(string  documentId);

        Task<List<CovenantSearchResult>> GetCovenants(string  documentId);
    }
}
