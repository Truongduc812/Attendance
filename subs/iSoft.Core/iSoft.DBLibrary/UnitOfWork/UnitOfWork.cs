using iSoft.Extensions.EntityFrameworkCore.Repository;
using iSoft.DBLibrary.Entities;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace iSoft.Extensions.EntityFrameworkCore.UnitOfWork
{
  public class UnitOfWork<TContext> : IUnitOfWork<TContext>, IDisposable
         where TContext : DbContext, new()
  {
    //Here TContext is nothing but your DBContext class
    //In our example it is EmployeeDBContext class
    private TContext _context;
    private bool _disposed;
    private string _errorMessage = string.Empty;
    private IDbContextTransaction _objTran;
    private Dictionary<string, object> _repositories;
    //Using the Constructor we are initializing the _context variable is nothing but
    //we are storing the DBContext (EmployeeDBContext) object in _context variable
    public UnitOfWork()
    {
      _context ??= new TContext();
    }
    public UnitOfWork(TContext dbContext)
    {
      _context = dbContext;
    }
    //The Dispose() method is used to free unmanaged resources like files, 
    //database connections etc. at any time.
    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }
    //This Context property will return the DBContext object i.e. (EmployeeDBContext) object
    public TContext Context
    {
      get { return _context; }
    }
    //This CreateTransaction() method will create a database Trnasaction so that we can do database operations by
    //applying do evrything and do nothing principle
    public void CreateTransaction()
    {
      _objTran = _context.Database.BeginTransaction();
    }
    public async Task CreateTransactionAsync()
    {
      _objTran = await _context.Database.BeginTransactionAsync();
    }
    //If all the Transactions are completed successfuly then we need to call this Commit() 
    //method to Save the changes permanently in the database
    public void Commit()
    {
      _objTran.Commit();
    }
    public Task CommitAsync()
    {
      return _objTran.CommitAsync();
    }
    //If atleast one of the Transaction is Failed then we need to call this Rollback() 
    //method to Rollback the database changes to its previous state
    public void Rollback()
    {
      _objTran.Rollback();
      _objTran.Dispose();
    }
    public async Task RollbackAsync()
    {
      await _objTran.RollbackAsync();
      await _objTran.DisposeAsync();
    }
    //This Save() Method Implement DbContext Class SaveChanges method so whenever we do a transaction we need to
    //call this Save() method so that it will make the changes in the database
    public void Save()
    {
      try
      {
        _context.SaveChanges();
      }
      catch (DbEntityValidationException dbEx)
      {
        foreach (var validationErrors in dbEx.EntityValidationErrors)
          foreach (var validationError in validationErrors.ValidationErrors)
            _errorMessage += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;
        throw new Exception(_errorMessage, dbEx);
      }
    }
    protected virtual void Dispose(bool disposing)
    {
      if (!_disposed)
        if (disposing)
          _context.Dispose();
      _disposed = true;
    }

    public GenericRepository<TContext, T> GenericRepository<T>() where T : BaseEntity
    {
      if (_repositories == null)
        _repositories = new Dictionary<string, object>();

      var type = typeof(T).Name;
      if (!_repositories.ContainsKey(type))
      {
        //var repositoryType = typeof(GenericRepository<TContext,T>);
        //var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _context);
        //_repositories.Add(type, repositoryInstance);
        var repositoryInstance = new GenericRepository<TContext, T>(_context);

        _repositories.Add(type, repositoryInstance);
      }
      return (GenericRepository<TContext, T>)_repositories[type];
    }

    public async Task<int> DoFunctionAsync(Task<int> func)
    {
      if (func == null)
        throw new Exception("function not implement");
      try
      {
        await CreateTransactionAsync();
        var effectRow = await func;
        await CommitAsync();
        return effectRow;
      }
      catch (Exception ex)
      {
        await RollbackAsync();
        throw ex;
      }
    }
  }
}
