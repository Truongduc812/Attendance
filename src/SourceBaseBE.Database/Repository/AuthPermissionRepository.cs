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
  public class AuthPermissionRepository : BaseCRUDRepository<AuthPermissionEntity>
  {
    public AuthPermissionRepository(CommonDBContext dbContext)
      : base(dbContext)
    {
    }
    public override string GetName()
    {
      return nameof(AuthPermissionRepository);
    }
    /// <summary>
    /// GetById (@GenCRUD)
    /// </summary>
    /// <param name="Id"></param>
    /// <returns></returns>
    /// <exception cref="DBException"></exception>
    public override AuthPermissionEntity? GetById(long id, bool isDirect = false, bool isTracking = true)
    {
      try
      {
        AuthPermissionEntity? result = null;
        //string cacheKey = $"{cacheKeyDetail}:{id}";
        //if (!isDirect)
        //{
        //  result = CachedFunc.GetRedisData<AuthPermissionEntity>(cacheKey, null);
        //}

        //if (result == null)
        //{
          var dataSet = _context.Set<AuthPermissionEntity>();
          IQueryable<AuthPermissionEntity> queryable;
          if (!isTracking)
          {
            queryable = dataSet.AsNoTracking().AsQueryable();
          }
          else
          {
            queryable = dataSet.AsQueryable();
          }
          result = queryable
                .Include(entity => entity.ListAuthGroup)
              .Include(entity => entity.ListUser)/*[GEN-7]*/
                .Where(entity => entity.DeletedFlag != true && entity.Id == id)
                .FirstOrDefault();
          //result.AuthPermission2s = result.AuthPermission2s.Select(x => new AuthPermission2Entity() { Id = x.Id, Name = x.Name }).ToList();

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
    public override List<AuthPermissionEntity> GetList(PagingRequestModel pagingReq = null)
    {
      try
      {
        string cacheKey = $"{cacheKeyList}";
        if (pagingReq != null)
        {
          cacheKey = $"{cacheKeyList}:{pagingReq.Page}|{pagingReq.PageSize}";
        }
        List<AuthPermissionEntity>? result = CachedFunc.GetRedisData<List<AuthPermissionEntity>>(cacheKey, null);
        if (result == null)
        {
          result = new List<AuthPermissionEntity>();
          if (pagingReq != null)
          {
            result = _context.Set<AuthPermissionEntity>()
                .AsNoTracking()
                .AsQueryable()
                .Include(entity => entity.ListAuthGroup)
              .Include(entity => entity.ListUser)/*[GEN-11]*/
                .Where(entity => entity.DeletedFlag != true)
                .OrderBy(entity => entity.Order).ThenBy(entity => entity.Id)
                .Skip(pagingReq.GetSkip()).Take(pagingReq.GetLimit())
                .AsParallel()
                .ToList();

            //.Select(entity => new AuthPermissionEntity
            // {
            //   Id = entity.Id,
            //   AuthPermission2s = entity.AuthPermission2s.Select(x => new AuthPermission2Entity { Id = x.Id, Name = x.Name }).ToList()
            // })

            for (var i = 0; i < result.Count; i++)
            {

              result[i].ListAuthGroup = result[i].ListAuthGroup?.Select(x => new AuthGroupEntity() { Id = x.Id }).ToList();
              result[i].ListUser = result[i].ListUser?.Select(x => new UserEntity() { Id = x.Id }).ToList();
              /*[GEN-12]*/
            }
          }
          else
          {
            result = _context.Set<AuthPermissionEntity>()
                .AsNoTracking()
                .AsQueryable()
                .Include(entity => entity.ListAuthGroup)
              .Include(entity => entity.ListUser)/*[GEN-13]*/
                .Where(entity => entity.DeletedFlag != true)
                .OrderBy(entity => entity.Order).ThenBy(entity => entity.Id)
                .Skip(0).Take(ConstCommon.ConstSelectListMaxRecord)
                .AsParallel()
                .ToList();

            for (var i = 0; i < result.Count; i++)
            {

              result[i].ListAuthGroup = result[i].ListAuthGroup?.Select(x => new AuthGroupEntity() { Id = x.Id }).ToList();
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
    public AuthPermissionEntity Upsert(AuthPermissionEntity entity, List<AuthGroupEntity> authGroupChildren, List<UserEntity> authUserChildren/*[GEN-8]*/, long? userId = null)
    {
      try
      {
        if (entity.Id <= 0)
        {
          // Insert
          entity = Insert(entity, authGroupChildren, authUserChildren/*[GEN-4]*/, userId);
        }
        else
        {
          // Update
          entity = Update(entity, authGroupChildren, authUserChildren/*[GEN-4]*/, userId);
        }
        return entity;
      }
      catch (Exception ex)
      {
        throw new DBException(ex);
      }
    }
    public AuthPermissionEntity Insert(AuthPermissionEntity entity, List<AuthGroupEntity> authGroupChildren, List<UserEntity> authUserChildren/*[GEN-8]*/, long? userId = null)
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
          entity.ListAuthGroup = MergeChildrenEntity(entity.ListAuthGroup, authGroupChildren);

          entity.ListUser = MergeChildrenEntity(entity.ListUser, authUserChildren);
          /*[GEN-10]*/
          _context.Set<AuthPermissionEntity>().Add(entity);
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
    public AuthPermissionEntity Update(AuthPermissionEntity entity, List<AuthGroupEntity> authGroupChildren, List<UserEntity> authUserChildren/*[GEN-8]*/, long? userId = null)
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
          entity.ListAuthGroup = MergeChildrenEntity(entity.ListAuthGroup, authGroupChildren);

          entity.ListUser = MergeChildrenEntity(entity.ListUser, authUserChildren);
          /*[GEN-9]*/
          _context.Set<AuthPermissionEntity>().Update(entity);
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
    public override string GetDisplayField(AuthPermissionEntity entity)
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
