using System.Collections.Generic;

namespace src.Models.Covenants
{
    public class KapitalniyRemontCovenant : BaseCovenant
    {
        public KapitalniyRemontCovenant()
        {
            CovenantName = CovenantType.KapitalniyRemont;

            KeyWords = new List<string>
            {
                "Арендодатель обязан обеспечивать надлежащее функционирование систем жизнеобеспечения и инженерных систем Помещения",
                "обеспечивать надлежащее функционирование систем жизнеобеспечения и инженерных систем Помещения",
                "капитальный ремонт"
            };
        }
    }
}
