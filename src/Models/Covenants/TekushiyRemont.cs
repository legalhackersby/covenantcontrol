using System.Collections.Generic;

namespace src.Models.Covenants
{
    public class TekushiyRemont : BaseCovenant
    {
        public TekushiyRemont()
        {
            CovenantName = CovenantType.TekushiyRemont;

            KeyWords = new List<string>
            {
                "Арендатор обязан не менее одного раза",
                "в течение 30",
                "текущий ремонт"
            };
        }
    }
}
