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
using UserEntity = SourceBaseBE.Database.Entities.UserEntity;
using ISoftProjectEntity = SourceBaseBE.Database.Entities.ISoftProjectEntity;
using iSoft.Common.Models.ConfigModel.Subs;

namespace SourceBaseBE.Database.Repository
{
	public class JobTitleRepository : BaseCRUDRepository<JobTitleEntity>
	{
		public override string TableName
		{
			get { return "JobTitles"; }
		}
		public JobTitleRepository(CommonDBContext dbContext)
		  : base(dbContext)
		{
		}
		public override string GetName()
		{
			return nameof(JobTitleRepository);
		}
		/// <summary>
		/// GetById (@GenCRUD)
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		/// <exception cref="DBException"></exception>
		public override JobTitleEntity? GetById(long id, bool isDirect = false, bool isTracking = true)
		{
			try
			{
				JobTitleEntity? result = null;
				//string cacheKey = $"{cacheKeyDetail}:{id}";
				//if (!isDirect)
				//{
				//  result = CachedFunc.GetRedisData<JobTitleEntity>(cacheKey, null);
				//}

				//if (result == null)
				//{
				var dataSet = _context.Set<JobTitleEntity>();
				IQueryable<JobTitleEntity> queryable;
				if (!isTracking)
				{
					queryable = dataSet.AsNoTracking().AsQueryable();
				}
				else
				{
					queryable = dataSet.AsQueryable();
				}
				result = queryable
					  /*[GEN-7]*/
					  .Where(entity => entity.DeletedFlag != true && entity.Id == id)
					  .FirstOrDefault();
				//result.JobTitle2s = result.JobTitle2s.Select(x => new JobTitle2Entity() { Id = x.Id, Name = x.Name }).ToList();

				//  CachedFunc.AddEntityCacheKey(GetName(), cacheKey, true);
				//  CachedFunc.SetRedisData(cacheKey, result, ConstCommon.ConstGetDetailCacheExpiredTimeInSeconds);
				//}
				return result;
			}
			catch (Exception ex)
			{
				throw new DBException(ex);
			}
		}
		/// <summary>
		/// GetById (@GenCRUD)
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		/// <exception cref="DBException"></exception>
		public override async Task<JobTitleEntity>? GetByIdAsync(long id, bool isDirect = false, bool isTracking = true)
		{
			try
			{
				JobTitleEntity? result = null;
				//string cacheKey = $"{cacheKeyDetail}:{id}";
				//if (!isDirect)
				//{
				//  result = CachedFunc.GetRedisData<JobTitleEntity>(cacheKey, null);
				//}

				//if (result == null)
				//{
				result = await (isTracking ?
				_context.Set<JobTitleEntity>()
					  //.AsNoTracking()
					  .AsQueryable()
					  /*[GEN-7]*/
					  .Where(entity => entity.DeletedFlag != true && entity.Id == id)
					  .FirstOrDefaultAsync() :
					  _context.Set<JobTitleEntity>()
					  .AsNoTracking()
					  .AsQueryable()
					  /*[GEN-7]*/
					  .Where(entity => entity.DeletedFlag != true && entity.Id == id)
					  .FirstOrDefaultAsync());
				//result.JobTitle2s = result.JobTitle2s.Select(x => new JobTitle2Entity() { Id = x.Id, Name = x.Name }).ToList();

				//  CachedFunc.AddEntityCacheKey(GetName(), cacheKey, true);
				//  CachedFunc.SetRedisData(cacheKey, result, ConstCommon.ConstGetDetailCacheExpiredTimeInSeconds);
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
		public override List<JobTitleEntity> GetList(PagingRequestModel pagingReq = null)
		{
			try
			{
				string cacheKey = $"{cacheKeyList}_GetList";
				if (pagingReq != null)
				{
					cacheKey = $"{cacheKeyList}_GetList:{pagingReq.Page}|{pagingReq.PageSize}";
				}
				List<JobTitleEntity>? result = CachedFunc.GetRedisData<List<JobTitleEntity>>(cacheKey, null);
				if (result == null)
				{
					result = new List<JobTitleEntity>();
					if (pagingReq != null)
					{
						result = _context.Set<JobTitleEntity>()
							.AsNoTracking()
							.AsQueryable()
							/*[GEN-11]*/
							.Where(entity => entity.DeletedFlag != true)
							.OrderBy(entity => entity.Order).ThenBy(entity => entity.Id)
							.Skip(pagingReq.GetSkip()).Take(pagingReq.GetLimit())
							.AsParallel()
							.ToList();

						//.Select(entity => new JobTitleEntity
						// {
						//   Id = entity.Id,
						//   JobTitle2s = entity.JobTitle2s.Select(x => new JobTitle2Entity { Id = x.Id, Name = x.Name }).ToList()
						// })

						for (var i = 0; i < result.Count; i++)
						{


							/*[GEN-12]*/
						}
					}
					else
					{
						result = _context.Set<JobTitleEntity>()
							.AsNoTracking()
							.AsQueryable()
							/*[GEN-13]*/
							.Where(entity => entity.DeletedFlag != true)
							.OrderBy(entity => entity.Order).ThenBy(entity => entity.Id)
							.Skip(0).Take(ConstCommon.ConstSelectListMaxRecord)
							.AsParallel()
							.ToList();

						for (var i = 0; i < result.Count; i++)
						{


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
		public JobTitleEntity Upsert(JobTitleEntity entity/*[GEN-8]*/, long? userId = null)
		{
			try
			{
				if (entity.Id <= 0)
				{
					// Insert
					entity = Insert(entity/*[GEN-4]*/, userId);
				}
				else
				{
					// Update
					entity = Update(entity/*[GEN-4]*/, userId);
				}
				return entity;
			}
			catch (Exception ex)
			{
				throw new DBException(ex);
			}
		}
		public JobTitleEntity Insert(JobTitleEntity entity/*[GEN-8]*/, long? userId = null)
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

					/*[GEN-10]*/
					_context.Set<JobTitleEntity>().Add(entity);
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
		public JobTitleEntity Update(JobTitleEntity entity/*[GEN-8]*/, long? userId = null)
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

					/*[GEN-9]*/
					_context.Set<JobTitleEntity>().Update(entity);
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

		public JobTitleEntity? GetByName(string name, bool isDirect = false)
		{
			try
			{
				JobTitleEntity? result = null;
				string cacheKey = $"{cacheKeyDetail}:GetByName_{name}";
				if (!isDirect)
				{
					result = CachedFunc.GetRedisData<JobTitleEntity>(cacheKey, null);
				}

				if (result == null)
				{
					result = _context.Set<JobTitleEntity>()
						.AsQueryable()
						.FirstOrDefault(entity => entity.DeletedFlag != true && entity.Name.ToLower() == name.ToLower());

					CachedFunc.AddEntityCacheKey(GetName(), cacheKey, true);
					CachedFunc.SetRedisData(cacheKey, result, ConstCommon.ConstGetDetailCacheExpiredTimeInSeconds);
				}
				return result;
			}
			catch (Exception ex)
			{
				throw new DBException(ex);
			}
		}

		public override string GetDisplayField(JobTitleEntity entity)
		{
			return entity.Name.ToString();
		}
		public override List<FormSelectOptionModel> GetSelectData(string entityName, string category)
		{
			List<FormSelectOptionModel> rs = GetListWithNoInclude().Select(x => new FormSelectOptionModel(x.Id, GetDisplayField(x))).ToList();
			return rs;
		}

		public override IEnumerable<JobTitleEntity> InsertMulti(IEnumerable<JobTitleEntity> entities, long? userId = null)
		{
			try
			{
				foreach (var entity in entities)
				{
					if (entity.Id > 0)
					{
						throw new DBException("Insert, Unexpected Id in entity");
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
						_context.Set<JobTitleEntity>().AddRange(entity);
					}
				}
				var result = _context.SaveChanges();
				ClearCachedData();
				return entities;
			}
			catch (Exception ex)
			{
				throw new DBException(ex);
			}
		}
		public async Task<List<JobTitleEntity>> InsertIfNotExist(List<JobTitleEntity> entities, long? userId = null)
		{

			var rs = new List<JobTitleEntity>();
			try
			{
				if (entities != null && entities.Count() > 0)
				{
					foreach (var entity in entities)
					{
						var existed = _context.Set<JobTitleEntity>().FirstOrDefault(x => x.Name.Trim() == entity.Name.Trim() && x.DeletedFlag != true);
						if (existed != null)
						{
							rs.Add(existed);
							continue;
						}
						entity.CreatedBy = userId;
						entity.UpdatedBy = userId;
						entity.CreatedAt = DateTime.Now;
						entity.UpdatedAt = DateTime.Now;
						rs.Add((await _context.Set<JobTitleEntity>().AddAsync(entity)).Entity);
					}
					await _context.SaveChangesAsync();
				}
				return rs;
			}
			catch (Exception ex)
			{
				throw new DBException(ex);
			}
		}
	}
}
