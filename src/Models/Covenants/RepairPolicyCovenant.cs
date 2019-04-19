using System.Collections.Generic;

namespace src.Models.Covenants
{
    public class RepairPolicyCovenant : BaseCovenant
    {
        public RepairPolicyCovenant()
        {
            CovenantName = CovenantType.RepairPolicy;

            Keywords = new List<string>
            {
                "all minor repairs are expected to be performed by or at the direction of the Tenant",
                "Any repair that is estimated to cost",
                "Inventory and Inspection Record",
                "necessary repairs",
                "repairs" ,
                "at the direction of the Tenant",
                "Owner take any action to complete the necessary repairs",
                "are scheduled for repair/replacement at regular intervals",
                "TENANT must pay for all repairs"
            };
        }
    }
}
