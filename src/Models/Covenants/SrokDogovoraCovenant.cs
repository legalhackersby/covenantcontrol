using System.Collections.Generic;

namespace src.Models.Covenants
{
    public class SrokDogovoraCovenant : BaseCovenant
    {
        public SrokDogovoraCovenant()
        {
            CovenantName = CovenantType.SrokDogovora;

            Keywords = new List<string>
            {
                "Договор действует до",
                "Срок действия договора",
                "срок действия",
                "Договор действует в течение"
            };
        }
    }
}
