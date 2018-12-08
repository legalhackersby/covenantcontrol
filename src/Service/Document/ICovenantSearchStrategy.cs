using System.Linq;
using System.Threading.Tasks;
using src.Models;
using src.Models.Covenants;

namespace src.Service.Document
{
    public interface ICovenantSearchStrategy
    {
        /// <summary>
        /// Gets or sets the search settings.
        /// </summary>
        /// <value>
        /// The search settings.
        /// </value>
        SearchSettings SearchSettings { get; set; }

        /// <summary>
        /// Searches the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="covenantKeyWord">The covenant key word.</param>
        /// <param name="covenantName">Name of the covenant.</param>
        /// <returns></returns>
        CovenantSearchResult Search(string text, string covenantKeyWord, string covenantName);
    }
}
