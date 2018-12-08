using src.Models;

namespace src.Service.Document
{
    public interface ITextParserService
    {
        CovenantSearchResult GetCovenantResult(string text, string covenant);
    }
}
