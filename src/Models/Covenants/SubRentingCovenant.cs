using System.Collections.Generic;

namespace src.Models.Covenants
{
    public class SubRentingCovenant : BaseCovenant
    {
        public SubRentingCovenant()
        {
            CovenantName = CovenantType.SubRenting;

            Keywords = new List<string>
            {
                "allow any other person to live",
                "Resident agrees not to assign this agreement",
                "appropriate surcharge",
                "sublease",
                "single family residence",
                "notices provided",
                "By no means may Tenant allow any additional persons to occupy premise"
            };
        }
    }
}
