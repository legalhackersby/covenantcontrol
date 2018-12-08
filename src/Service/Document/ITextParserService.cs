using System.Collections.Generic;
using src.Models;

namespace src.Service.Document
{
    public interface ITextParserService
    {
        List<CovenantSearchResult> GetCovenantResults(string text);
    }
}
