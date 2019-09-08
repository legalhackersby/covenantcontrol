using System;
using System.Collections.Generic;
using System.Linq;
using LingvoNET;
using src.Models;
using src.Models.Covenants;

namespace src.Service.Document
{
    public class WordsPercentageMatchCovenantSearchStrategy : ICovenantSearchStrategy
    {
        /// <summary>
        /// Gets or sets the search settings.
        /// </summary>
        /// <value>
        /// The search settings.
        /// </value>
        public SearchSettings SearchSettings { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WordsPercentageMatchCovenantSearchStrategy"/> class.
        /// </summary>
        public WordsPercentageMatchCovenantSearchStrategy()
        {
            this.SearchSettings = new SearchSettings();
        }

        public WordsPercentageMatchCovenantSearchStrategy(SearchSettings searchSettings)
        {
            this.SearchSettings = searchSettings;
        }

        /// <summary>
        /// Searches the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="covenantKeyword">The covenant key word.</param>
        /// <param name="covenantName">Name of the covenant.</param>
        /// <returns></returns>
        public List<CovenantSearchResult> Search(string text, string covenantKeyword, string covenantName)
        {
            var covenantList = new List<CovenantSearchResult>();

            var allTextParagraphs = text.Split(this.SearchSettings.ParagraphsSeparators, StringSplitOptions.RemoveEmptyEntries).ToList();

            if (allTextParagraphs.Any())
            {
                foreach (var paragraph in allTextParagraphs)
                {
                    var keyWordsInParagraph = covenantKeyword.Split(this.SearchSettings.KeywordSeparators, StringSplitOptions.RemoveEmptyEntries);

                    if (keyWordsInParagraph.Length > 0)
                    {
                        if (this.SearchSettings.ExctractStemm)
                        {
                            keyWordsInParagraph = keyWordsInParagraph.Select(Stemmer.Stemm).ToArray();
                        }

                        var wordCountInParagraph = 0;

                        foreach (var keyWord in keyWordsInParagraph)
                        {
                            if (paragraph.IndexOf(keyWord, StringComparison.OrdinalIgnoreCase) > -1)
                            {
                                wordCountInParagraph++;
                            }
                        }

                        if ((double)(wordCountInParagraph / keyWordsInParagraph.Length) * 100 >= this.SearchSettings.AcceptableSearchPercentage)
                        {
                            var index = text.IndexOf(paragraph, StringComparison.Ordinal);

                            if (index > -1)
                            {
                                covenantList.Add(new CovenantSearchResult
                                {
                                    CovenantValue = paragraph,
                                    CovenantMathesKeyWord = covenantKeyword,
                                    CovenantType = covenantName,
                                    StartIndex = index,
                                    EndIndex = index + paragraph.Length
                                });
                            }
                        }
                    }
                }
            }

            return covenantList;
        }

        /// <summary>
        /// Searches the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="covenantKeyword">The covenant key word.</param>
        /// <param name="covenantName">Name of the covenant.</param>
        /// <returns></returns>
        public List<CovenantWebSearchResult> SearchWeb(string text, string covenantKeyword, string covenantName)
        {
            string[] parSeps;
            if (text.Contains("\\n"))
            {
                parSeps = new[] { "\\n" };
            }
            else
            {
                parSeps = new[] {"\\n", "."};
            }

            var covenantList = new List<CovenantWebSearchResult>();

            var allTextParagraphs = text.Split(parSeps, StringSplitOptions.RemoveEmptyEntries).ToList();

            if (allTextParagraphs.Any())
            {
                foreach (var paragraph in allTextParagraphs)
                {
                    var keyWordsInParagraph = covenantKeyword.Split(this.SearchSettings.KeywordSeparators, StringSplitOptions.RemoveEmptyEntries);

                    if (keyWordsInParagraph.Length > 0)
                    {
                        if (this.SearchSettings.ExctractStemm)
                        {
                            keyWordsInParagraph = keyWordsInParagraph.Select(Stemmer.Stemm).ToArray();
                        }

                        var wordCountInParagraph = 0;

                        foreach (var keyWord in keyWordsInParagraph)
                        {
                            if (paragraph.IndexOf(keyWord, StringComparison.OrdinalIgnoreCase) > -1)
                            {
                                wordCountInParagraph++;
                            }
                        }

                        if ((double)(wordCountInParagraph / keyWordsInParagraph.Length) * 100 >= this.SearchSettings.AcceptableSearchPercentage)
                        {
                            var index = text.IndexOf(paragraph, StringComparison.Ordinal);

                            if (index > -1)
                            {
                                covenantList.Add(new CovenantWebSearchResult
                                {
                                    CovenantValue = paragraph,
                                    CovenantMathesKeyWord = covenantKeyword,
                                    CovenantType = covenantName,
                                    StartIndex = index,
                                    EndIndex = index + paragraph.Length
                                });
                            }
                        }
                    }
                }
            }

            return covenantList;
        }

        /// <summary>
        /// Splits the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">The items.</param>
        /// <param name="nSize">Size of the n.</param>
        /// <returns></returns>
        public IEnumerable<List<T>> SplitList<T>(List<T> items, int nSize = 1)
        {
            for (int i = 0; i < items.Count; i += nSize)
            {
                yield return items.GetRange(i, Math.Min(nSize, items.Count - i));
            }
        }
    }
}
