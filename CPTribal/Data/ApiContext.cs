using CPTribal.DataModels;
using Microsoft.EntityFrameworkCore;

namespace CPTribal.Data
{
    public class ApiContext: DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options) : base(options)
        {
            
        }
        public DbSet<CreditLine> CreditLine { get; set; }
    }
}
