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
  public class AuthGroupRepository : BaseCRUDRepository<AuthGroupEntity>
  {
    public AuthGroupRepository(CommonDBContext dbContext)
      : base(dbContext)
    {
    }
    public override string GetName()
    {
      return nameof(AuthGroupRepository);
    }
    /// <summary>
    /// GetById (@GenCRUD)
    /// </summary>
    /// <param name="Id"></param>
    /// <returns></returns>
    /// <exception cref="DBException"></exception>
    public override AuthGroupEntity? GetById(long id, bool isDirect = false, bool isTracking = true)
    {
      try
      {
        AuthGroupEntity? result = null;
        //string cacheKey = $"{cacheKeyDetail}:{id}";
        //if (!isDirect)
        //{
        //  result = CachedFunc.GetRedisData<AuthGroupEntity>(cacheKey, null);
        //}

        //if (result == null)
        //{
          var dataSet = _context.Set<AuthGroupEntity>();
          IQueryable<AuthGroupEntity> queryable;
          if (!isTracking)
          {
            queryable = dataSet.AsNoTracking().AsQueryable();
          }
          else
          {
            queryable = dataSet.AsQueryable();
          }
          result = queryable
                .Include(entity => entity.ListAuthPermission)
              .Include(entity => entity.ListUser)/*[GEN-7]*/
                .Where(entity => entity.DeletedFlag != true && entity.Id == id)
                .FirstOrDefault();
          //result.AuthGroup2s = result.AuthGroup2s.Select(x => new AuthGroup2Entity() { Id = x.Id, Name = x.Name }).ToList();

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
    public override List<AuthGroupEntity> GetList(PagingRequestModel pagingReq = null)
    {
      try
      {
        string cacheKey = $"{cacheKeyList}";
        if (pagingReq != null)
        {
          cacheKey = $"{cacheKeyList}:{pagingReq.Page}|{pagingReq.PageSize}";
        }
        List<AuthGroupEntity>? result = CachedFunc.GetRedisData<List<AuthGroupEntity>>(cacheKey, null);
        if (result == null)
        {
          result = new List<AuthGroupEntity>();
          if (pagingReq != null)
          {
            result = _context.Set<AuthGroupEntity>()
                .AsNoTracking()
                .AsQueryable()
                .Include(entity => entity.ListAuthPermission)
              .Include(entity => entity.ListUser)/*[GEN-11]*/
                .Where(entity => entity.DeletedFlag != true)
                .OrderBy(entity => entity.Order).ThenBy(entity => entity.Id)
                .Skip(pagingReq.GetSkip()).Take(pagingReq.GetLimit())
                .AsParallel()
                .ToList();

            //.Select(entity => new AuthGroupEntity
            // {
            //   Id = entity.Id,
            //   AuthGroup2s = entity.AuthGroup2s.Select(x => new AuthGroup2Entity { Id = x.Id, Name = x.Name }).ToList()
            // })

            for (var i = 0; i < result.Count; i++)
            {

              result[i].ListAuthPermission = result[i].ListAuthPermission?.Select(x => new AuthPermissionEntity() { Id = x.Id }).ToList();
              result[i].ListUser = result[i].ListUser?.Select(x => new UserEntity() { Id = x.Id }).ToList();
              /*[GEN-12]*/
            }
          }
          else
          {
            result = _context.Set<AuthGroupEntity>()
                .AsNoTracking()
                .AsQueryable()
                .Include(entity => entity.ListAuthPermission)
              .Include(entity => entity.ListUser)/*[GEN-13]*/
                .Where(entity => entity.DeletedFlag != true)
                .OrderBy(entity => entity.Order).ThenBy(entity => entity.Id)
                .Skip(0).Take(ConstCommon.ConstSelectListMaxRecord)
                .AsParallel()
                .ToList();

            for (var i = 0; i < result.Count; i++)
            {

              result[i].ListAuthPermission = result[i].ListAuthPermission?.Select(x => new AuthPermissionEntity() { Id = x.Id }).ToList();
              result[i].ListUser = result[i].ListUser?.Select(x => new UserEntity() { Id = x.Id }).ToList();
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
    public AuthGroupEntity Upsert(AuthGroupEntity entity, List<AuthPermissionEntity> authPermissionChildren, List<UserEntity> authUserChildren/*[GEN-8]*/, long? userId = null)
    {
      try
      {
        if (entity.Id <= 0)
        {
          // Insert
          entity = Insert(entity, authPermissionChildren, authUserChildren/*[GEN-4]*/, userId);
        }
        else
        {
          // Update
          entity = Update(entity, authPermissionChildren, authUserChildren/*[GEN-4]*/, userId);
        }
        return entity;
      }
      catch (Exception ex)
      {
        throw new DBException(ex);
      }
    }
    public AuthGroupEntity Insert(AuthGroupEntity entity, List<AuthPermissionEntity> authPermissionChildren, List<UserEntity> authUserChildren/*[GEN-8]*/, long? userId = null)
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
          entity.ListAuthPermission = MergeChildrenEntity(entity.ListAuthPermission, authPermissionChildren);

          entity.ListUser = MergeChildrenEntity(entity.ListUser, authUserChildren);
          /*[GEN-10]*/
          _context.Set<AuthGroupEntity>().Add(entity);
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
    public AuthGroupEntity Update(AuthGroupEntity entity, List<AuthPermissionEntity> authPermissionChildren, List<UserEntity> authUserChildren/*[GEN-8]*/, long? userId = null)
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
          entity.ListAuthPermission = MergeChildrenEntity(entity.ListAuthPermission, authPermissionChildren);

          entity.ListUser = MergeChildrenEntity(entity.ListUser, authUserChildren);
          /*[GEN-9]*/
          _context.Set<AuthGroupEntity>().Update(entity);
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
    public override string GetDisplayField(AuthGroupEntity entity)
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
