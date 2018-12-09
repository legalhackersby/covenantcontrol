using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;
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

        public List<CovenantSearchResult> Search(string text, string covenantKeyWord, string covenantName)
        {
            var covenantList = new List<CovenantSearchResult>();
            //var keyWordsInParagraph = covenantKeyWord.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            //if (keyWordsInParagraph.Length > 0)
            {
                var allTextParagraphs = text.Split('\n', StringSplitOptions.RemoveEmptyEntries).ToList();
                if (allTextParagraphs.Any())
                {

                    {

                        foreach (var paragraph in allTextParagraphs)
                        {
                            var keyWordsInParagraph = covenantKeyWord.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                            if (keyWordsInParagraph.Length > 0)
                            {
                                var wordCountInParagraph = 0;
                                foreach (var keyWord in keyWordsInParagraph)
                                {
                                    if (paragraph.IndexOf(keyWord, StringComparison.OrdinalIgnoreCase) > -1)
                                    {
                                        wordCountInParagraph++;
                                    }
                                }

                                if ((double) (wordCountInParagraph / keyWordsInParagraph.Length) * 100 >=
                                    acceptablePercent)
                                {
                                    var index = text.IndexOf(paragraph, StringComparison.Ordinal);
                                    if (index > -1)
                                    {
                                        covenantList.Add(new CovenantSearchResult
                                        {
                                            CovenantValue = paragraph,
                                            CovenantMathesKeyWord = covenantKeyWord,
                                            CovenantType = covenantName,
                                            StartIndex = index,
                                            EndIndex = index + paragraph.Length
                                        });
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return covenantList;
        }
    }
}
