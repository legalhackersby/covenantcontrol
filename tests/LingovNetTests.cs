
using Xunit;

using System;
using LingvoNET;
using System.Text;

namespace tests
{
    public class LingvoNetTests
    {
        [Fact]
        public void Stemming()
        {
             // issues https://github.com/PavelTorgashov/LingvoNET/issues/3#issuecomment-445518711
              Assert.Equal("согласовыва", LingvoNET.Stemmer.Stemm("согласовывать"));
              Assert.Equal("арендодател", LingvoNET.Stemmer.Stemm("арендодателем"));
              Assert.Equal("перепланировк", LingvoNET.Stemmer.Stemm("перепланировки"));
              Assert.Equal("субаренду", LingvoNET.Stemmer.Stemm("субаренду"));
              Assert.Equal("арендатор", LingvoNET.Stemmer.Stemm("Арендатор"));
              Assert.Equal("действует", LingvoNET.Stemmer.Stemm("действует"));
              Assert.Equal("оплачива", LingvoNET.Stemmer.Stemm("оплачивается"));
        }

        [Fact]
        public void Source()
        {
             Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
             var words = Analyser.FindSimilarSourceForm("арендодателем");
 
        }
    }
}
