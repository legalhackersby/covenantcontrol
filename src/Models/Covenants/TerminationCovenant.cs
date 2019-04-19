using System.Collections.Generic;

namespace src.Models.Covenants
{
    public class TerminationCovenant : BaseCovenant
    {
        public TerminationCovenant()
        {
            CovenantName = CovenantType.Termination;

            Keywords = new List<string>
            {
                "Termination",
                "terminate the Lease",
                "Termination Date shall be no earlier",
                "Tenant shall vacate",
                "If Tenant fails to vacate",
                "in advance of the date"
            };
        }
    }
}
