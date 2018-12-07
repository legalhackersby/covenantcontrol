using src.Models;
using System;
using System.Threading.Tasks;

namespace src.Service
{
    public interface IDocumentService
    {
        Task<string> CreateDocument(File file);
    }
}
