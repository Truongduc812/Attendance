using iSoft.DBLibrary.SQLBuilder.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace iSoft.Extensions.EntityFrameworkCore.Repository
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        // public ISQLBuilde sqlBuilder { get; set; }
        abstract IEnumerable<TEntity> GetAll();
        abstract Task<IEnumerable<TEntity>> GetAllAsync();
        abstract TEntity GetById(object id);
        abstract IEnumerable<TEntity> GetByProperty(string table, string column, object value);
        abstract Task<IEnumerable<TEntity>> GetByPropertyAsync(string table, string column, object value);
        abstract Task<IQueryable<TEntity>> GetByMultiPropertiesAsync(string table, Dictionary<string, object> columns);
        abstract Task<TEntity> GetByIdAsync(object id);
        abstract IQueryable<TEntity> Table { get; }
        abstract void Insert(TEntity entity, long? userId = null);
        abstract Task<int> InsertAsync(TEntity entity, long? userId = null);
        abstract void BulkInsert(IEnumerable<TEntity> entities, long? userId = null);
        abstract Task<int> BulkInsertAsync(IEnumerable<TEntity> entities, long? userId = null);
        abstract void Update(TEntity entity, long? userId = null);
        abstract Task<int> UpdateAsync(TEntity entity, long? userId = null);
        abstract void Update(IEnumerable<TEntity> entities, long? userId = null);
        abstract Task<int> UpdateRangeAsync(IEnumerable<TEntity> entities, long? userId = null);
        abstract void Delete(TEntity entity);
        abstract Task<int> DeleteAsync(TEntity entity);
        abstract bool IsExisted(TEntity entity);
    }
}
