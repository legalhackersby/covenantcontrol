using System.Threading.Tasks;
using src.Service.Upload;
using Xunit;

namespace integration
{
    /// <summary>
    /// The extractor test.
    /// </summary>
    public class ExtractorTest
    {
        [Fact]
        public async Task ConvertAsync_RightParametersIn_FileSuccesfullyConverted()
        {
            string rootFolder = @"D:/shared-src/chaintrack/src/";
            string inputFilePath = @"D:/shared-src/chaintrack/data/2_аренда_хакатон.doc";

            var converted = await new ConvertToTxt(rootFolder).ConvertAsync(inputFilePath);

            Assert.NotNull(converted);
        }
    }
}
