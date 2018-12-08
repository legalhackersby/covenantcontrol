using System.Collections.Generic;

namespace src.Models.Covenants
{
    public class PoluchenieSoglasovaniyaCovenant : BaseCovenant
    {
        public PoluchenieSoglasovaniyaCovenant()
        {
            CovenantName = CovenantType.PoluchenieSoglasovaniya;

            Keywords = new List<string>
            {
                "предварительного письменного согласования арендодателя",
                "предварительного письменного согласования с арендодателем",
                "письменного согласования арендодателя",
                "письменного согласования с арендодателем",
                "письменного разрешения арендодателя",
                "согласованию с арендодателем",
                "согласованные с арендодателем"
            };
        }
    }
}
