using src.Models;
using src.Models.Covenants;

namespace src.Service.Document
{
    public class WordsPercentageMatchCovenantSearchStrategy : ICovenantSearchStrategy
    {
        public SearchSettings SearchSettings { get; set; }

        public WordsPercentageMatchCovenantSearchStrategy()
        {
        }

        public CovenantSearchResult Search(string text, string covenantKeyWord, string covenantName)
        {
            return null;
        }
    }
}
