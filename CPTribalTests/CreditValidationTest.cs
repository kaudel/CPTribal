using CPTribal.BussinesRules;
using CPTribal.Data;
using CPTribal.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;

namespace CPTribalTests
{
    [TestClass]
    public class CreditValidationTest
    {
        [TestMethod]
        public void EvaluateCredit_CreditSME_Accepted()
        {            
            //Arrange
            CreditParameter creditParam = FillCreditParameter("1", "SME", 1252, 4125, 500);
            BaseCreditLine creditLine = new CreditSME(creditParam);
            IDbAccess dbAccess = new DbAccess(GenerateContext());
            CreditValidation creditValidation = new CreditValidation(creditLine, dbAccess);

            //Act
            ResponseCreditLine result = creditValidation.EvaluateCredit(creditLine);

            //Assert
            Assert.AreEqual(500, result.CreditLine);
            Assert.AreEqual("Credit application accepted", result.Message);
            Assert.AreEqual(HttpStatusCode.Created, result.ReturnCode);
        }

        [TestMethod]
        public void EvaluateCredit_CreditStartup_Accepted_with_CashBalance()
        {
            //Arrange
            CreditParameter creditParam = FillCreditParameter("1", "Startup", 2760, 4125, 890);
            BaseCreditLine creditLine = new CreditStartup(creditParam);
            IDbAccess dbAccess = new DbAccess(GenerateContext());
            CreditValidation creditValidation = new CreditValidation(creditLine, dbAccess);

            //Act
            ResponseCreditLine result = creditValidation.EvaluateCredit(creditLine);

            //Assert
            Assert.AreEqual(920, result.CreditLine);
            Assert.AreEqual("Credit application accepted", result.Message);
            Assert.AreEqual(HttpStatusCode.Created, result.ReturnCode);
        }

        [TestMethod]
        public void EvaluateCredit_CreditStartup_Accepted_with_MonthlyRevenue()
        {
            //Arrange
            CreditParameter creditParam = FillCreditParameter("1", "Startup", 2760, 5000, 890);
            BaseCreditLine creditLine = new CreditStartup(creditParam);
            IDbAccess dbAccess = new DbAccess(GenerateContext());
            CreditValidation creditValidation = new CreditValidation(creditLine, dbAccess);

            //Act
            ResponseCreditLine result = creditValidation.EvaluateCredit(creditLine);

            //Assert
            Assert.AreEqual(1000, result.CreditLine);
            Assert.AreEqual("Credit application accepted", result.Message);
            Assert.AreEqual(HttpStatusCode.Created, result.ReturnCode);
        }

        [TestMethod]
        [DataRow(2760, 4125, 890, 920)] //CashBalance
        [DataRow(2760, 5000, 890, 1000)] //MonthlyRevenue
        public void EvaluateCredit_CreditStartup_Accepted(double cashbalance, 
                                                          double monthlyRevenue, 
                                                          double requestedCreditLine,
                                                          double creditLineResult)
        {
            //Arrange
            CreditParameter creditParam = FillCreditParameter("1", "Startup", cashbalance, monthlyRevenue, requestedCreditLine);
            BaseCreditLine creditLine = new CreditStartup(creditParam);
            ApiContext context = GenerateContext();
            IDbAccess dbAccess = new DbAccess(context);
            CreditValidation creditValidation = new CreditValidation(creditLine, dbAccess);

            //Act
            ResponseCreditLine result = creditValidation.EvaluateCredit(creditLine);

            //Assert
            Assert.AreEqual(creditLineResult, result.CreditLine);
            Assert.AreEqual("Credit application accepted", result.Message);
            Assert.AreEqual(HttpStatusCode.Created, result.ReturnCode);
        }

        [TestMethod]
        [DataRow(2760,4125,2300)] // MonthlyRevenue
        [DataRow(2760, 5000, 2300)] //CashBalance
        public void EvaluateCredit_CreditStartup_Reject(double cashbalance, 
                                                        double monthlyRevenue, 
                                                        double requestedCreditLine )
        {
            //Arrange
            CreditParameter creditParam = FillCreditParameter("1", "Startup", cashbalance, monthlyRevenue, requestedCreditLine);
            BaseCreditLine creditLine = new CreditStartup(creditParam);
            ApiContext context = GenerateContext();
            IDbAccess dbAccess = new DbAccess(context);
            CreditValidation creditValidation = new CreditValidation(creditLine, dbAccess);

            //Act
            ResponseCreditLine result = creditValidation.EvaluateCredit(creditLine);

            //Assert
            Assert.AreEqual(0, result.CreditLine);
            Assert.AreEqual("Credit application not accepted", result.Message);
            Assert.AreEqual(HttpStatusCode.OK, result.ReturnCode);
        }


        [TestMethod]
        public void EvaluateCredit_Reject_FailIntent3()
        {
            //Arrange
            CreditParameter creditParam = FillCreditParameter("1", "Startup", 2760, 5000, 2300);
            BaseCreditLine creditLine = new CreditStartup(creditParam);
            IDbAccess dbAccess = new DbAccess(GenerateContext(true));
            CreditValidation creditValidation = new CreditValidation(creditLine, dbAccess);

            //Act
            ResponseCreditLine result = creditValidation.EvaluateCredit(creditLine);
            result = creditValidation.EvaluateCredit(creditLine);
            result = creditValidation.EvaluateCredit(creditLine);

            //Assert
            Assert.AreEqual(0, result.CreditLine);
            Assert.AreEqual("A sales agent will contact you", result.Message);
            Assert.AreEqual(HttpStatusCode.OK, result.ReturnCode);
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

        private ApiContext GenerateContext(bool keepTracking =false)
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
