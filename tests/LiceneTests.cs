
using Xunit;
using Lucene.Net;
using Lucene.Net.Search;
using Lucene.Net.Analysis;
using Lucene.Net.Index;
using Lucene.Net.Store;
using Lucene.Net.Search.Spans;

namespace tests
{
    public class LiceneTests
    {
        [Fact]
        public void Stemming()
        {
SpanQuery[] clauses = new SpanQuery[3];
clauses[0] = new SpanMultiTermQueryWrapper<FuzzyQuery>(new FuzzyQuery(new Term("contents", "mosa")));
clauses[1] = new SpanMultiTermQueryWrapper<FuzzyQuery>(new FuzzyQuery(new Term("contents", "employee")));
clauses[2] = new SpanMultiTermQueryWrapper<FuzzyQuery>(new FuzzyQuery(new Term("contents", "appreicata")));
SpanNearQuery query = new SpanNearQuery(clauses, 0, true);

        }
    }
}
