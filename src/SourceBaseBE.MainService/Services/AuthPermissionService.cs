using Serilog;
using SourceBaseBE.Database.Repository;
using SourceBaseBE.Database.DBContexts;
using System;
using System.Data;
using iSoft.Common.Models.RequestModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using iSoft.Database.Entities;
using System.Linq;
using iSoft.Database.Extensions;
using SourceBaseBE.Database.Entities;
using static iSoft.Common.ConstCommon;
using iSoft.Common.Enums;
using iSoft.Database.Models;
using UserEntity = SourceBaseBE.Database.Entities.UserEntity;
using Microsoft.Extensions.Logging;

namespace SourceBaseBE.MainService.Services
{
  public class AuthPermissionService : BaseCRUDService<AuthPermissionEntity>
  {
    private UserRepository _authUserRepository;
    private AuthGroupRepository _authGroupRepository;
/*[GEN-1]*/

    public AuthPermissionService(CommonDBContext dbContext, ILogger<AuthPermissionService> logger)
      : base(dbContext, logger)
    {
      _repository = new AuthPermissionRepository(_dbContext);
      _authUserRepository = new UserRepository(_dbContext);
      this._authGroupRepository = new (this._dbContext);
/*[GEN-2]*/
    }
    public override AuthPermissionEntity GetById(long id, bool isDirect = false, bool isTracking = true)
    {
      var entity = _repository.GetById(id, isDirect, isTracking);
      var entityRS = (AuthPermissionEntity)_authUserRepository.FillTrackingUser(entity);
      return entityRS;
    }
    public override List<AuthPermissionEntity> GetList(PagingRequestModel pagingReq = null)
    {
      var list = _repository.GetList(pagingReq);
      var listRS = _authUserRepository.FillTrackingUser(list.Cast<BaseCRUDEntity>().ToList()).Cast<AuthPermissionEntity>().ToList();
      return listRS;
    }
    public override List<Dictionary<string, object>> GetFormDataObjElement(AuthPermissionEntity entity)
    {
      string entityName = nameof(AuthPermissionEntity);
      var listRS = new List<Dictionary<string, object>>();
      List<object> objectList = null;
      var properties = typeof(AuthPermissionEntity).GetProperties();
      bool addedFlag = false;
      foreach (var property in properties)
      {
        addedFlag = false;
        // foreignKeyAttribute
        var foreignKeyAttribute = (ForeignKeyAttribute)Attribute.GetCustomAttribute(property, typeof(ForeignKeyAttribute));
        if (foreignKeyAttribute != null && !addedFlag)
        {
          string parentEntity = foreignKeyAttribute.Name;
          listRS.Add(new Dictionary<string, object> {
                  {"display_name", GetDisplayName(property.Name, entityName)},
                  {"key", property.Name},
                  {"value", property.GetValue(entity)},
                  {"type", EnumFormDataType.select.ToStringValue()},
                  {"select_data", GetListOptionData(parentEntity, entityName, "")},
                });
          addedFlag = true;
        }

        // ListEntityAttribute
        var listEntityAttribute = (ListEntityAttribute)Attribute.GetCustomAttribute(property, typeof(ListEntityAttribute));
        if (listEntityAttribute != null && !addedFlag)
        {
          string childEntity = listEntityAttribute.EntityTargetName;

          listRS.Add(new Dictionary<string, object> {
                  {"display_name", GetDisplayName(property.Name, entityName)},
                  {"key", property.Name},
                  {"value", GetListIdChildren(entity, childEntity)},
                  {"type", EnumFormDataType.selectMulti.ToStringValue()},
                  {"select_multi_data", GetListOptionData(childEntity, entityName, listEntityAttribute.Category)},
                });
          addedFlag = true;
        }

        // ListEntityAttribute
        var formDataTypeAttr = (FormDataTypeAttribute)Attribute.GetCustomAttribute(property, typeof(FormDataTypeAttribute));

        if (formDataTypeAttr == null && !addedFlag)
        {
          if (property.PropertyType == typeof(string) || Nullable.GetUnderlyingType(property.PropertyType) == typeof(string))
          {
            formDataTypeAttr = new FormDataTypeAttribute(EnumFormDataType.textbox, false, property.Name);
          }
          else if (property.PropertyType == typeof(DateTime) || Nullable.GetUnderlyingType(property.PropertyType) == typeof(DateTime))
          {
            formDataTypeAttr = new FormDataTypeAttribute(EnumFormDataType.datetime, false);
          }
          else if (property.PropertyType == typeof(int) || Nullable.GetUnderlyingType(property.PropertyType) == typeof(int)
            || property.PropertyType == typeof(long) || Nullable.GetUnderlyingType(property.PropertyType) == typeof(long)
            || property.PropertyType == typeof(short) || Nullable.GetUnderlyingType(property.PropertyType) == typeof(short))
          {
            formDataTypeAttr = new FormDataTypeAttribute(EnumFormDataType.integerNumber, false);
          }
          else if (property.PropertyType == typeof(double) || Nullable.GetUnderlyingType(property.PropertyType) == typeof(double))
          {
            formDataTypeAttr = new FormDataTypeAttribute(EnumFormDataType.floatNumber, false);
          }
          else if (property.PropertyType == typeof(bool) || Nullable.GetUnderlyingType(property.PropertyType) == typeof(bool))
          {
            formDataTypeAttr = new FormDataTypeAttribute(EnumFormDataType.checkbox, false);
          }
        }

        if (formDataTypeAttr != null && !addedFlag)
        {
          objectList = new List<object>(formDataTypeAttr.Options);
          switch (formDataTypeAttr.TypeName)
          {
            case EnumFormDataType.integerNumber:
            case EnumFormDataType.floatNumber:
              listRS.Add(new Dictionary<string, object> {
                  {"display_name", GetDisplayName(property.Name, entityName)},
                  {"key", property.Name},
                  {"value", property.GetValue(entity)},
                  {"type", formDataTypeAttr.TypeName.ToStringValue()},
                  {"min", formDataTypeAttr.Min},
                  {"max", formDataTypeAttr.Max},
                  {"default_value", formDataTypeAttr.DefaultVal},
                  {"unit", formDataTypeAttr.Unit},
                });
              addedFlag = true;
              break;
            case EnumFormDataType.checkbox:
              listRS.Add(new Dictionary<string, object> {
                  {"display_name", GetDisplayName(property.Name, entityName)},
                  {"key", property.Name},
                  {"value", property.GetValue(entity)},
                  {"type", formDataTypeAttr.TypeName.ToStringValue()},
                  {"default_value", formDataTypeAttr.DefaultVal},
                  {"unit", formDataTypeAttr.Unit},
                });
              addedFlag = true;
              break;
            case EnumFormDataType.textbox:
              listRS.Add(new Dictionary<string, object> {
                  {"display_name", GetDisplayName(property.Name, entityName)},
                  {"key", property.Name},
                  {"value", property.GetValue(entity)},
                  {"type", formDataTypeAttr.TypeName.ToStringValue()},
                  {"placeholder", formDataTypeAttr.Placeholder},
                  {"unit", formDataTypeAttr.Unit},
                });
              addedFlag = true;
              break;
            case EnumFormDataType.label:
            case EnumFormDataType.readonlyType:
              listRS.Add(new Dictionary<string, object> {
                  {"display_name", GetDisplayName(property.Name, entityName)},
                  {"key", property.Name},
                  {"value", property.GetValue(entity)},
                  {"type", formDataTypeAttr.TypeName.ToStringValue()},
                  {"unit", formDataTypeAttr.Unit},
                });
              addedFlag = true;
              break;
            case EnumFormDataType.datetime:
              listRS.Add(new Dictionary<string, object> {
                  {"display_name", GetDisplayName(property.Name, entityName)},
                  {"key", property.Name},
                  {"value", property.GetValue(entity)},
                  {"type", formDataTypeAttr.TypeName.ToStringValue()},
                  {"min", formDataTypeAttr.Min},
                  {"max", formDataTypeAttr.Max},
                  {"default_value", formDataTypeAttr.DefaultVal},
                });
              addedFlag = true;
              break;
            case EnumFormDataType.dateOnly:
              string value = property.GetValue(entity) == null ? "" : ((DateTime)property.GetValue(entity)).ToString(ConstDateTimeFormat.YYYYMMDD);
              listRS.Add(new Dictionary<string, object> {
                  {"display_name", GetDisplayName(property.Name, entityName)},
                  {"key", property.Name},
                  {"value",value },
                  {"type", formDataTypeAttr.TypeName.ToStringValue()},
                  {"min", formDataTypeAttr.Min},
                  {"max", formDataTypeAttr.Max},
                  {"default_value", formDataTypeAttr.DefaultVal},
                });
              addedFlag = true;
              break;
            case EnumFormDataType.radio:
              //objectList = new List<object>(formDataTypeAttr.Options);
              listRS.Add(new Dictionary<string, object> {
                  {"display_name", GetDisplayName(property.Name, entityName)},
                  {"key", property.Name},
                  {"value", property.GetValue(entity)},
                  {"type", formDataTypeAttr.TypeName.ToStringValue()},
                  {"radio_data", GetListOptionData(objectList)},
                  {"default_value", formDataTypeAttr.DefaultVal},
                });
              addedFlag = true;
              break;
            case EnumFormDataType.select:
              //objectList = new List<object>(formDataTypeAttr.Options);
              listRS.Add(new Dictionary<string, object> {
                  {"display_name", GetDisplayName(property.Name, entityName)},
                  {"key", property.Name},
                  {"value", property.GetValue(entity)},
                  {"type", formDataTypeAttr.TypeName.ToStringValue()},
                  {"select_data", GetListOptionData(objectList)},
                  {"default_value", formDataTypeAttr.DefaultVal},
                });
              addedFlag = true;
              break;
            case EnumFormDataType.timespan:
              listRS.Add(new Dictionary<string, object> {
                  {"display_name", GetDisplayName(property.Name, entityName)},
                  {"key", property.Name},
                  {"value", property.GetValue(entity)},
                  {"type", formDataTypeAttr.TypeName.ToStringValue()},
                  {"min", formDataTypeAttr.Min},
                  {"max", formDataTypeAttr.Max},
                  {"default_value", formDataTypeAttr.DefaultVal},
                });
              addedFlag = true;
              break;
            case EnumFormDataType.image:
              listRS.Add(new Dictionary<string, object> {
                  {"display_name", GetDisplayName(property.Name, entityName)},
                  {"key", property.Name},
                  {"value", property.GetValue(entity)},
                  {"type", formDataTypeAttr.TypeName.ToStringValue()},
                  {"width", formDataTypeAttr.Width},
                  {"height", formDataTypeAttr.Height},
                });
              addedFlag = true;
              break;
            default:
              listRS.Add(new Dictionary<string, object> {
                  {"display_name", GetDisplayName(property.Name, entityName)},
                  {"key", property.Name},
                  {"value", property.GetValue(entity)},
                  {"type", formDataTypeAttr.TypeName.ToStringValue()},
                });
              addedFlag = true;
              break;
          }
        }
      }
      return listRS;
    }

    /// <summary>
    /// UpsertIfNotExist (@GenCRUD)
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    public override AuthPermissionEntity Upsert(AuthPermissionEntity entity, long? userId = null)
    {
      List<AuthGroupEntity> authGroupChildren = null;
      if (entity.AuthGroupIds != null && entity.AuthGroupIds.Count >= 1)
      {
        authGroupChildren = _authGroupRepository.GetListByListIds(entity.AuthGroupIds, true);
      }

      List<UserEntity> authUserChildren = null;
      if (entity.UserIds != null && entity.UserIds.Count >= 1)
      {
        authUserChildren = _authUserRepository.GetListByListIds(entity.UserIds, true);
      }
/*[GEN-3]*/
      var upsertedEntity = ((AuthPermissionRepository)_repository).Upsert(entity, authGroupChildren, authUserChildren/*[GEN-4]*/, userId);
      var entityRS = (AuthPermissionEntity)_authUserRepository.FillTrackingUser(upsertedEntity);
      return entityRS;
    }
    public override int Delete(long id, long? userId = null, bool isSoftDelete = true)
    {
      int deletedCount = _repository.Delete(id, userId, isSoftDelete);
      return deletedCount;
    }

    /// <summary>
    /// GetListIdChildren (@GenCRUD)
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="childEntity"></param>
    /// <returns></returns>
    private List<long> GetListIdChildren(AuthPermissionEntity entity, string childEntity)
    {
      switch (childEntity)
      {

        case nameof(AuthGroupEntity):
          if (entity.ListAuthGroup == null)
          {
            return new List<long>();
          }
          return entity.ListAuthGroup.Select(x => x.Id).ToList();

        case nameof(UserEntity):
          if (entity.ListUser == null)
          {
            return new List<long>();
          }
          return entity.ListUser.Select(x => x.Id).ToList();
/*[GEN-5]*/
        default:
          return new List<long>();
      }
    }

    /// <summary>
    /// GetListOptionData (@GenCRUD)
    /// </summary>
    /// <param name="targetEntity"></param>
    /// <param name="entityName"></param>
    /// <param name="category"></param>
    /// <returns></returns>
    public List<FormSelectOptionModel> GetListOptionData(string targetEntity, string entityName, string category)
    {
      var listRS = new List<FormSelectOptionModel>();
      switch (targetEntity)
      {
        case nameof(AuthGroupEntity):
            listRS = this._authGroupRepository.GetSelectData(entityName, category);
            break;

          case nameof(UserEntity):
            listRS = this._authUserRepository.GetSelectData(entityName, category);
            break;
/*[GEN-6]*/
        default:
          break;
      }
      return listRS;
    }

    //public AuthPermissionEntity UpsertTestTransaction(AuthPermissionEntity entity, long? userId = null)
    //{
    //  using (var transaction = this._dbContext.Database.BeginTransaction())
    //  {
    //    try
    //    {
    //      _repository...
    //      _authUserRepository...
    //      transaction.Commit();
    //    }
    //    catch (Exception ex)
    //    {
    //      try
    //      {
    //        transaction.Rollback();
    //      }
    //      catch { }
    //      throw new DBException(ex);
    //    }
    //  }
    //}
  }
}