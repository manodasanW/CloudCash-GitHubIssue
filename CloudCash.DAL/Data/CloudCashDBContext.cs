using CloudCash.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CloudCash.DAL.Data
{
    public class CloudCashDbContext : DbContext
    {
        public CloudCashDbContext()
        {

        }

        public CloudCashDbContext(DbContextOptions<CloudCashDbContext> options) : base(options)
        {

        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server = tcp:githubissue.database.windows.net, 1433; Initial Catalog = GitHubIssue; Persist Security Info = False; User ID = GitHub; Password =123Git456Hub; MultipleActiveResultSets = False; Encrypt = True; TrustServerCertificate = False; Connection Timeout = 30;");
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("SQL_Latin1_General_CP1_CS_AS");

            modelBuilder.Entity<TableInfo>().HasOne(x => x.Category).WithMany();
            modelBuilder.Entity<TableInfo>().Navigation(b => b.Category).UsePropertyAccessMode(PropertyAccessMode.Property);

            modelBuilder.Entity<Product>().HasOne(x => x.Category).WithMany();
            modelBuilder.Entity<Product>().Navigation(b => b.Category).UsePropertyAccessMode(PropertyAccessMode.Property);

            modelBuilder.Entity<Reservation>().HasIndex(u => u.Email).IsUnique();
            modelBuilder.Entity<Customer>().HasIndex(u => u.Email).IsUnique();
            modelBuilder.Entity<TableCategory>().HasIndex(u => u.Name).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.NickName).IsUnique();
            modelBuilder.Entity<User>().Property(u => u.NickName).UseCollation("SQL_Latin1_General_CP1_CS_AS");
            modelBuilder.Entity<ProductCategory>().HasIndex(u => u.Name).IsUnique();
        }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<ExpenseIncome> ExpenseIncomes { get; set; }

        public DbSet<Payment> Payments { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductCategory> ProductCategories { get; set; }

        public DbSet<Sell> Sells { get; set; }

        public DbSet<Shift> Shifts { get; set; }

        public DbSet<Table> Tables { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<TableCategory> TableCategories { get; set; }

        public DbSet<TableInfo> TableInfos { get; set; }

        public DbSet<Reservation> Reservations { get; set; }

        public DbSet<Card> Cards { get; set; }

        public DbSet<UserLog> UserLogs { get; set; }
    }
}
