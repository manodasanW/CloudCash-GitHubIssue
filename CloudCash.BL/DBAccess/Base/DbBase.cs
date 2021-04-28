using CloudCash.DAL.Data;
using CloudCash.DAL.Factories;

namespace CloudCash.BL.DbAccess.Base
{
    public abstract class DbBase
    {

        protected readonly IDbContextFactory _cloudCashDbContextFactory;

        protected readonly CloudCashDbContext _connection;

        public DbBase(IDbContextFactory cloudCashDbContextFactory)
        {
            _cloudCashDbContextFactory = cloudCashDbContextFactory;
            _connection = _cloudCashDbContextFactory.CreateDbContext();
        }
    }
}
