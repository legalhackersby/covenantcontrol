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
                "текущего месяца",
                "следующего за отчетным",
                "арендная плата перечисляется",
                "арендная плата оплачивается",
                "арендная плата подлежит оплате",
                "уплачивается",
                "оплачивается",
                "перечисляется",
                "числа месяца, следующего за отчетным"
            };
        }
    }
}
