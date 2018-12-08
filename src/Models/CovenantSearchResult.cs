namespace src.Models
{
    public class CovenantSearchResult
    {
        /// <summary>
        /// Gets or sets the start covenant index.
        /// </summary>
        /// <value>
        /// The start covenant index.
        /// </value>
        public int StartIndex { get; set; }

        /// <summary>
        /// Gets or sets the end covenant index.
        /// </summary>
        /// <value>
        /// The end covenant index.
        /// </value>
        public int EndIndex { get; set; }

        /// <summary>
        /// Gets or sets the covenant value.
        /// </summary>
        /// <value>
        /// The covenant value.
        /// </value>
        public string CovenantValue { get; set; }
    }
}
