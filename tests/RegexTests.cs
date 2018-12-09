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
            var textParserService1 = new TextParserService(new ExactMatchCovenantSearchStrategy());
            var result1 = textParserService1.GetCovenantResults(ContractTextHelper.Contract1);

            var extParserService2 = new TextParserService(new WordsPercentageMatchCovenantSearchStrategy());
            var nextResult = extParserService2.GetCovenantResults(ContractTextHelper.Contract1);

            var result3 = result1.Select(_ => _.StartIndex).Except(nextResult.Select(_ => _.StartIndex)).ToList();

            // exact first covenant in documenst
            var covenantsInOrder = nextResult.OrderBy(x => x.StartIndex).ToArray();
            // discuss why we include or exclude some issues
            var covenant1 = "1.5. Договор вступает в силу с даты подписания сторонами. Срок действия договора устанавливается до 31.08.2019 года.\r";
            var covenant2 = "2.2. Арендная плата по п. 2.1. настоящего договора производится Арендатором ежемесячно, начиная с даты подписания Акта приема-передачи, в срок до 15 (пятнадцатого) числа текущего месяца. Арендатор предоставляет Арендодателю в срок до 20 (двадцатого) числа отчетного месяца копию платежного поручения о перечислении суммы арендной платы по адресу: Республика Беларусь, город Минск, ул. Радужная, 25. Копия платежного поручения должна содержать отметку обслуживающего банка о проведении платежа.\r";
            var covenant3 = "2.3. Размер арендной платы, указанный в п.2.1. настоящего договора, может изменяться в одностороннем порядке по инициативе Арендодателя, но не чаще одного раза в год. О предстоящем изменении размера арендной платы Арендатор извещается Арендодателем не позднее, чем за 30 календарных дней.\r";
            var covenant6 = "3.2.8. Не менее одного раза в 3 года производить за свой счет текущий ремонт в арендуемом Помещении, а также осуществлять ремонт фасада здания и прилегающей территории по согласованию с Арендодателем.\r";
            Assert.Equal(covenant1, covenantsInOrder[0].CovenantValue);
            Assert.Equal(covenant2, covenantsInOrder[1].CovenantValue);
            Assert.Equal(covenant3, covenantsInOrder[2].CovenantValue);
            //Assert.Equal(16, result.Count);
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
            var result = this.textParserService.GetCovenantResults(ContractTextHelper.Contract2);
            Assert.Equal(25, result.Count);
            Assert.True(result.All(_ => _.CovenantValue.Length > 3));
            Assert.True(result.All(_ => _.StartIndex < _.EndIndex));
            Assert.True(result.Distinct().Count() == result.Count);

            // exact first covenant in document
            var covenantsInOrder = result.OrderBy(x => x.StartIndex).ToArray();
            var covenant1 = "Срок действия договора устанавливается до 31.08.2019 года.";
            Assert.Equal(covenant1, covenantsInOrder[0].CovenantValue);
        }

        /// <summary>
        /// Contract3 covenant results.
        /// </summary>
        [Fact]
        public void Contract3_GetCovenantResults()
        {
            var result = this.textParserService.GetCovenantResults(ContractTextHelper.Contract3);
            Assert.Equal(17, result.Count);
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
            var text = ContractTextHelper.Contract1;
            var textParserService1 = new TextParserService(new ExactMatchCovenantSearchStrategy());
            var result = textParserService1.GetCovenantResults(ContractTextHelper.Contract1);

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

            var text1 = ContractTextHelper.Contract1;

            var textParserService2 = new TextParserService(new WordsPercentageMatchCovenantSearchStrategy());
            var result2 = textParserService2.GetCovenantResults(ContractTextHelper.Contract1);

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
            var text1 = @"Sentence 1 Test. Sentence 2. Sentence 3 Test. Sentence 4 Test. Sentence 5. \n";

            var textParserService2 = new TextParserService(new PresizeWordsPercentageMathCovenantSearchStrategy());
            var result2 = textParserService2.GetCovenantResults(ContractTextHelper.Contract1);

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
