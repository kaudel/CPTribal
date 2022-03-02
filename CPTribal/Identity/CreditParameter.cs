using System.ComponentModel.DataAnnotations;

namespace CPTribal.Identity
{
    public class CreditParameter
    {
        public string Id { get; set; }
        [Required]
        public string? FoundingType { get; set; }
        [Required]
        public double CashBalance { get; set; }
        [Required]
        public double MonthlyRevenue { get; set; }
        [Required]
        public double RequestedCreditLine { get; set; }
        [Required]
        public DateTime RequestedDate { get; set; }

    }
}
