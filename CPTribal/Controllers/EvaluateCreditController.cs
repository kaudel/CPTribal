using CPTribal.BussinesRules;
using CPTribal.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CPTribal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EvaluateCreditController : ControllerBase
    {
        private ICreditValidation _BussinesRules;
        private readonly ILogger<EvaluateCreditController> _logger;

        public EvaluateCreditController(ILogger<EvaluateCreditController> logger, ICreditValidation bussinesRules)
        {
            _logger = logger;
            _BussinesRules = bussinesRules;
        }

        [HttpPost]
        public async Task<IActionResult> EvaluateCreditRequest(CreditParameter parameter)
        {
            if (!ValidateFieldInt(parameter.CashBalance) || !ValidateFieldInt(parameter.MonthlyRevenue) || !ValidateFieldInt(parameter.RequestedCreditLine))
            {
                return BadRequest("Parameter CashBalance and MonthlyRevenue  needs to be greater than 0");
            }

            ResponseCreditLine response = null;

            if (Enum.TryParse<FoundingType>(parameter.FoundingType, ignoreCase: true, out var foundingSelected))
            {
                switch (foundingSelected)
                {
                    case FoundingType.SME:
                        response = _BussinesRules.EvaluateCredit(new CreditSME(parameter));
                        break;
                    case FoundingType.STARTUP:
                        response = _BussinesRules.EvaluateCredit(new CreditStartup(parameter));
                        break;
                    default:
                        break;                        
                }
            }
            else
            {
                return BadRequest("Founding Type parameter not defined correctly");
            }

            return new JsonResult(response);
        }

        //Todo: Hacerlo con genericos
        private bool ValidateFieldInt(double field)
        {
            return field > 0 ? true : false;
        }
    }
}
