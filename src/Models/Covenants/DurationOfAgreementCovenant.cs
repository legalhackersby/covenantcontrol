using System.Collections.Generic;

namespace src.Models.Covenants
{
    public class DurationOfAgreementCovenant : BaseCovenant
    {
        public DurationOfAgreementCovenant()
        {
            CovenantName = CovenantType.DurationOfAgreementCovenant;

            Keywords = new List<string>
            {
                "Commencing on the",
                "starting on",
                "ending on"
            };
        }
    }
}
