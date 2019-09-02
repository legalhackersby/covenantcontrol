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
        public List<BaseCovenant> Covenants { get; set; }
        private readonly ICovenantSearchStrategy covenantSearchStrategy;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextParserService"/> class.
        /// </summary>
        public TextParserService(ICovenantSearchStrategy covenantSearchStrategy)
        {
            this.covenantSearchStrategy = covenantSearchStrategy;
            this.Covenants = new List<BaseCovenant>();
        }

        public List<CovenantSearchResult> GetCovenantResults(string text)
        {
            // TODO: input should be gotten from file such as StreamReader. For prototype and unit testing it is easy to use a direct plain text.
            var covenantSearchResults = new List<CovenantSearchResult>();
            this.Covenants = CovenantHelper.GetCovenants();

            try
            {
                if (!string.IsNullOrWhiteSpace(text))
                {
                    foreach (var covenant in this.Covenants)
                    {
                        foreach (var covenantKeyWord in covenant.Keywords.OrderByDescending(_ => _.Length))
                        {
                            var covenantStartIndex = text.IndexOf(covenantKeyWord, StringComparison.OrdinalIgnoreCase);

                            if (covenantStartIndex > -1)
                            {
                                var covenantSearchResult = this.covenantSearchStrategy.Search(text, covenantKeyWord, covenant.CovenantName);

                                if (covenantSearchResult != null && covenantSearchResult.Any())// && !covenantSearchResults.Contains(covenantSearchResult))
                                {
                                    covenantSearchResults.AddRange(covenantSearchResult);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return covenantSearchResults.Distinct().ToList();
        }

        public List<CovenantWebSearchResult> GetCovenantWebResults(string text)
        {
            // TODO: input should be gotten from file such as StreamReader. For prototype and unit testing it is easy to use a direct plain text.
            var covenantSearchResults = new List<CovenantWebSearchResult>();
            this.Covenants = CovenantHelper.GetCovenants();

            try
            {
                if (!string.IsNullOrWhiteSpace(text))
                {
                    foreach (var covenant in this.Covenants)
                    {
                        foreach (var covenantKeyWord in covenant.Keywords.OrderByDescending(_ => _.Length))
                        {
                            var covenantStartIndex = text.IndexOf(covenantKeyWord, StringComparison.OrdinalIgnoreCase);

                            if (covenantStartIndex > -1)
                            {
                                var covenantSearchResult = this.covenantSearchStrategy.SearchWeb(text, covenantKeyWord, covenant.CovenantName);

                                if (covenantSearchResult != null && covenantSearchResult.Any())// && !covenantSearchResults.Contains(covenantSearchResult))
                                {
                                    covenantSearchResults.AddRange(covenantSearchResult);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return covenantSearchResults.Distinct().ToList();
        }
    }
}
