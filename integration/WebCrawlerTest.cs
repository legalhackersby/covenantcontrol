using MongoDB.Driver;
using src.Repository;
using src.Service.Document;
using src.Service.iSwarm;
using Xunit;

namespace integration
{
    public class WebCrawlerTest
    {
        private IWebCrawlerService webCrawlerService;

        public WebCrawlerTest()
        {
            var client = new MongoClient();
            var database = client.GetDatabase("testCovenantControl");
            this.webCrawlerService = new WebCrawlerService(new ChapterMongoRepository(database), new ChangesSearchResultMongoRepository(database),  new TextParserService(new WordsPercentageMatchCovenantSearchStrategy()),new CovenantsWebRepository(database) );
        }

        [Fact]
        public void Test()
        {
            this.webCrawlerService.HandleData();
        }
    }
}
