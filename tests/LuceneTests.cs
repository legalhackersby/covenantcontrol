
using Xunit;
using Lucene.Net;
using Lucene.Net.Search;
using Lucene.Net.Analysis;
using Lucene.Net.Index;
using Lucene.Net.Store;
using Lucene.Net.Search.Spans;
using Lucene.Net.Documents;
using Lucene.Net.Codecs;
using Lucene.Net.Search.Similarities;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Util;

namespace tests
{
    public class LiceneTests
    {
        [Fact]
        public void Stemming()
        {
            var analyzer = new StandardAnalyzer(LuceneVersion.LUCENE_CURRENT);
            var ramDirectory = new RAMDirectory();
            var index = new IndexWriterConfig(LuceneVersion.LUCENE_CURRENT, analyzer);

        }
    }
}
