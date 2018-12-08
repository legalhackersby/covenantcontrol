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
            this.KeywordsCountPerAttempt = 1;
            this.Reverse = false;
            this.Separator = " ";
        }

        /// <summary>
        /// Gets or sets the keywords count per attempt.
        /// </summary>
        /// <value>
        /// The keywords count per attempt.
        /// </value>
        public int KeywordsCountPerAttempt { get; set; }

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
        public string Separator { get; set; }
    }
}
