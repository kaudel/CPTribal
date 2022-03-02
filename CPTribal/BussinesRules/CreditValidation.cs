using CPTribal.Identity;
using CPTribal.DataModels;
using CPTribal.Data;
using System.Net;

namespace CPTribal.BussinesRules
{
    public interface ICreditValidation
    {
        public ResponseCreditLine EvaluateCredit(BaseCreditLine parameters);
        public ResponseCreditLine CreditNotAccepted(CreditLine queryItem, double credit, bool insert);
        public ResponseCreditLine CreditAccepted(CreditLine queryItem, double credit, bool insert);
    }


    public class CreditValidation : ICreditValidation
    {
        private readonly BaseCreditLine _creditLine;
        private readonly IDbAccess _dbAccess;
        public CreditValidation( BaseCreditLine creditLine, IDbAccess dbAccess)
        {
            _creditLine = creditLine;
            _dbAccess = dbAccess;
        }

        public ResponseCreditLine EvaluateCredit(BaseCreditLine creditLine)
        {
            bool newRequest = true;
            ResponseCreditLine response = new ResponseCreditLine();
            CreditLine queryItem = _dbAccess.SelectCreditLine(creditLine.CreditParameter.Id);

            if (queryItem != null)            
                newRequest = false;
            
            if (newRequest)
            {                
                queryItem = MapItem(creditLine.CreditParameter);
                var (isAccepted, returnCredit) = creditLine.CalculateCreditLine();
                if (isAccepted)
                    return CreditAccepted(queryItem, returnCredit, true);
                else
                    return CreditNotAccepted(queryItem, returnCredit,true);
            }
            else
            {
                if (queryItem.Accepted)
                    response = FormatResponse("Credit application accepted", HttpStatusCode.OK, queryItem.CreditAuthorized);
                else
                {
                    if (queryItem.FailIntent >= 3)
                    {
                        response = FormatResponse("A sales agent will contact you", HttpStatusCode.OK, 0);
                        return response;
                    }
                    //Calculate again
                    int failIntents = queryItem.FailIntent;
                    queryItem = MapItem(creditLine.CreditParameter);
                    queryItem.FailIntent = failIntents;

                    var (isAccepted, returnCredit) = creditLine.CalculateCreditLine();
                    if (isAccepted)
                    {
                        queryItem.FailIntent = 0;
                        return CreditAccepted(queryItem, returnCredit, false);
                    }
                    else
                    {
                        response= CreditNotAccepted(queryItem, returnCredit, false);
                        if (queryItem.FailIntent >= 3)
                        {
                            response = FormatResponse("A sales agent will contact you", HttpStatusCode.OK, 0);
                            return response;
                        }

                        return response; 
                    }
                }
                return response;
            } 
        }

        public ResponseCreditLine CreditNotAccepted(CreditLine queryItem, double credit, bool insert)
        {
            ResponseCreditLine response = new ResponseCreditLine();
            queryItem.Accepted = false;
            queryItem.FailIntent += 1;
            response = FormatResponse("Credit application not accepted", HttpStatusCode.OK, 0);
            if (insert)
                _dbAccess.AddCreditLine(queryItem);
            else
            { 
                _dbAccess.UpdateCreditLine(queryItem);
            }

            _dbAccess.SaveChanges();
            return response;
        }
        public ResponseCreditLine CreditAccepted(CreditLine queryItem, double credit, bool insert  )
        {
            ResponseCreditLine response = new ResponseCreditLine();
            queryItem.Accepted = true;
            queryItem.CreditAuthorized = credit;
            response = FormatResponse("Credit application accepted", HttpStatusCode.Created, credit);
            if (insert)
                _dbAccess.AddCreditLine(queryItem);
            else
            {
                _dbAccess.UpdateCreditLine(queryItem);
            }

            _dbAccess.SaveChanges();
            return response;
        }

        private CreditLine MapItem(CreditParameter parameter)
        {
            CreditLine item = new CreditLine(){ 
                Id = parameter.Id,
                CashBalance = parameter.CashBalance,
                FoundingType = parameter.FoundingType,
                MonthlyRevenue = parameter.MonthlyRevenue,
                RequestedCreditLine = parameter.RequestedCreditLine,
                RequestedDate = parameter.RequestedDate                   
            };
            return item;
        }

        private ResponseCreditLine FormatResponse(string message, HttpStatusCode code, double credit)
        {
            ResponseCreditLine response = new ResponseCreditLine()
            {
                Message = message,
                ReturnCode = code,
                CreditLine = credit
            };
            return response;
        }

    }
}
