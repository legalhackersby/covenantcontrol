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
            var converter = new ConvertToTxt(@"D:/shared-src/chaintrack/src/Converter/");
            var inputFilePath = @"D:/shared-src/chaintrack/data/2_аренда_хакатон.doc";

            var converted = await converter.Convert(inputFilePath);

            Assert.NotNull(converted);
        }
    }
}

