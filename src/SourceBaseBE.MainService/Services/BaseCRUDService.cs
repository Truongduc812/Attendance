using System.Collections.Generic;
using SourceBaseBE.Database.Repository;
using iSoft.Common.Models.RequestModels;
using System;
using System.Reflection;
using iSoft.Database.Entities;
using SourceBaseBE.MainService.Models;
using iSoft.Database.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SourceBaseBE.Database.DBContexts;
using PRPO.Database.Helpers;
using SourceBaseBE.Database.Models.ResponseModels;

namespace SourceBaseBE.MainService.Services
{
	public class BaseCRUDService<TEntity> : IDisposable where TEntity : BaseCRUDEntity, new()
	{
		public CommonDBContext _dbContext;
		public ILogger<BaseCRUDService<TEntity>> _logger;
		public BaseCRUDRepository<TEntity> _repository;
		public BaseCRUDService(CommonDBContext dbContext, ILogger<BaseCRUDService<TEntity>> logger)
		{
			_dbContext = dbContext;
			_logger = logger;
		}
		public virtual List<Dictionary<string, object>> GetFormDataObjElement(TEntity entity)
		{
			return new List<Dictionary<string, object>>();
		}
		public virtual object GetDisplayName(string name, string entityName)
		{
			var disName = JsonPropertyHelper<TEntity>.GetDisplayName(name);
			return disName;
		}
		public virtual List<TEntity> GetList(PagingRequestModel pagingReq = null)
		{
			return _repository.GetList(pagingReq);
		}
		public virtual TEntity GetById(long id, bool isDirect = false, bool isTracking = true)
		{
			return _repository.GetById(id, isDirect, isTracking);
		}
		public virtual Task<TEntity> GetByIdAsync(long id, bool isDirect = false, bool isTracking = true)
		{
			return _repository.GetByIdAsync(id, isDirect, isTracking);
		}
		public virtual long GetTotalRecord()
		{
			return _repository.GetTotalRecord();
		}
		public virtual TEntity Upsert(TEntity entity, long? userId = null)
		{
			return _repository.Upsert(entity, userId);
		}
		public virtual int Delete(TEntity entity, long? userId = null, bool isSoftDelete = true)
		{
			return _repository.Delete(entity, userId, isSoftDelete);
		}
		public virtual int Delete(long id, long? userId = null, bool isSoftDelete = true)
		{
			return _repository.Delete(id, userId, isSoftDelete);
		}
		public virtual List<FormSelectOptionModel> GetListOptionData(List<object> list)
		{
			List<FormSelectOptionModel> rs = new List<FormSelectOptionModel>();
			if (list != null)
			{
				foreach (var item in list)
				{
					rs.Add(new FormSelectOptionModel(item.ToString(), item.ToString()));
				}
			}
			return rs;
		}

		private bool _disposedValue;
		protected virtual void Dispose(bool disposing)
		{
			if (!_disposedValue)
			{
				if (disposing)
				{
					//// TODO: dispose managed state (managed objects)
					//this._repository.Dispose();

					var fields = this.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
					foreach (var field in fields)
					{
						var type = (field.GetValue(this) as IDisposable);
						type?.Dispose();
					}
				}

				_disposedValue = true;
			}
		}
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
	}
}