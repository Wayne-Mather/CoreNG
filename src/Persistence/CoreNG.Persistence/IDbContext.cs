
using System;
using System.Threading;
using System.Threading.Tasks;
using CoreNG.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace CoreNG.Persistence
{
    public interface IDbContext : IDisposable
    {
        DbSet<Account> Accounts { get; set; }
        DbSet<Budget> Budgets { get; set; }
        DbSet<Category> Categories { get; set; }
        DbSet<Ledger> Ledgers { get; set; }
        DbSet<ScheduledTransaction> ScheduledTransactions { get; set; }
        
        bool AlreadyInTransaction { get; set; }
        bool TransactionsSupported { get; }
        IDatabaseContextTransaction GetTransaction();
        
        DatabaseFacade Database { get; }
        int SaveChanges();
        int SaveChanges(bool acceptAllChangesOnSuccess);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));

        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default(CancellationToken));
    }
}