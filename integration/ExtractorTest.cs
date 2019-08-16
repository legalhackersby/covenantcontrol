using System.IO;
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
        private string AppPath => Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName);

        [Fact]
        public async Task ConvertAsync_RightParametersIn_FileSuccesfullyConverted()
        {
            string rootFolder = Path.Combine(AppPath, "src");
            string inputFilePath = Path.Combine(AppPath, "data", "2_аренда_хакатон.doc");

            var converted = await new ConvertToTxt(rootFolder).ExtractTextAsync(inputFilePath);

            Assert.NotNull(converted);
        }
    }
}
