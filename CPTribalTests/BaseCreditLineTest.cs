using CPTribal.BussinesRules;
using CPTribal.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CPTribalTests
{
    [TestClass]
    public class BaseCreditLineTest
    {
        [TestMethod]
        public void CalculateMonthlyRevenue_ok()
        {
            //Arrange
            CreditParameter creditParam = FillCreditParameter("", "", 0, 100, 0);            
            BaseCreditLine creditLine = new CreditStartup(creditParam);
            
            //Act
            var result = creditLine.CalculateMonthlyRevenue(creditParam.MonthlyRevenue);

            //Assert
            Assert.AreEqual(result, 20);
        }

        [TestMethod]
        public void CalculateMonthlyRevenue_Negative_Equal0()
        {
            //Arrange
            CreditParameter creditParam = FillCreditParameter("", "", 0, -15, 0);
            BaseCreditLine creditLine = new CreditStartup(creditParam);
            
            //Act
            var result = creditLine.CalculateMonthlyRevenue(creditParam.MonthlyRevenue);

            //Assert
            Assert.AreEqual(result, 0);
        }

        [TestMethod]
        public void CalculateCashRevenue_ok()
        {
            //Arrange
            CreditParameter creditParam = FillCreditParameter("", "", 0, 90, 0);
            BaseCreditLine creditLine = new CreditStartup(creditParam);

            //Act
            var result = creditLine.CalculateCashbalance(creditParam.MonthlyRevenue);

            //Assert
            Assert.AreEqual(result, 30);
        }

        [TestMethod]
        public void CalculateCashRevenue_Negative_Equal0()
        {
            //Arrange
            CreditParameter creditParam = FillCreditParameter("", "", 0, -15, 0);
            BaseCreditLine creditLine = new CreditStartup(creditParam);

            //Act
            var result = creditLine.CalculateMonthlyRevenue(creditParam.MonthlyRevenue);

            //Assert
            Assert.AreEqual(result, 0);
        }

        [TestMethod]
        public void CalculateCreditLine_CreditSME_Accepted()
        {
            //Arrange
            CreditParameter creditParam = FillCreditParameter("1", "SME", 1252, 4125, 500);
            BaseCreditLine creditLine = new CreditSME(creditParam);

            //Act
            var (isAccepted, returnCredit) = creditLine.CalculateCreditLine();

            //Assert
            Assert.IsTrue(isAccepted);
            Assert.AreEqual(returnCredit, 500);
        }

        [TestMethod]
        public void CalculateCreditLine_CreditSME_Rejected()
        {
            //Arrange
            CreditParameter creditParam = FillCreditParameter("1", "SME", 1252, 4125, 2000);
            BaseCreditLine creditLine = new CreditSME(creditParam);

            //Act
            var (isAccepted, returnCredit) = creditLine.CalculateCreditLine();

            //Assert
            Assert.IsFalse(isAccepted);
            Assert.AreEqual(returnCredit, 0);
        }

        [TestMethod]
        public void CalculateCreditLine_CreditStartup_Accepted_with_CashBalance()
        {
            //Arrange
            CreditParameter creditParam = FillCreditParameter("1", "Startup", 2760, 4125, 890);
            BaseCreditLine creditLine = new CreditStartup(creditParam);

            //Act
            var (isAccepted, returnCredit) = creditLine.CalculateCreditLine();

            //Assert
            Assert.IsTrue(isAccepted);
            Assert.AreEqual(returnCredit, 920);
        }

        [TestMethod]
        public void CalculateCreditLine_CreditStartup_Accepted_with_MonthlyRevenue()
        {
            //Arrange
            CreditParameter creditParam = FillCreditParameter("1", "Startup", 2760, 5000,890 );
            BaseCreditLine creditLine = new CreditStartup(creditParam);

            //Act
            var (isAccepted, returnCredit) = creditLine.CalculateCreditLine();

            //Assert
            Assert.IsTrue(isAccepted);
            Assert.AreEqual(returnCredit, 1000);
        }

        [TestMethod]
        public void CalculateCreditLine_CreditStartup_Rejected_with_MonthlyRevenue()
        {
            //Arrange
            CreditParameter creditParam = FillCreditParameter("1", "Startup", 2760, 5000, 2300);
            BaseCreditLine creditLine = new CreditStartup(creditParam);

            //Act
            var (isAccepted, returnCredit) = creditLine.CalculateCreditLine();

            //Assert
            Assert.IsFalse(isAccepted);
            Assert.AreEqual(returnCredit, 1000);
        }

        [TestMethod]
        public void CalculateCreditLine_CreditStartup_Rejected_with_CashBalance()
        {
            //Arrange
            CreditParameter creditParam = FillCreditParameter("1", "Startup", 2760, 4125, 2300);
            BaseCreditLine creditLine = new CreditStartup(creditParam);

            //Act
            var (isAccepted, returnCredit) = creditLine.CalculateCreditLine();

            //Assert
            Assert.IsFalse(isAccepted);
            Assert.AreEqual(returnCredit, 920);
        }

        private CreditParameter FillCreditParameter(string id, string foundingType, double cashBalance,
            double monthlyRevenue, double requestCredLine)=>  new CreditParameter()
            {
                Id = id,
                FoundingType = foundingType,
                CashBalance = cashBalance,
                MonthlyRevenue = monthlyRevenue,
                RequestedCreditLine = requestCredLine,
                RequestedDate = DateTime.Now
            };        
    }
}
