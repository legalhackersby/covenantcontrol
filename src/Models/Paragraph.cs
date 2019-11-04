using System.Collections.Generic;

namespace src.Models
{
    public class Paragraph
    {
        public string Type { get; set; }

        public string Text { get; set; }

        public string HeaderLevel { get; set; }

        public IEnumerable<Paragraph> SubParagraphs { get; set; }
    }
}
