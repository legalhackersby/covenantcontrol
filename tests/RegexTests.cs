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
            this.textParserService = new TextParserService();
        }

        /// <summary>
        /// Contract1 get covenant results.
        /// </summary>
        [Fact]
        public void Contract1_GetCovenantResults()
        {
            var result = this.textParserService.GetCovenantResults(ContractTextHelper.Contract1);
            Assert.True(result.Count == 16);
            Assert.True(result.All(_ => _.CovenantValue.Length > 3));
        }

        /// <summary>
        /// Contract2 covenant results.
        /// </summary>
        [Fact]
        public void Contract2_GetCovenantResults()
        {
            var result = this.textParserService.GetCovenantResults(ContractTextHelper.Contract2);
            Assert.True(result.Count == 23);
            Assert.True(result.All(_ => _.CovenantValue.Length > 3));
        }

        /// <summary>
        /// Contract3 covenant results.
        /// </summary>
        [Fact]
        public void Contract3_GetCovenantResults()
        {
            var result = this.textParserService.GetCovenantResults(ContractTextHelper.Contract3);
            Assert.True(result.Count == 15);
            Assert.True(result.All(_ => _.CovenantValue.Length > 3));
        }
    }
}
