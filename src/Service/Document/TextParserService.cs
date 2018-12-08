using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using src.Models;

namespace src.Service.Document
{
    public class TextParserService : ITextParserService
    {
        private const int CovenantLengthWhenValueNotFound = 30;

        private List<string> DateTimeRegexFormats = new List<string>
        {
            @"(?<day>\d{2})\.(?<month>\d{2})\.(?<year>\d{4})"
        };

        public CovenantSearchResult GetCovenantResult(string input, string covenant)
        {
            // TODO: input should be get from file such as StreamReader. FOr prototype and unit testing it is easy to use a direct plain text.
            CovenantSearchResult result = null;

            try
            {
                if (!string.IsNullOrWhiteSpace(input))
                {
                    var covenantIndex = input.IndexOf(covenant, StringComparison.Ordinal);
                    if (covenantIndex > -1)
                    {
                        var newInput = input.Substring(covenantIndex, input.Length - covenantIndex);
                        var match = GetCovenantMatchResult(newInput);

                        var covenantEndIndex = match != null
                            ? covenantIndex + match.Index + match.Length
                            : covenantIndex + CovenantLengthWhenValueNotFound;

                        result = new CovenantSearchResult
                        {
                            StartIndex = covenantIndex,
                            EndIndex = covenantEndIndex,
                            CovenantValue = match != null ? match.Value : null
                        };
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return result;
        }

        private Match GetCovenantMatchResult(string input)
        {
            foreach (var dateTimeRegexFormat in DateTimeRegexFormats)
            {
                Regex rx = new Regex(dateTimeRegexFormat);
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
