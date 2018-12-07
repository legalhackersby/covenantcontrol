using src.Models;
using System.Threading.Tasks;

namespace src.Data
{
    public interface IDocumentProvider
    {
        Task<Document> GetDocumentAsync(string id);
        Task<string> InsertDocumentAsync(Document document);
    }
}
