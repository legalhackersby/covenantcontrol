using System.Collections.Generic;

namespace src.Models.Covenants
{
    public class AlterationsCovenant : BaseCovenant
    {
        public AlterationsCovenant()
        {
            CovenantName = CovenantType.Alterations;

            Keywords = new List<string>
            {
                "alterations",
                "additions",
                "improvements",
                "decorations"
            };
        }
    }
}
