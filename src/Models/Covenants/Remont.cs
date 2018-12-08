using System.Collections.Generic;

namespace src.Models.Covenants
{
    public class Remont : BaseCovenant
    {
        public Remont()
        {
            CovenantName = CovenantType.Remont;

            KeyWords = new List<string>
            {
                "Арендатор обязан не менее одного раза",
                "текущий ремонт",
                "Арендодатель обязан обеспечивать надлежащее функционирование систем жизнеобеспечения и инженерных систем Помещения",
                "обеспечивать надлежащее функционирование систем жизнеобеспечения и инженерных систем Помещения",
                "капитальный ремонт"
            };
        }
    }
}
