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
	public class MessageRepository : BaseCRUDRepository<MessageEntity>
	{
		public MessageRepository(CommonDBContext dbContext)
			: base(dbContext)
		{
		}
		public override string GetName()
		{
			return nameof(MessageRepository);
		}
		/// <summary>
		/// GetById (@GenCRUD)
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		/// <exception cref="DBException"></exception>
		public override MessageEntity? GetById(long id, bool isDirect = false, bool isTracking = true)
		{
			try
			{
				MessageEntity? result = null;
				//string cacheKey = $"{cacheKeyDetail}:{id}";
				//if (!isDirect)
				//{
				//	result = CachedFunc.GetRedisData<MessageEntity>(cacheKey, null);
				//}

				//if (result == null)
    //    {
          var dataSet = _context.Set<MessageEntity>();
          IQueryable<MessageEntity> queryable;
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
					//result.Message2s = result.Message2s.Select(x => new Message2Entity() { Id = x.Id, Name = x.Name }).ToList();

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
		/// GetById (@GenCRUD)
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		/// <exception cref="DBException"></exception>
		public override async Task<MessageEntity>? GetByIdAsync(long id, bool isDirect = false, bool isTracking = true)
		{
			try
			{
				MessageEntity? result = null;
				//string cacheKey = $"{cacheKeyDetail}:{id}";
				//if (!isDirect)
				//{
				//	result = CachedFunc.GetRedisData<MessageEntity>(cacheKey, null);
				//}

				//if (result == null)
				//{
					result = await (isTracking ?
					_context.Set<MessageEntity>()
							  //.AsNoTracking()
							  .AsQueryable()
							  /*[GEN-7]*/
							  .Where(entity => entity.DeletedFlag != true && entity.Id == id)
							  .FirstOrDefaultAsync() :
							  _context.Set<MessageEntity>()
							  .AsNoTracking()
							  .AsQueryable()
							  /*[GEN-7]*/
							  .Where(entity => entity.DeletedFlag != true && entity.Id == id)
							  .FirstOrDefaultAsync());
					//result.Message2s = result.Message2s.Select(x => new Message2Entity() { Id = x.Id, Name = x.Name }).ToList();

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
		public override List<MessageEntity> GetList(PagingRequestModel pagingReq = null)
		{
			try
			{
				string cacheKey = $"{cacheKeyList}";
				if (pagingReq != null)
				{
					cacheKey = $"{cacheKeyList}:{pagingReq.Page}|{pagingReq.PageSize}";
				}
				List<MessageEntity>? result = CachedFunc.GetRedisData<List<MessageEntity>>(cacheKey, null);
				if (result == null)
				{
					result = new List<MessageEntity>();
					if (pagingReq != null)
					{
						result = _context.Set<MessageEntity>()
								.AsNoTracking()
								.AsQueryable()
								/*[GEN-11]*/
								.Where(entity => entity.DeletedFlag != true)
								.OrderBy(entity => entity.Order).ThenBy(entity => entity.Id)
								.Skip(pagingReq.GetSkip()).Take(pagingReq.GetLimit())
								.AsParallel()
								.ToList();

						//.Select(entity => new MessageEntity
						// {
						//   Id = entity.Id,
						//   Message2s = entity.Message2s.Select(x => new Message2Entity { Id = x.Id, Name = x.Name }).ToList()
						// })

						for (var i = 0; i < result.Count; i++)
						{

							
/*[GEN-12]*/
						}
					}
					else
					{
						result = _context.Set<MessageEntity>()
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

    public List<MessageEntity> GetListByUser(long userId, PagingRequestModel pagingReq = null)
    {
      try
      {
        string cacheKey = $"{cacheKeyList}_GetListByUser:{userId}";
        if (pagingReq != null)
        {
          cacheKey += $":{pagingReq.GetKeyCache()}";
        }
        List<MessageEntity>? result = CachedFunc.GetRedisData<List<MessageEntity>>(cacheKey, null);
        if (result == null)
        {
          result = new List<MessageEntity>();
          if (pagingReq != null)
          {
            result = _context.Set<MessageEntity>()
                .AsNoTracking()
                .AsQueryable()
                /*[GEN-11]*/
                .Where(entity => entity.DeletedFlag != true)
                .Where(entity => entity.UserId == userId)
                .OrderByDescending(entity => entity.SendDate).ThenByDescending(entity => entity.Id)
                .Skip(pagingReq.GetSkip()).Take(pagingReq.GetLimit())
                .AsParallel()
                .ToList();
          }
          else
          {
            result = _context.Set<MessageEntity>()
                .AsNoTracking()
                .AsQueryable()
                /*[GEN-13]*/
                .Where(entity => entity.DeletedFlag != true)
                .Where(entity => entity.UserId == userId)
                .OrderByDescending(entity => entity.SendDate).ThenByDescending(entity => entity.Id)
                .Skip(0).Take(ConstCommon.ConstSelectListMaxRecord)
                .AsParallel()
                .ToList();
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

    public long GetTotalRecordByUserId(long userId)
    {
      try
      {
        string cacheKey = $"{cacheKeyTotalRecord}_GetTotalRecordByUserId:{userId}";
        long? result = CachedFunc.GetRedisData<long>(cacheKey, -1);
        if (result == -1)
        {
          result = _context.Set<MessageEntity>()
                  .AsQueryable()
                  .Where(entity => entity.DeletedFlag != true && entity.UserId == userId)
                  .LongCount();

          CachedFunc.AddEntityCacheKey(GetName(), cacheKey, true);
          CachedFunc.SetRedisData(cacheKey, result, ConstCommon.ConstGetListCacheExpiredTimeInSeconds);
        }
        return result.Value;
      }
      catch (Exception ex)
      {
        throw new DBException(ex);
      }
    }
    public MessageEntity Upsert(MessageEntity entity/*[GEN-8]*/, long? userId = null)
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
		public MessageEntity Insert(MessageEntity entity/*[GEN-8]*/, long? userId = null)
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
					_context.Set<MessageEntity>().Add(entity);
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
		public MessageEntity Update(MessageEntity entity/*[GEN-8]*/, long? userId = null)
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
					_context.Set<MessageEntity>().Update(entity);
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
		public override string GetDisplayField(MessageEntity entity)
		{
			return entity.Title.ToString();
		}
		public override List<FormSelectOptionModel> GetSelectData(string entityName, string category)
		{
			List<FormSelectOptionModel> rs = GetListWithNoInclude().Select(x => new FormSelectOptionModel(x.Id, GetDisplayField(x))).ToList();
			return rs;
		}
	}
}
