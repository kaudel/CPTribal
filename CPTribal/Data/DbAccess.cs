using CPTribal.DataModels;
using CPTribal.Identity;
using Microsoft.EntityFrameworkCore;

namespace CPTribal.Data
{
    public interface IDbAccess
    {
        public CreditLine SelectCreditLine(string id);
        public void AddCreditLine(CreditLine parameters);
        public void UpdateCreditLine(CreditLine parameters);
        public void DeleteCreditLine(CreditLine parameters);
        public bool SaveChanges();
    }

    public class DbAccess:IDbAccess
    {
        private readonly ApiContext _context;

        public DbAccess(ApiContext context)
        {
           _context = context;
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public CreditLine SelectCreditLine(string id)
        {
            return _context.CreditLine.Where(x => x.Id == id).FirstOrDefault();
        }

        public void AddCreditLine(CreditLine parameters)
        {
            _context.CreditLine.Add(parameters);
        }

        public void UpdateCreditLine(CreditLine parameters)
        {
            var entry = _context.CreditLine.FirstOrDefault(x => x.Id == parameters.Id);
            _context.Entry(entry).CurrentValues.SetValues(parameters);
        }

        public void DeleteCreditLine(CreditLine parameters)
        {
            _context.Remove(parameters);
        }
    }
}
