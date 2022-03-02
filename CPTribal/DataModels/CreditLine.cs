using System.ComponentModel.DataAnnotations;
using System.Net;

namespace CPTribal.DataModels
{
    public class CreditLine
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
        public bool Accepted { get; set; }
        public double CreditAuthorized { get; set; }
        public int FailIntent { get; set; } = 0;
    }
}
