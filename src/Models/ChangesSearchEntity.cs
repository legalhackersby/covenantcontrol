namespace src.Models
{
    public class ChangesSearchEntity : BaseEntity
    {
        public string ChangeValue { get; set; }

        public int StartIndex { get; set; }

        public int EndIndex { get; set; }

        public string ChapterTitle { get; set; }
    }
}
