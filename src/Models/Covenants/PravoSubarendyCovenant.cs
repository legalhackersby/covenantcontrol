using System.Collections.Generic;

namespace src.Models.Covenants
{
    public class PravoSubarendyCovenant : BaseCovenant
    {
        public PravoSubarendyCovenant()
        {
            CovenantName = CovenantType.PravoSubarendy;

            KeyWords = new List<string>
            {
                "Арендатор не вправе без согласия Арендодателя сдавать арендованное помещение в субаренду",
                "Арендатор не вправе сдавать имущество в субаренду без письменного согласия Арендодателя",
                "субаренда"
            };
        }
    }
}
