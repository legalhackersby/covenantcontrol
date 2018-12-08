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
                "договор действует по",
                "договор действует в течение",
                "Срок действия договора",
                "срок действия",
                "договор действует",
                "Договор действует в течение"
            };
        }
    }
}
