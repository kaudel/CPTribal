using System.Net;

namespace CPTribal.Identity
{
    public class ResponseCreditLine
    {
        public string? Message { get; set; }
        public double CreditLine { get; set; } = 0.0;
        public HttpStatusCode ReturnCode { get; set; }
    }
}
