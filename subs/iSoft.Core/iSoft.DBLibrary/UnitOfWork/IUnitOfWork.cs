
using iSoft.Extensions.EntityFrameworkCore.Repository;
using iSoft.DBLibrary.Entities;
using System;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace iSoft.Extensions.EntityFrameworkCore.UnitOfWork
{
  public interface IUnitOfWork<TContext> : IDisposable
        where TContext : DbContext, new()
  {
    TContext Context { get; }
    void CreateTransaction();
    public Task CreateTransactionAsync();
    void Commit();
    public Task CommitAsync();
    void Rollback();
    Task RollbackAsync();
    void Save();
    public Task<int> DoFunctionAsync(Task<int> func);
    GenericRepository<TContext, T> GenericRepository<T>() where T : BaseEntity;
  }
}
