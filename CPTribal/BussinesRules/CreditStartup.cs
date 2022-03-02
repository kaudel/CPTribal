using CPTribal.Identity;

namespace CPTribal.BussinesRules
{
    public class CreditStartup : BaseCreditLine
    {
        public CreditStartup(CreditParameter parameter) : base(parameter)
        {
        }

        public override (bool isAccepted, double creditLine) CalculateCreditLine()
        {
            double creditLine = 0.0;

            var cashBalance = CalculateCashbalance(CreditParameter.CashBalance);
            var monthlyRevenue = CalculateMonthlyRevenue(CreditParameter.MonthlyRevenue);

            if (cashBalance > monthlyRevenue)
                creditLine = cashBalance;
            else
                creditLine = monthlyRevenue;


            if( creditLine > CreditParameter.RequestedCreditLine)
                return (true, creditLine);            
            else
                return (false, creditLine);


        }
    }
}
