using MongoDB.Bson;

namespace src.Models
{
    public class JsonContentChangesSearchEntity : BaseEntity
    {
        public ObjectId NewParagraphId { get; set; }

        public ObjectId OldParagraphId { get; set; }

        public string ChapterTitle { get; set; }

        public string PageTitle { get; set; }
    }
}
