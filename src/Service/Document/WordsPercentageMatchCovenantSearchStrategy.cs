using System;
using System.Collections.Generic;
using System.Linq;
using LingvoNET;
using Microsoft.Win32;
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
        /// <param name="covenantKeyWord">The covenant key word.</param>
        /// <param name="covenantName">Name of the covenant.</param>
        /// <returns></returns>
        public List<CovenantSearchResult> Search(string text, string covenantKeyWord, string covenantName)
        {
            var covenantList = new List<CovenantSearchResult>();
            //var keyWordsInParagraph = covenantKeyWord.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            //if (keyWordsInParagraph.Length > 0)
            {
                var allTextParagraphs = text.Split('\n', StringSplitOptions.RemoveEmptyEntries).ToList();
                if (allTextParagraphs.Any())
                {
                    foreach (var paragraph in allTextParagraphs)
                    {
                        var keyWordsInParagraph = covenantKeyWord.Split(this.SearchSettings.KeywordSeparator, StringSplitOptions.RemoveEmptyEntries);

                        if (this.SearchSettings.ExctractStemm)
                        {
                            keyWordsInParagraph = keyWordsInParagraph.Select(Stemmer.Stemm).ToArray();
                        }

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

                            if ((double)(wordCountInParagraph / keyWordsInParagraph.Length) * 100 >= this.SearchSettings.AcceptableSearchPercentage)
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

            return covenantList;
        }
    }
}
