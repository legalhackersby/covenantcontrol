using System.Collections.Generic;

namespace src.Models.Covenants
{
    public class PaymentOfRentCovenant : BaseCovenant
    {
        public PaymentOfRentCovenant()
        {
            CovenantName = CovenantType.PaymentOfRent;

            Keywords = new List<string>
            {
                "Tenant agrees to pay",
                "due and payable monthly in advance on the",
                "Rent must be received by",
                "has not been received by",
                "may be paid by check",
                "single payment will be accepted",
                "Tenant will add",
                "Tenant will pay",
                "Tenant must pay",
                "Tenant must also pay",
                "will be paid for directly by Tenant",
                "rent is not received prior",
                "will be responsible for payment"
            };
        }
    }
}
