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
            this.textParserService = new TextParserService(new ExactMatchCovenantSearchStrategy());
        }

        /// <summary>
        /// Contract1 get covenant results.
        /// </summary>
        [Fact]
        public void Contract1_GetCovenantResults()
        {
            var result = this.textParserService.GetCovenantResults(ContractTextHelper.Contract1);
            Assert.Equal(16, result.Count);
            Assert.True(result.All(_ => _.CovenantValue.Length > 3));
            Assert.True(result.All(_ => _.StartIndex < _.EndIndex));
            Assert.True(result.Distinct().Count() == result.Count);
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
