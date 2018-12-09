using System.Linq;
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
            this.textParserService = new TextParserService(new WordsPercentageMatchCovenantSearchStrategy(100));
        }

        /// <summary>
        /// Contract1 get covenant results.
        /// </summary>
        [Fact]
        public void Contract1_GetCovenantResults()
        {
            var textParserService1 = new TextParserService(new ExactMatchCovenantSearchStrategy());
            var result1 = textParserService1.GetCovenantResults(ContractTextHelper.Contract1);

            var extParserService2 = new TextParserService(new WordsPercentageMatchCovenantSearchStrategy(100));
            var nextResult = extParserService2.GetCovenantResults(ContractTextHelper.Contract1);

            var result3 = result1.Select(_ => _.StartIndex).Except(nextResult.Select(_ => _.StartIndex)).ToList();

            // exact first covenant in documenst
            var covenantsInOrder = nextResult.OrderBy(x => x.StartIndex).ToArray();
            var covenant1 = "Срок действия договора устанавливается до 31.08.2019 года.";
            Assert.Equal(covenant1, covenantsInOrder[0].CovenantValue);

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
    }
}
