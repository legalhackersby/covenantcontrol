using System.Collections.Generic;

namespace src.Models.Covenants
{
    public class GeneralCovenant : BaseCovenant
    {
        public GeneralCovenant()
        {
            CovenantName = CovenantType.General;

            Keywords = new List<string>
            {
                "ежемесячно",
                "согласие"
            };
        }
    }
}
