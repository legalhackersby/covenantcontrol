using System.Collections.Generic;

namespace src.Models.Covenants
{
    public class TenantResponsibilityCovenant : BaseCovenant
    {
        public TenantResponsibilityCovenant()
        {
            CovenantName = CovenantType.TenantResponsibility;

            Keywords = new List<string>
            {
                "tenant responsibility",
                "the tenant agrees to",
                "the tenant agrees not to"
            };
        }
    }
}
