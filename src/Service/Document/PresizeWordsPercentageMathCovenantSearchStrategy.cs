using System;
using System.Collections.Generic;
using System.Linq;
using LingvoNET;
using src.Models;
using src.Models.Covenants;

namespace src.Service.Document
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="src.Service.Document.ICovenantSearchStrategy" />
    public class PresizeWordsPercentageMathCovenantSearchStrategy : ICovenantSearchStrategy
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
        public PresizeWordsPercentageMathCovenantSearchStrategy()
        {
            this.SearchSettings = new SearchSettings();
        }

        public PresizeWordsPercentageMathCovenantSearchStrategy(SearchSettings searchSettings)
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
        /// <exception cref="NotImplementedException"></exception>
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
                            var sentences = paragraph.Split(this.SearchSettings.SentenceSeparators);
                            var matchSentences = new Dictionary<int, string>();

                            for (int i = 0; i < sentences.Length; i++)
                            {
                                foreach (var keyword in keyWordsInParagraph)
                                {
                                    if (sentences[i].Contains(keyword, StringComparison.OrdinalIgnoreCase))
                                    {
                                        matchSentences.Add(i, sentences[i]);
                                        break;
                                    }
                                }
                            }

                            if (sentences.Count() == matchSentences.Count())
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
                            else
                            {
                                int iterator = matchSentences.Keys.Min();

                                var items = new Dictionary<Guid, List<string>>();
                                var guid = Guid.NewGuid();

                                foreach (var sentenceKeyValuePair in matchSentences.OrderBy(_ => _.Key))
                                {
                                    if (iterator == sentenceKeyValuePair.Key)
                                    {
                                        this.Do(items, guid, sentenceKeyValuePair);
                                        iterator++;
                                    }
                                    else
                                    {
                                        iterator = sentenceKeyValuePair.Key;

                                        guid = Guid.NewGuid();

                                        this.Do(items, guid, sentenceKeyValuePair);

                                        iterator++;
                                    }
                                }

                                foreach (var item in items)
                                {
                                    var resultSentence = string.Join(this.SearchSettings.SentenceSeparators.First(), item.Value);

                                    var index = text.IndexOf(resultSentence, StringComparison.Ordinal);

                                    if (index > -1)
                                    {
                                        covenantList.Add(new CovenantSearchResult
                                        {
                                            CovenantValue = resultSentence,
                                            CovenantMathesKeyWord = covenantKeyword,
                                            CovenantType = covenantName,
                                            StartIndex = index,
                                            EndIndex = index + resultSentence.Length
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

        public List<CovenantWebSearchResult> SearchWeb(string text, string covenantKeyword, string covenantName)
        {
            throw new NotImplementedException();
        }

        private void Do(Dictionary<Guid, List<string>> items, Guid guid, KeyValuePair<int, string> sentenceKeyValuePair)
        {
            if (items.ContainsKey(guid))
            {
                var list = items[guid];
                list.Add(sentenceKeyValuePair.Value);
            }
            else
            {
                items.Add(guid, new List<string> { sentenceKeyValuePair.Value });
            }
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
