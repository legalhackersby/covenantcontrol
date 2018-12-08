using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using src.Models;
using src.Models.Covenants;

namespace src.Service.Document
{
    /// <summary>
    /// The text parser service.
    /// </summary>
    /// <seealso cref="src.Service.Document.ITextParserService" />
    public class TextParserService : ITextParserService
    {
        /// <summary>
        /// Gets or sets the covenants.
        /// </summary>
        /// <value>
        /// The covenants.
        /// </value>
        public List<BaseCovenant> Covenants { get; set; }

        /// <summary>
        /// Gets or sets the search settings.
        /// </summary>
        /// <value>
        /// The search settings.
        /// </value>
        public SearchSettings SearchSettings { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextParserService"/> class.
        /// </summary>
        public TextParserService()
        {
            this.Covenants = new List<BaseCovenant>();
            this.SearchSettings = new SearchSettings();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextParserService"/> class.
        /// </summary>
        /// <param name="searchSettings">The search settings.</param>
        public TextParserService(SearchSettings searchSettings)
        {
            this.Covenants = new List<BaseCovenant>();
            this.SearchSettings = searchSettings;
        }

        private List<string> DateTimeRegexFormats = new List<string>
        {
            @"(?<day>\d{2})\.(?<month>\d{2})\.(?<year>\d{4})"
        };

        private List<string> StringRegexFormats = new List<string>
        {
            @"^.*?(?=\n)"
        };

        /// <summary>
        /// Gets the covenant results.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public List<CovenantSearchResult> GetCovenantResults(string input)
        {
            // TODO: input should be gotten from file such as StreamReader. For prototype and unit testing it is easy to use a direct plain text.
            var covenantSearchResults = new List<CovenantSearchResult>();

            this.Covenants.AddRange(CovenantHelper.GetCovenants());

            try
            {
                if (!string.IsNullOrWhiteSpace(input))
                {
                    foreach (var covenant in this.Covenants)
                    {
                        foreach (var covenantKeyWord in covenant.Keywords.OrderByDescending(_ => _.Length))
                        {
                            var covenantStartIndex = input.IndexOf(covenantKeyWord, StringComparison.OrdinalIgnoreCase);
                            if (covenantStartIndex > -1)
                            {
                                var covenantSearchResult = GetCovenantResult(input, covenantStartIndex, covenant.CovenantName, covenantKeyWord);
                                if (covenantSearchResult != null && !covenantSearchResults.Contains(covenantSearchResult))
                                {
                                    covenantSearchResults.Add(covenantSearchResult);
                                }
                            }
                        }
                    }

                    if (this.SearchSettings != null)
                    {

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return covenantSearchResults;
        }

        private CovenantSearchResult GetCovenantResult(string input, int covenantStartIndex, string covenantName, string covenantKeyWord)
        {
            CovenantSearchResult result = null;
            if (covenantStartIndex > -1)
            {
                covenantStartIndex = GetAdjustedStartCovenantIndex(input, covenantStartIndex);
                var newInput = input.Substring(covenantStartIndex, input.Length - covenantStartIndex).TrimStart();
                var match = GetCovenantMatchResult(newInput);
                if (match != null)
                {
                    var covenantEndIndex = covenantStartIndex + match.Index + match.Length;

                    result = new CovenantSearchResult
                    {
                        StartIndex = covenantStartIndex,
                        EndIndex = covenantEndIndex,
                        CovenantValue = match.Value,
                        CovenantType = covenantName,
                        CovenantMathesKeyWord = covenantKeyWord
                    };
                }
            }

            return result;
        }

        private int GetAdjustedStartCovenantIndex(string input, int covenantIndex)
        {
            for (int i = covenantIndex; i > 0; i--)
            {
                var dotSymbol = input[i];
                if (dotSymbol == '.')
                {
                    covenantIndex = i + 1;
                    break;
                }
            }

            return covenantIndex;
        }

        private Match GetCovenantMatchResult(string input)
        {
            foreach (var regexFormat in StringRegexFormats)
            {
                Regex rx = new Regex(regexFormat);
                Match m = rx.Match(input);
                if (m.Success)
                {
                    return m;
                }
            }

            return null;
        }
    }
}
