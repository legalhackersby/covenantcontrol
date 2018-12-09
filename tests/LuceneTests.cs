
using Xunit;
using Lucene.Net;
using Lucene.Net.Search;
using Lucene.Net.Analysis;
using Lucene.Net.Index;
using Lucene.Net.Store;
using Lucene.Net.Search.Spans;
using Lucene.Net.Documents;
using Lucene.Net.Codecs;
using Lucene.Net.Analysis.Query;
using Lucene.Net.Analysis.Hunspell;
using Lucene.Net.Search.Similarities;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Util;
using System;
using Lucene.Net.QueryParsers.Classic;

namespace tests
{
    public class LiceneTests
    {
        [Fact]
        public void MultiDistanceAndFuzzyMatchSplitByParagraph()
        {
            var analyzer = new StandardAnalyzer(LuceneVersion.LUCENE_CURRENT);
            var directory = new RAMDirectory();
            var config = new IndexWriterConfig(LuceneVersion.LUCENE_CURRENT, analyzer);
            var index = new IndexWriter(directory, config);

            var documentText = "1.5. Договор вступает в силу с даты подписания сторонами. Срок действия договора устанавливается до 31.08.2019 года.";
            var covenant = "Срок действия договора устанавливается до 31.08.2019 года.";
            var keywords = new []{"Договор", "вступает", "в силу"};
            var document = new Document();
            document.AddTextField("paragraph", documentText, Field.Store.YES); 
            index.AddDocument(document);
            index.Dispose();
            var reader = DirectoryReader.Open(directory);
            var searcher = new IndexSearcher(reader);
            var queryParser = new QueryParser(LuceneVersion.LUCENE_CURRENT, "paragraph", analyzer);
            //var query = new RegexpQuery(new Term("keyword", "Договр"));
            var query = queryParser.Parse("Договор");
            var regexResults = searcher.Search(query, 1);
            var scoredDocs = regexResults.ScoreDocs;
            Assert.True(scoredDocs.Length > 0);
            foreach(var doc in scoredDocs)
            {
                var data = searcher.Doc(doc.Doc);
                var found = data.Get("paragraph");
                Console.WriteLine(found);
            }
        }
    }
}
