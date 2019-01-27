using System;
using Microsoft.EntityFrameworkCore.Storage;

namespace CoreNG.Persistence
{
    public interface IDatabaseContextTransaction: IDisposable
    {
        IDbContextTransaction Transaction { get; }
        void Commit();
        void Rollback();
    }
}