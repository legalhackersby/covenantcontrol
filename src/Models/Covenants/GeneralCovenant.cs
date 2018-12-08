using System.Collections.Generic;

namespace src.Models.Covenants
{
    public class GeneralCovenant : BaseCovenant
    {
        public GeneralCovenant()
        {
            CovenantName = CovenantType.General;

            KeyWords = new List<string>
            {
                "ежемесячно",
                "согласие"
            };
        }
    }
}
