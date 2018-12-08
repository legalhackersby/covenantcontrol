using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using src.Models;

namespace src.Service.Document
{
    public class TextParserService : ITextParserService
    {
        private List<string> DateTimeRegexFormats = new List<string>
        {
            @"(?<day>\d{2})\.(?<month>\d{2})\.(?<year>\d{4})"
        };

        private List<string> StringRegexFormats = new List<string>
        {
            @"^.*?(?=\n)"
        };

        public List<CovenantSearchResult> GetCovenantResults(string input)
        {
            // TODO: input should be get from file such as StreamReader. FOr prototype and unit testing it is easy to use a direct plain text.
            var covenantSearchResults= new List<CovenantSearchResult>();

            try
            {
                if (!string.IsNullOrWhiteSpace(input))
                {
                    var covenants = CovenantHelper.GetCovenants();
                    foreach (var covenant in covenants)
                    {
                        foreach (var covenantKeyWord in covenant.KeyWords.OrderByDescending(_ => _.Length))
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
                var newInput = input.Substring(covenantStartIndex, input.Length - covenantStartIndex);
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
                    covenantIndex = i+1;
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
