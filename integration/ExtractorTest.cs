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
            // TODO: add git lfs to test data
            // TODO: run relative of with portable office?
            var converter = new ConvertToTxt();
            // TODO: fix path
            var example = @"F:\shared-src\chaintrack\data\2_аренда_хакатон.doc";
            var converted = await converter.Convert(example);
            Assert.NotNull(converted);
        }     
    }
}

