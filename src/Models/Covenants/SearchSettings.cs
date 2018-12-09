namespace src.Models.Covenants
{
    /// <summary>
    /// The covenant search settings.
    /// </summary>
    public class SearchSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchSettings"/> class.
        /// </summary>
        public SearchSettings()
        {
            this.Reverse = false;
            this.ExctractStemm = false;
            this.KeywordSeparators = new[] { ' ' };
            this.ParagraphsSeparators = new[] { '\n' };
            this.AcceptableSearchPercentage = 99;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="SearchSettings"/> is reverse.
        /// </summary>
        /// <value>
        ///   <c>true</c> if reverse; otherwise, <c>false</c>.
        /// </value>
        public bool Reverse { get; set; }

        /// <summary>
        /// Gets or sets the separator.
        /// </summary>
        /// <value>
        /// The separator.
        /// </value>
        public char[] KeywordSeparators { get; set; }

        /// <summary>
        /// Gets or sets the paragraphs separator.
        /// </summary>
        /// <value>
        /// The paragraphs separator.
        /// </value>
        public char[] ParagraphsSeparators { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [exctract stemm].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [exctract stemm]; otherwise, <c>false</c>.
        /// </value>
        public bool ExctractStemm { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [acceptable search percentage].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [acceptable search percentage]; otherwise, <c>false</c>.
        /// </value>
        public int AcceptableSearchPercentage { get; set; }
    }
}
