using System.Threading.Tasks;
using src.Service.Upload;
using Xunit;

namespace integration
{
    public class ExtractorTest
    {
        [Fact]
        public async Task Word()
        {
            string rootFolder = @"D:/shared-src/chaintrack/src/Converter/";
            string inputFilePath = @"D:/shared-src/chaintrack/data/2_аренда_хакатон.doc";

            var converted = await new ConvertToTxt(rootFolder).ConvertAsync(inputFilePath);

            Assert.NotNull(converted);
        }
    }
}
