using System.Collections.Generic;

namespace src.Models.Covenants
{
    /// <summary>
    /// The base covenant.
    /// </summary>
    public class BaseCovenant
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseCovenant"/> class.
        /// </summary>
        public BaseCovenant()
        {
            this.Keywords = new List<string>();
            this.SearchSettings = new SearchSettings();
        }
        /// <summary>
        /// Gets or sets the name of the covenant.
        /// </summary>
        /// <value>
        /// The name of the covenant.
        /// </value>
        public string CovenantName { get; set; }

        /// <summary>
        /// Gets or sets the key words.
        /// </summary>
        /// <value>
        /// The key words.
        /// </value>
        public List<string> Keywords { get; set; }

        /// <summary>
        /// Gets or sets the search settings.
        /// </summary>
        /// <value>
        /// The search settings.
        /// </value>
        public SearchSettings SearchSettings { get; set; }
    }
}
