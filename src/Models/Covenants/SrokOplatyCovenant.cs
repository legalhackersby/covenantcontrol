using System.Collections.Generic;

namespace src.Models.Covenants
{
    public class SrokOplatyCovenant : BaseCovenant
    {
        public SrokOplatyCovenant()
        {
            CovenantName = CovenantType.SrokOplaty;

            KeyWords = new List<string>
            {
                "Арендная плата перечисляется Арендатором ежемесячно",
                "Арендная плата перечисляется на расчетный счет Арендодателя",
                "Арендная плата перечисляется до",
                "ежемесячно",
                "текущего месяца",
                "следующего за отчетным"
            };
        }
    }
}
