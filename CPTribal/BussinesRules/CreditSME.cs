using CPTribal.Identity;

namespace CPTribal.BussinesRules
{
    public class CreditSME : BaseCreditLine
    {
        public CreditSME(CreditParameter parameter):base(parameter)
        {
        }

        public override (bool isAccepted, double creditLine) CalculateCreditLine()
        {
            double creditLine = 0.0;
            if (CalculateMonthlyRevenue(CreditParameter.MonthlyRevenue) > CreditParameter.RequestedCreditLine)
            {
                creditLine = CreditParameter.RequestedCreditLine;
                return (true, creditLine);
            }

            return (false, creditLine);
        }
    }
}
