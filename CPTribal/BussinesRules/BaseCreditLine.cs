using CPTribal.Identity;

namespace CPTribal.BussinesRules
{
    public abstract class BaseCreditLine
    {
        public CreditParameter CreditParameter { get; set; }
        public BaseCreditLine(CreditParameter parameter)
        {
            CreditParameter = parameter;
        }

        public abstract (bool isAccepted, double creditLine) CalculateCreditLine();

        public double CalculateMonthlyRevenue(double monthlyRevenue)
        {
            double result = 0.0;

            if (monthlyRevenue > 0)
                result = monthlyRevenue / 5;

            return result;
        }

        public double CalculateCashbalance(double cashBalance)
        {
            double result = 0.0;

            if (cashBalance > 0)
                result = cashBalance / 3;

            return result;
        }
    }

}
