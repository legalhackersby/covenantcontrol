using System.Collections.Generic;

namespace src.Models.Covenants
{
    public class PravoIzmeneniyaSubarendyCovenant : BaseCovenant
    {
        public PravoIzmeneniyaSubarendyCovenant()
        {
            CovenantName = CovenantType.PravoIzmeneniyaArendy;

            KeyWords = new List<string>
            {
                "Арендатор обязан не производить без письменного согласия Арендодателя",
                "перепланировка",
                "переоборудование",
                "неотделимые улучшения",
                "ремонтные",
                "строительные",
                "не производить без письменного согласия Арендодателя",
                "перепланировки",
                "переоборудования",
                "неотделимых улучшений",
                "строительных"

            };
        }
    }
}
