using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using src.Models;
using src.Models.Covenants;

namespace src.Service.Document
{
    public class ExactMatchCovenantSearchStrategy : ICovenantSearchStrategy
    {
        private List<string> StringRegexFormats = new List<string>
        {
            @"^.*?(?=\n)"
        };

        private List<string> DateTimeRegexFormats = new List<string>
        {
            @"(?<day>\d{2})\.(?<month>\d{2})\.(?<year>\d{4})"
        };

        public SearchSettings SearchSettings { get; set; }

        public List<CovenantSearchResult> Search(string text, string covenantKeyWord, string covenantName)
        {
            var covenantStartIndex = text.IndexOf(covenantKeyWord, StringComparison.OrdinalIgnoreCase);

            if (covenantStartIndex > -1)
            {
                return new List<CovenantSearchResult>
                    {GetCovenantResult(text, covenantStartIndex, covenantName, covenantKeyWord)};
            }

            return null;
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
