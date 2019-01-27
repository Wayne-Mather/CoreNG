using System.Transactions;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace CoreNG.Persistence
{  
    
    public class DatabaseContextTransaction : IDatabaseContextTransaction
    {
        private readonly IDbContext _context;
        private IDbContextTransaction _transaction;

        public Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction Transaction { get; }
        
        public DatabaseContextTransaction(IDbContext context)
        {
            _context = context;
            _transaction = null;
        }

        public DatabaseContextTransaction(IDbContext context, IDbContextTransaction transaction)
        {
            _transaction = transaction;
            _context = context;
        }
        
        public void Dispose()
        {
            if (_transaction != null)
            {
                if (_context.AlreadyInTransaction)
                {
                    try
                    {
                        Rollback();
                    }
                    catch (TransactionException ex)
                    {
                        // NOTE: we will silently fail as it is expected the framework
                        // will not commit anything to storage...
                    } 
                }
                _context.AlreadyInTransaction = false;
                _transaction.Dispose();
                _transaction = null;
            }
        }


        

        public void Commit()
        {
            if (_transaction != null)
            {
                try
                {
                    _transaction.Commit();
                }
                
                catch (TransactionAbortedException ex)
                {
                    throw;
                }
                catch (TransactionException ex)
                {
                    throw new TransactionAbortedException(ex.Message, ex.InnerException);
                }
                

                _context.AlreadyInTransaction = false;
            }  
        }

        public void Rollback()
        {
            if (_transaction != null)
            {
                _context.AlreadyInTransaction = false;
                _transaction.Rollback();
            }
        }
    }
}