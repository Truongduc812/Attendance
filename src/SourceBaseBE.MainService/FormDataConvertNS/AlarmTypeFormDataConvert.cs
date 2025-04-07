
//using iSoft.Database.Entities;
//using iSoft.DBLibrary.Entities;
//using NPOI.SS.Formula.Functions;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations.Schema;
//using SourceBaseBE.MainService.Models;
//using SourceBaseBE.Database.Entities;
//using SourceBaseBE.MainService.Enums;
//using SourceBaseBE.MainService.InitFunctionsNS;

//namespace SourceBaseBE.MainService.FormDataConvertNS 
//{

//  public static partial class FormDataConvertAlarmType<TEntity> where TEntity : BaseCRUDEntity, new()
//  {
//    public static List<Dictionary<string, object>> GetFormDataObjElement(AlarmTypeEntity entity)
//    {
//      string category = nameof(AlarmTypeEntity);
//      List<Dictionary<string, object>> listRS = new List<Dictionary<string, object>>();

//      var properties = typeof(AlarmTypeEntity).GetProperties();
//      foreach (var property in properties)
//      {
//        switch (property.Name)
//        {
//          case "Id":
//            listRS.Add(new Dictionary<string, object> {
//              {"display_name", GetDisplayName(property.Name, category)},
//              {"key", property.Name},
//              {"value", property.GetValue(entity)},
//              {"type", EnumFormDataType.hidden.ToStringValue()},
//            });
//            break;
//          case "CreatedBy":
//          case "CreatedAt":
//          case "UpdatedBy":
//          case "UpdatedAt":
//          case "DeletedFlag":
//            listRS.Add(new Dictionary<string, object> {
//              {"display_name", GetDisplayName(property.Name, category)},
//              {"key", property.Name},
//              {"value", property.GetValue(entity)},
//              {"type", EnumFormDataType.label.ToStringValue()},
//            });
//            break;
//          case "TimeIntervalInSeconds":
//          case "TimeIntervalInSeconds2":
//            listRS.Add(new Dictionary<string, object> {
//              {"display_name", GetDisplayName(property.Name, category)},
//              {"key", property.Name},
//              {"value", property.GetValue(entity)},
//              {"type", EnumFormDataType.timespan.ToStringValue()},
//            });
//            break;
//          default:

//            var foreignKeyAttribute = (ForeignKeyAttribute)Attribute.GetCustomAttribute(property, typeof(ForeignKeyAttribute));
//            if (foreignKeyAttribute != null)
//            {
//              string refTableName = foreignKeyAttribute.Name;

//              listRS.Add(new Dictionary<string, object> {
//                  {"display_name", GetDisplayName(property.Name, category)},
//                  {"key", property.Name},
//                  {"value", property.GetValue(entity)},
//                  {"type", EnumFormDataType.select.ToStringValue()},
//                  {"select_data", InitFunctions.GetListOptionData(refTableName)},
//                });
//            }
//            else
//            {
//              if (property.PropertyType == typeof(string)
//                || Nullable.GetUnderlyingType(property.PropertyType) == typeof(string))
//              {
//                listRS.Add(new Dictionary<string, object> {
//                  {"display_name", GetDisplayName(property.Name, category)},
//                  {"key", property.Name},
//                  {"value", property.GetValue(entity)},
//                  {"type", EnumFormDataType.textbox.ToStringValue()},
//                });
//              }
//              else if (property.PropertyType == typeof(DateTime)
//                || Nullable.GetUnderlyingType(property.PropertyType) == typeof(DateTime))
//              {
//                listRS.Add(new Dictionary<string, object> {
//                  {"display_name", GetDisplayName(property.Name, category)},
//                  {"key", property.Name},
//                  {"value", property.GetValue(entity)},
//                  {"type", EnumFormDataType.datetime.ToStringValue()},
//                  //{"min", DateTime.Now},
//                  //{"max", DateTime.Now},
//                });
//              }
//              else if (property.PropertyType == typeof(int)
//                || Nullable.GetUnderlyingType(property.PropertyType) == typeof(int)
//                || property.PropertyType == typeof(long)
//                || Nullable.GetUnderlyingType(property.PropertyType) == typeof(long)
//                || property.PropertyType == typeof(short)
//                || Nullable.GetUnderlyingType(property.PropertyType) == typeof(short)
//                || property.PropertyType == typeof(double)
//                || Nullable.GetUnderlyingType(property.PropertyType) == typeof(double))
//              {
//                listRS.Add(new Dictionary<string, object> {
//                  {"display_name", GetDisplayName(property.Name, category)},
//                  {"key", property.Name},
//                  {"value", property.GetValue(entity)},
//                  {"type", EnumFormDataType.number.ToStringValue()},
//                  //{"min", 0},
//                  //{"max", 1},
//                });
//              }
//              else if (property.PropertyType == typeof(bool)
//                || Nullable.GetUnderlyingType(property.PropertyType) == typeof(bool))
//              {
//                listRS.Add(new Dictionary<string, object> {
//                  {"display_name", GetDisplayName(property.Name, category)},
//                  {"key", property.Name},
//                  {"value", property.GetValue(entity)},
//                  {"type", EnumFormDataType.checkbox.ToStringValue()},
//                });
//              }
//            }

//            break;
//        }
//      }
//      return listRS;
//    }
//  }

//}
