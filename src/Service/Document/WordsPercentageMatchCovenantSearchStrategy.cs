using System;
using System.Linq;
using src.Models;
using src.Models.Covenants;

namespace src.Service.Document
{
    public class WordsPercentageMatchCovenantSearchStrategy : ICovenantSearchStrategy
    {
        private readonly int acceptablePercent;

        public SearchSettings SearchSettings { get; set; }

        public WordsPercentageMatchCovenantSearchStrategy(int acceptablePercent)
        {
            this.acceptablePercent = acceptablePercent;
        }

        public CovenantSearchResult Search(string text, string covenantKeyWord, string covenantName)
        {
            var keyWordsInParagraph = covenantKeyWord.Split(" ");
            if (keyWordsInParagraph.Length > 1)
            {
                var allTextParagraphs = text.Split('\n', StringSplitOptions.RemoveEmptyEntries);
                if (allTextParagraphs.Any())
                {
                    foreach (var paragraph in allTextParagraphs)
                    {
                        var wordCountInParagraph = 0;
                        foreach (var keyWord in keyWordsInParagraph)
                        {
                            if (paragraph.IndexOf(keyWord, StringComparison.OrdinalIgnoreCase) > -1)
                            {
                                wordCountInParagraph++;
                            }
                        }

                        if ((double)(wordCountInParagraph / keyWordsInParagraph.Length) * 100 >= acceptablePercent)
                        {
                            var index = text.IndexOf(paragraph, StringComparison.Ordinal);
                            if (index > -1)
                            {
                                return new CovenantSearchResult
                                {
                                    CovenantValue = paragraph,
                                    CovenantMathesKeyWord = covenantKeyWord,
                                    CovenantType = covenantName,
                                    StartIndex = index,
                                    EndIndex = index + paragraph.Length
                                };
                            }
                        }
                    }
                }
            }

            return null;
        }
    }
}
