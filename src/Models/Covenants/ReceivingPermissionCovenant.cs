using System.Collections.Generic;

namespace src.Models.Covenants
{
    public class ReceivingPermissionCovenant : BaseCovenant
    {
        public ReceivingPermissionCovenant()
        {
            CovenantName = CovenantType.ReceivingPermission;

            Keywords = new List<string>
            {
                "written permission",
                "excess written permission",
                "requesting permission",
                "without the written permission",
                "receive permission",
                "24 hour notice",
                "by an agreement in writing",
                "Landlord's prior written consent",
                "notices provided"
            };
        }
    }
}
