using CoreNG.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace CoreNG.Persistence.Sqlite
{
    public class CoreNgDbContext: DbContext, IDbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Ledger> Ledgers { get; set; }
        public DbSet<ScheduledTransaction> ScheduledTransactions { get; set; }

        public bool AlreadyInTransaction { get; set; }

        public bool TransactionsSupported
        {
            get { return true; }
        }
    
        public IDatabaseContextTransaction GetTransaction()
        {
            if (TransactionsSupported)
            {
                if (!AlreadyInTransaction)
                {
                    AlreadyInTransaction = true;
                    return new DatabaseContextTransaction(this, this.Database.BeginTransaction());
                }
            }
            return new DatabaseContextTransaction(this);
        }

        public CoreNgDbContext()
        {
            
        }
        
        public CoreNgDbContext(DbContextOptions<CoreNgDbContext> options): base(options)
        {
            
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("DataSource=app.db");
            }
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyAllConfigurations();
        }
    }
}