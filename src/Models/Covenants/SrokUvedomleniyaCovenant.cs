using System.Collections.Generic;

namespace src.Models.Covenants
{
    public class SrokUvedomleniyaCovenant : BaseCovenant
    {
        public SrokUvedomleniyaCovenant()
        {
            CovenantName = CovenantType.SrokUvedomleniya;

            KeyWords = new List<string>
            {
                "Арендатор обязан сообщить в письменном виде Арендодателю",
                "Арендатор обязан уведомить Арендодателя",
                "письменное уведомление",
                "Арендатор вправе отказаться от исполнения настоящего договора, направив об этом уведомление",
                "уведомить",
                "письменно"
            };
        }
    }
}