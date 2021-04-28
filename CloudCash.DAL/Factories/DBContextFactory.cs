using CloudCash.DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace CloudCash.DAL.Factories
{
    public class DbContextFactory : IDbContextFactory
    {
        public CloudCashDbContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<CloudCashDbContext>();
            optionsBuilder.UseSqlServer("Server = tcp:cloudcash.database.windows.net, 1433; Initial Catalog = CloudCashDB; Persist Security Info = False; User ID = JR; Password =ParaLounge4496; MultipleActiveResultSets = False; Encrypt = True;Database=EFProviders.InMemory; TrustServerCertificate = False; Connection Timeout = 30;");
            return new CloudCashDbContext(optionsBuilder.Options);
        }
    }
}
