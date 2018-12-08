using System;
using System.Net.Http;
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
            var converter = new ConvertToTxt(@"D:/shared-src/chaintrack/src/Converter/", "", "");
            var inputFilePath = @"D:/shared-src/chaintrack/data/2_аренда_хакатон.doc";
            var outputResultDirectory = @"D:/shared-src/chaintrack/data/output";

            var converted = await converter.Convert(inputFilePath, outputResultDirectory);

            Assert.NotNull(converted);
        }
    }
}

