using System;
using System.Collections.Generic;
using Lucene.Net.Support.C5;

namespace src.Models
{
    public class TestEntity : BaseEntity
    {
        public string Source { get; set; }

        public string PageTitle { get; set; }

        public string ChapterTitle { get; set; }

        public string Body { get; set; }

        public List<TestEntity> SubEntities { get; set; }
    }
}
