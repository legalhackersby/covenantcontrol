using System.Collections.Generic;
using src.Models.Covenants;

namespace src.Service.Document
{
    public class CovenantHelper
    {
        public static List<BaseCovenant> GetCovenants()
        {
            var list = new List<BaseCovenant>
            {
                new PravoIzmeneniyaSubarendyCovenant(),
                new PravoSubarendyCovenant(),
                new SrokDogovoraCovenant(),
                new SrokOplatyCovenant(),
                new SrokPredostavleniyaSchetovCovenant(),
                new SrokUvedomleniyaCovenant(),
                new Remont(),
                new VozmeshenieEnhancementCovenant(),
                new PoluchenieSoglasovaniyaCovenant(),
                new ReceivingPermissionCovenant(),
                new AlterationsCovenant(),
                new SubRentingCovenant(),
                new RepairPolicyCovenant(),
                new DurationOfAgreementCovenant(),
                new PaymentCovenant(),
                new TerminationCovenant()
            };

            return list;
        }
    }
}
