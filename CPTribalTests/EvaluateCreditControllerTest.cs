using CPTribal.BussinesRules;
using CPTribal.Controllers;
using CPTribal.Data;
using CPTribal.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net.Http;

namespace CPTribalTests
{
    [TestClass]
    public class EvaluateCreditControllerTest
    {
        [TestMethod]
        public async void CreditStartUp_EvaluateCreditRequest_StartUp()
        {
            //Arrange
            CreditParameter creditParam = FillCreditParameter("1", "Startup", 2760, 4125, 890);
            BaseCreditLine creditLine = new CreditStartup(creditParam);
            IDbAccess dbAccess = new DbAccess(GenerateContext());
            ICreditValidation creditValidation = new CreditValidation(creditLine, dbAccess);
            ILogger<EvaluateCreditController> _logger = new Logger<EvaluateCreditController>(new NullLoggerFactory());
            EvaluateCreditController controller = new EvaluateCreditController(_logger, creditValidation);            

            //Act

            var result = await controller.EvaluateCreditRequest(creditParam);
            //result.
            //var response = result as OkObjectResult;
            //response.
            ////Assert
            //Assert.Equals(result.Result.)
        }

        private CreditParameter FillCreditParameter(string id, string foundingType, double cashBalance,
        double monthlyRevenue, double requestCredLine) => new CreditParameter()
        {
            Id = id,
            FoundingType = foundingType,
            CashBalance = cashBalance,
            MonthlyRevenue = monthlyRevenue,
            RequestedCreditLine = requestCredLine,
            RequestedDate = DateTime.Now
        };

        private ApiContext GenerateContext(bool keepTracking = false)
        {
            string DbName = "";
            DbName = Guid.NewGuid().ToString();

            if (keepTracking)
                DbName = "CreditEvaluation";

            var options = new DbContextOptionsBuilder<ApiContext>()
                .UseInMemoryDatabase(DbName).Options;

            var context = new ApiContext(options);
            return context;
        }


    }
}