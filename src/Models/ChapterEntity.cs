using System;

namespace src.Models
{
    public class ChapterEntity : BaseEntity
    {
        public string Source { get; set; }

        public string PageTitle { get; set; }

        public string ChapterTitle { get; set; }

        public string Body { get; set; }

        public DateTime CreatedTime { get; set; }
    }
}
