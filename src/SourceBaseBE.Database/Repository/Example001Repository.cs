using iSoft.Common;
using iSoft.Common.Exceptions;
using iSoft.Common.Models.RequestModels;
using MathNet.Numerics.Statistics.Mcmc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using iSoft.Redis.Services;
using SourceBaseBE.Database.DBContexts;
using SourceBaseBE.Database.Entities;
using iSoft.Database.Models;
using iSoft.Common.Models.ConfigModel.Subs;

namespace SourceBaseBE.Database.Repository
{
	public class Example001Repository : BaseCRUDRepository<Example001Entity>
	{
		public Example001Repository(CommonDBContext dbContext)
			: base(dbContext)
		{
		}
		public override string GetName()
		{
			return nameof(Example001Repository);
		}
		/// <summary>
		/// GetById (@GenCRUD)
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		/// <exception cref="DBException"></exception>
		public override Example001Entity? GetById(long id, bool isDirect = false, bool isTracking = true)
		{
			try
			{
				Example001Entity? result = null;
				//string cacheKey = $"{cacheKeyDetail}:{id}";
				//if (!isDirect)
				//{
				//	result = CachedFunc.GetRedisData<Example001Entity>(cacheKey, null);
				//}

				//if (result == null)
    //    {
          var dataSet = _context.Set<Example001Entity>();
          IQueryable<Example001Entity> queryable;
          if (!isTracking)
          {
            queryable = dataSet.AsNoTracking().AsQueryable();
          }
          else
          {
            queryable = dataSet.AsQueryable();
          }
          result = queryable
                .Include(entity => entity.ListGenTemplate)/*[GEN-7]*/
							  .Where(entity => entity.DeletedFlag != true && entity.Id == id)
							  .FirstOrDefault();
					//result.Example0012s = result.Example0012s.Select(x => new Example0012Entity() { Id = x.Id, Name = x.Name }).ToList();

				//	CachedFunc.AddEntityCacheKey(GetName(), cacheKey, true);
				//	CachedFunc.SetRedisData(cacheKey, result, ConstCommon.ConstGetDetailCacheExpiredTimeInSeconds);
				//}
				return result;
			}
			catch (Exception ex)
			{
				throw new DBException(ex);
			}
		}
		/// <summary>
		/// GetList (@GenCRUD)
		/// </summary>
		/// <param name="pagingReq"></param>
		/// <returns></returns>
		/// <exception cref="DBException"></exception>
		public override List<Example001Entity> GetList(PagingRequestModel pagingReq = null)
		{
			try
			{
				string cacheKey = $"{cacheKeyList}";
				if (pagingReq != null)
				{
					cacheKey = $"{cacheKeyList}:{pagingReq.Page}|{pagingReq.PageSize}";
				}
				List<Example001Entity>? result = CachedFunc.GetRedisData<List<Example001Entity>>(cacheKey, null);
				if (result == null)
				{
					result = new List<Example001Entity>();
					if (pagingReq != null)
					{
						result = _context.Set<Example001Entity>()
								.AsNoTracking()
								.AsQueryable()
								.Include(entity => entity.ListGenTemplate)/*[GEN-11]*/
								.Where(entity => entity.DeletedFlag != true)
								.OrderBy(entity => entity.Order).ThenBy(entity => entity.Id)
								.Skip(pagingReq.GetSkip()).Take(pagingReq.GetLimit())
								.AsParallel()
								.ToList();

						//.Select(entity => new Example001Entity
						// {
						//   Id = entity.Id,
						//   Example0012s = entity.Example0012s.Select(x => new Example0012Entity { Id = x.Id, Name = x.Name }).ToList()
						// })

						for (var i = 0; i < result.Count; i++)
						{

							result[i].ListGenTemplate = result[i].ListGenTemplate?.Select(x => new GenTemplateEntity() { Id = x.Id }).ToList();
							/*[GEN-12]*/
						}
					}
					else
					{
						result = _context.Set<Example001Entity>()
								.AsNoTracking()
								.AsQueryable()
								.Include(entity => entity.ListGenTemplate)/*[GEN-13]*/
								.Where(entity => entity.DeletedFlag != true)
								.OrderBy(entity => entity.Order).ThenBy(entity => entity.Id)
								.Skip(0).Take(ConstCommon.ConstSelectListMaxRecord)
								.AsParallel()
								.ToList();

						for (var i = 0; i < result.Count; i++)
						{

							result[i].ListGenTemplate = result[i].ListGenTemplate?.Select(x => new GenTemplateEntity() { Id = x.Id }).ToList();
							/*[GEN-14]*/
						}
					}

					CachedFunc.AddEntityCacheKey(GetName(), cacheKey, true);
					CachedFunc.SetRedisData(cacheKey, result, ConstCommon.ConstGetListCacheExpiredTimeInSeconds);
				}
				return result;
			}
			catch (Exception ex)
			{
				throw new DBException(ex);
			}
		}
		public Example001Entity Upsert(Example001Entity entity, List<GenTemplateEntity> genTemplateChildren/*[GEN-8]*/, long? userId = null)
		{
			try
			{
				if (entity.Id <= 0)
				{
					// Insert
					entity = Insert(entity, genTemplateChildren/*[GEN-4]*/, userId);
				}
				else
				{
					// Update
					entity = Update(entity, genTemplateChildren/*[GEN-4]*/, userId);
				}
				return entity;
			}
			catch (Exception ex)
			{
				throw new DBException(ex);
			}
		}
		public Example001Entity Insert(Example001Entity entity, List<GenTemplateEntity> genTemplateChildren/*[GEN-8]*/, long? userId = null)
		{
			try
			{
				if (entity.Id > 0)
				{
					throw new DBException($"Insert, Unexpected Id in entity, Id={entity.Id}");
				}
				else
				{
					if (userId != null)
					{
						entity.CreatedBy = userId;
					}
					entity.CreatedAt = DateTime.Now;
					entity.UpdatedBy = entity.CreatedBy;
					entity.UpdatedAt = entity.CreatedAt;
					entity.DeletedFlag = false;
					entity.ListGenTemplate = MergeChildrenEntity(entity.ListGenTemplate, genTemplateChildren);
					/*[GEN-10]*/
					_context.Set<Example001Entity>().Add(entity);
				}
				var result = _context.SaveChanges();
				ClearCachedData();
				return entity;
			}
			catch (Exception ex)
			{
				throw new DBException(ex);
			}
		}
		public Example001Entity Update(Example001Entity entity, List<GenTemplateEntity> genTemplateChildren/*[GEN-8]*/, long? userId = null)
		{
			try
			{
				if (entity.Id <= 0)
				{
					throw new DBException("Update, Id not found in entity");
				}
				else
				{
					if (userId != null)
					{
						entity.UpdatedBy = userId;
					}
					entity.UpdatedAt = DateTime.Now;
					entity.ListGenTemplate = MergeChildrenEntity(entity.ListGenTemplate, genTemplateChildren);
					/*[GEN-9]*/
					_context.Set<Example001Entity>().Update(entity);
				}
				var result = _context.SaveChanges();
				ClearCachedData();
				return entity;
			}
			catch (Exception ex)
			{
				throw new DBException(ex);
			}
		}
		public override string GetDisplayField(Example001Entity entity)
		{
			return entity.Name.ToString();
		}
		public override List<FormSelectOptionModel> GetSelectData(string entityName, string category)
		{
			List<FormSelectOptionModel> rs = GetListWithNoInclude().Select(x => new FormSelectOptionModel(x.Id, GetDisplayField(x))).ToList();
			return rs;
		}
	}
}
