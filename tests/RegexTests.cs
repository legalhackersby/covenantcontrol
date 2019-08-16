using System;
using System.IO;
using System.Linq;
using src.Models.Covenants;
using src.Service.Document;
using Xunit;

namespace tests
{
    /// <summary>
    /// Regex tests.
    /// </summary>
    public class RegexTests
    {
        /// <summary>
        /// The text parser service.
        /// </summary>
        private readonly ITextParserService textParserService;

        private string TestDataPath => Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, "data", "tests");

        /// <summary>
        /// Initializes a new instance of the <see cref="RegexTests"/> class.
        /// </summary>
        public RegexTests()
        {
            this.textParserService = new TextParserService(new WordsPercentageMatchCovenantSearchStrategy());
        }

        /// <summary>
        /// Contract1 get covenant results.
        /// </summary>
        [Fact]
        public void Contract1_GetCovenantResults()
        {
            var path = Path.Combine(TestDataPath, "1.txt");
            var text = File.ReadAllText(path);

            var textParserService1 = new TextParserService(new ExactMatchCovenantSearchStrategy());
            var result1 = textParserService1.GetCovenantResults(text);

            var extParserService2 = new TextParserService(new WordsPercentageMatchCovenantSearchStrategy());
            var nextResult = extParserService2.GetCovenantResults(text);

            var result3 = result1.Select(_ => _.StartIndex).Except(nextResult.Select(_ => _.StartIndex)).ToList();

            // exact first covenant in documenst
            var covenantsInOrder = nextResult.OrderBy(x => x.StartIndex).ToArray();

            // discuss why we include or exclude some issues
            var covenant1 = "1.5. Договор вступает в силу с даты подписания сторонами. Срок действия договора устанавливается до 31.08.2019 года.\r";
            var covenant2 = "2. РАЗМЕР АРЕНДНОЙ ПЛАТЫ И ПОРЯДОК РАСЧЕТОВ\r";
            var covenant3 = "2.1. Сумма ежемесячной арендной платы составляет 2 000 (две тысячи)  российских рублей без НДС.. НДС уплачивается в порядке, установленном международными договорами, участниками которых являются Российская Федерация и Республика Беларусь. Расчет стоимости арендной платы прилагается (Приложение №5).\r";
            Assert.Equal(covenant1, covenantsInOrder[0].CovenantValue);
            Assert.Equal(covenant2, covenantsInOrder[1].CovenantValue);
            Assert.Equal(covenant3, covenantsInOrder[2].CovenantValue);
            //Assert.True(result.All(_ => _.CovenantValue.Length > 3));
            //Assert.True(result.All(_ => _.StartIndex < _.EndIndex));
            //Assert.True(result.Distinct().Count() == result.Count);
        }

        /// <summary>
        /// Contract2 covenant results.
        /// </summary>
        [Fact]
        public void Contract2_GetCovenantResults()
        {
            var path = Path.Combine(TestDataPath, "2.txt");
            var text = File.ReadAllText(path);

            var result = this.textParserService.GetCovenantResults(text);
            Assert.Equal(93, result.Count);
            Assert.True(result.All(_ => _.CovenantValue.Length > 3));
            Assert.True(result.All(_ => _.StartIndex < _.EndIndex));
            Assert.True(result.Distinct().Count() == result.Count);

            // exact first covenant in document
            var covenantsInOrder = result.OrderBy(x => x.StartIndex).ToArray();
            Assert.Contains("«Индекс» – Европейский Средний Общий индекс", covenantsInOrder[0].CovenantValue);
        }

        /// <summary>
        /// Contract3 covenant results.
        /// </summary>
        [Fact]
        public void Contract3_GetCovenantResults()
        {
            var path = Path.Combine(TestDataPath, "3.txt");
            var text = File.ReadAllText(path);

            var result = this.textParserService.GetCovenantResults(text);
            Assert.Equal(22, result.Count);
            Assert.True(result.All(_ => _.CovenantValue.Length > 3));
            Assert.True(result.All(_ => _.StartIndex < _.EndIndex));
            Assert.True(result.Distinct().Count() == result.Count);
        }

        /// <summary>
        /// Contract1 get covenant results.
        /// </summary>
        [Fact]
        public void Contract1_GetCovenantResults_Strategy()
        {
            var path = Path.Combine(TestDataPath, "1.txt");
            var text = File.ReadAllText(path);

            var textParserService1 = new TextParserService(new ExactMatchCovenantSearchStrategy());
            var result = textParserService1.GetCovenantResults(text);

            foreach (var covenantSearchResult in result)
            {
                text = text.Replace(covenantSearchResult.CovenantValue, $"<mark>{covenantSearchResult.CovenantValue}</mark>");
            }

            using (var sw = new StreamWriter($@"d:\TestCovenant_Exact_{Guid.NewGuid().ToString("N")}.html"))
            {
                var newText = $@"<html>
<body>
{text}
</body>
</html>";
                sw.WriteLine(newText);
                sw.Flush();
            }

            var text1 = File.ReadAllText(path);

            var textParserService2 = new TextParserService(new WordsPercentageMatchCovenantSearchStrategy());
            var result2 = textParserService2.GetCovenantResults(text1);

            foreach (var covenantSearchResult in result2)
            {
                text1 = text1.Replace(covenantSearchResult.CovenantValue, $"<mark>{covenantSearchResult.CovenantValue}</mark>");
            }

            using (var sw = new StreamWriter($@"d:\TestCovenant_PercentStrategy_{Guid.NewGuid():N}.html"))
            {
                var newText = $@"<html>
<body>
{text1}
</body>
</html>";
                sw.WriteLine(newText);
                sw.Flush();
            }
            //Assert.Equal(16, result.Count);
            //Assert.True(result.All( => .CovenantValue.Length > 3));
            //Assert.True(result.All( => .StartIndex < _.EndIndex));
            //Assert.True(result.Distinct().Count() == result.Count);
        }

        /// <summary>
        /// Contract1 get covenant results.
        /// </summary>
        [Fact]
        public void Contract1_GetCovenantResults_Strategy2()
        {
            var path = Path.Combine(TestDataPath, "1.txt");
            var text = File.ReadAllText(path);

            var text1 = @"Sentence 1 Test. Sentence 2. Sentence 3 Test. Sentence 4 Test. Sentence 5. \n";

            var textParserService2 = new TextParserService(new PresizeWordsPercentageMathCovenantSearchStrategy());
            var result2 = textParserService2.GetCovenantResults(text);

            foreach (var covenantSearchResult in result2)
            {
                text1 = text1.Replace(covenantSearchResult.CovenantValue, $"<mark>{covenantSearchResult.CovenantValue}</mark>");
            }

            using (var sw = new StreamWriter($@"d:\TestCovenant_PercentStrategy_{Guid.NewGuid():N}.html"))
            {
                var newText = $@"<html>
<body>
{text1}
</body>
</html>";
                sw.WriteLine(newText);
                sw.Flush();
            }
            //Assert.Equal(16, result.Count);
            //Assert.True(result.All( => .CovenantValue.Length > 3));
            //Assert.True(result.All( => .StartIndex < _.EndIndex));
            //Assert.True(result.Distinct().Count() == result.Count);
        }
    }
}
