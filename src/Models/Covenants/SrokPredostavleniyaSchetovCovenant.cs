using System.Collections.Generic;

namespace src.Models.Covenants
{
    public class SrokPredostavleniyaSchetovCovenant : BaseCovenant
    {
        public SrokPredostavleniyaSchetovCovenant()
        {
            CovenantName = CovenantType.SrokPredostavleniyaSchetov;

            Keywords = new List<string>
            {
                "Возмещение стоимости  коммунальных услуг ",
                "счет-фактура",
                "счета",
                "счетов"
            };
        }
    }
}
