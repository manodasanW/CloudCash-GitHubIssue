using CloudCash.DAL.Data;

namespace CloudCash.DAL.Factories
{
    public interface IDbContextFactory
    {
        CloudCashDbContext CreateDbContext();
    }
}
