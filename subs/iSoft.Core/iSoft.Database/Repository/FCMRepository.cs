using iSoft.Database.DBContexts;
using iSoft.Database.Entities;
using Microsoft.Extensions.Logging;
using System.Data;
using iSoft.Extensions.EntityFrameworkCore.Repository;
using iSoft.DBLibrary.SQLBuilder.Interfaces;
using iSoft.Extensions.EntityFrameworkCore.UnitOfWork;
using iSoft.Common.Exceptions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data.Entity.Validation;
using iSoft.DBLibrary.Entities;
using iSoft.DBLibrary.SQLBuilder.Enum;
using iSoft.DBLibrary.SQLBuilder;
using iSoft.DBLibrary.DBConnections.Interfaces;
using iSoft.Common;
using iSoft.Common.Models.RequestModels;
using iSoft.Database.Models;
using System.Data.Entity;
using iSoft.Common.Models.ConfigModel.Subs;
using iSoft.Redis.Services;

namespace iSoft.Database.Repository
{
    public class FCMRepository : BaseCRUDRepository<FCMEntity>
  {
    public FCMRepository(CommonDBContext dbContext, ServerConfigModel redisConfig, ILoggerFactory loggerFactory)
        : base(dbContext, redisConfig, loggerFactory)
    {
    }
    public override string GetName()
    {
      return nameof(FCMRepository);
    }
    /// <summary>
    /// GetById (@GenCRUD)
    /// </summary>
    /// <param name="Id"></param>
    /// <returns></returns>
    /// <exception cref="DBException"></exception>
    public override FCMEntity? GetById(long id, bool isDirect = false, bool isTracking = true)
    {
      try
      {
        FCMEntity? result = null;
        //string cacheKey = $"{cacheKeyDetail}:{id}";
        //if (!isDirect)
        //{
        //  result = CachedFunc.GetRedisData<FCMEntity>(cacheKey, null);
        //}

        //if (result == null)
        //{
          var dataSet = _context.Set<FCMEntity>();
          IQueryable<FCMEntity> queryable;
          if (!isTracking)
          {
            queryable = dataSet.AsNoTracking().AsQueryable();
          }
          else
          {
            queryable = dataSet.AsQueryable();
          }
          result = queryable
                    .Where(entity => entity.DeletedFlag != true && entity.Id == id)
                    .FirstOrDefault();

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
    public FCMEntity? GetByUserId(long userId, bool isDirect = false)
    {
      try
      {
        FCMEntity? result = null;
        string cacheKey = $"{cacheKeyDetail}_userId:{userId}";
        if (!isDirect)
        {
          result = CachedFunc.GetRedisData<FCMEntity>(cacheKey, null);
        }

        if (result == null)
        {
          result = _context.Set<FCMEntity>()
                    //.AsNoTracking()
                    .AsQueryable()
                    .Where(entity => entity.DeletedFlag != true && entity.UserId == userId)
                    .FirstOrDefault();

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
    public virtual List<FCMEntity> GetByListUserIds(List<long> userIds, bool isDirect = false)
    {
      try
      {
        List<FCMEntity>? result = null;
        string cacheKey = $"{cacheKeyListByListIds}_userIds:{string.Join(",", userIds)}";
        if (!isDirect)
        {
          result = CachedFunc.GetRedisData<List<FCMEntity>>(cacheKey, null);
        }

        if (result == null)
        {
          result = new List<FCMEntity>();

          result = _context.Set<FCMEntity>()
                  .AsQueryable()
                  .Where(entity => entity.DeletedFlag != true && userIds.Contains(entity.UserId))
                  .OrderBy(entity => entity.Order).ThenBy(entity => entity.Id)
                  .AsParallel()
                  .ToList();

          CachedFunc.AddEntityCacheKey(GetName(), cacheKey, true);
          CachedFunc.SetRedisData(cacheKey, result, ConstCommon.ConstGetListCacheExpiredTimeInSeconds);
        }
        return result;
      }
      catch (Exception dbEx)
      {
        throw new DBException(dbEx);
      }
    }
    /// <summary>
    /// GetList (@GenCRUD)
    /// </summary>
    /// <param name="pagingReq"></param>
    /// <returns></returns>
    /// <exception cref="DBException"></exception>
    public override List<FCMEntity> GetList(PagingRequestModel pagingReq = null)
    {
      try
      {
        string cacheKey = $"{cacheKeyList}";
        if (pagingReq != null)
        {
          cacheKey = $"{cacheKeyList}:{pagingReq.Page}|{pagingReq.PageSize}";
        }
        List<FCMEntity>? result = CachedFunc.GetRedisData<List<FCMEntity>>(cacheKey, null);
        if (result == null)
        {
          result = new List<FCMEntity>();
          if (pagingReq != null)
          {
            result = _context.Set<FCMEntity>()
                    .AsNoTracking()
                    .AsQueryable()
                    .Where(entity => entity.DeletedFlag != true)
                    .OrderBy(entity => entity.Order).ThenBy(entity => entity.Id)
                    .Skip(pagingReq.GetSkip()).Take(pagingReq.GetLimit())
                    .AsParallel()
                    .ToList();

            //.Select(entity => new FCMEntity
            // {
            //   Id = entity.Id,
            //   ISoftProject2s = entity.ISoftProject2s.Select(x => new ISoftProject2Entity { Id = x.Id, Name = x.Name }).ToList()
            // })

            for (var i = 0; i < result.Count; i++)
            {

              /*[GEN-12]*/
            }
          }
          else
          {
            result = _context.Set<FCMEntity>()
                    .AsNoTracking()
                    .AsQueryable()
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
    public FCMEntity Upsert(FCMEntity entity, List<FCMEntity> authUserChildren/*[GEN-8]*/, long? userId = null)
    {
      try
      {
        if (entity.Id <= 0)
        {
          // Insert
          entity = Insert(entity, authUserChildren/*[GEN-4]*/, userId);
        }
        else
        {
          // Update
          entity = Update(entity, authUserChildren/*[GEN-4]*/, userId);
        }
        return entity;
      }
      catch (Exception ex)
      {
        throw new DBException(ex);
      }
    }
    public FCMEntity Insert(FCMEntity entity, List<FCMEntity> authUserChildren/*[GEN-8]*/, long? userId = null)
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
          _context.Set<FCMEntity>().Add(entity);
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
    public FCMEntity Update(FCMEntity entity, List<FCMEntity> authUserChildren/*[GEN-8]*/, long? userId = null)
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
          _context.Set<FCMEntity>().Update(entity);
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
    public override string GetDisplayField(FCMEntity entity)
    {
      return entity.Token.ToString();
    }
    public override List<FormSelectOptionModel> GetSelectData(string entityName, string category)
    {
      List<FormSelectOptionModel> rs = GetListWithNoInclude().Select(x => new FormSelectOptionModel(x.Id, GetDisplayField(x))).ToList();
      return rs;
    }
  }
}
