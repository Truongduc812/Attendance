
//using iSoft.Database.Entities;
//using iSoft.DBLibrary.Entities;
//using NPOI.SS.Formula.Functions;
//using System;
//using System.Collections.Generic;
//using SourceBaseBE.MainService.Models;
//using SourceBaseBE.Database.Entities;
//using SourceBaseBE.MainService.Enums;

//namespace SourceBaseBE.MainService.FormDataConvertNS
//{

//  public class FormDataConvert<TEntity> where TEntity : BaseCRUDEntity, new()
//  {
//    #region Example FormDataTypeAttribute
//    // [
//    //   {
//    //     "display_name": "Id",
//    // 	"key": "Id",
//    //     "value": "12",
//    //     "type": "hidden",
//    //   },
//    //   {
//    //     "display_name": "StringValue",
//    // 	"key": "StringValue",
//    //     "value": "Nguyễn Văn A",
//    //     "type": "readonly",
//    //   },
//    //   {
//    //     "display_name": "Avatar",
//    // 	"key": "Avatar",
//    //     "value": "image/a.png",
//    // 	"width": "100px",
//    // 	"height": "100px",
//    //     "type": "image",
//    //   },
//    //   {
//    //     "display_name": "Date of birth",
//    // 	"key": "date1",
//    //     "value": "2023-12-30",
//    // 	"min": "2023-12-10",
//    // 	"max": "2024-01-15",
//    //     "type": "dateOnly",
//    //   },
//    //   {
//    //     "display_name": "Date of birth",
//    // 	"key": "date1",
//    //     "value": "2023-12-04T23:58:00.000",
//    // 	"min": "2023-12-04T23:58:00.000",
//    // 	"max": "2023-12-04T23:58:00.000",
//    //     "type": "datetime",
//    //   },
//    //   {
//    //     "display_name": "runTime",
//    // 	"key": "runTime",
//    //     "value": "3600",
//    //     "type": "timespan",
//    //   },
//    //   {
//    //     "display_name": "StringValue",
//    // 	"key": "StringValue",
//    //     "value": "Nguyễn Văn A",
//    //     "type": "label",
//    //   },
//    //   {
//    //     "display_name": "StringValue",
//    // 	"key": "StringValue",
//    //     "value": "Nguyễn Văn A",
//    //     "type": "textbox",
//    //   },
//    //   {
//    //     "display_name": "Gender",
//    // 	"key": "Gender",
//    //     "value": "true",
//    // 	"label": "Gender",
//    //     "type": "checkbox",
//    //   },
//    //   {
//    //     "display_name": "Gender",
//    // 	"key": "Gender",
//    //     "value": "2",
//    //     "type": "radio",
//    // 	"radio_data": [
//    //     	{
//    //     		"Id": 1,
//    //     		"display_name": "A"
//    //     	},
//    //     	{
//    //     		"Id": 2,
//    //     		"display_name": "B"
//    //     	},
//    //     	{
//    //     		"Id": 3,
//    //     		"display_name": "C"
//    //     	},
//    // 	]
//    //   },
//    //   {
//    //     "display_name": "Age",
//    // 	"key": "Age",
//    //     "value": 18,
//    // 	"min": "10",
//    // 	"max": "100",
//    //     "type": "number",
//    //   },
//    //	   {
//    //	     "display_name": "Category",
//    //	 	"key": "Category",
//    //	     "value": 18,
//    //	     "type": "select",
//    //	     "select_data":[
//    //	     	{
//    //	     		"Id": 1,
//    //	     		"display_name": "A"

//    //				 },
//    //	     	{
//    //	     		"Id": 18,
//    //	     		"display_name": "B"
//    //	     	},
//    //	     	{
//    //"Id": 19,
//    //	     		"display_name": "C"

//    //				 }
//    //	     ]
//    //	   },
//    // ]
//    #endregion

//    //public static List<Dictionary<string, object>> GetFormDataObjElement(TestCRUDEntity entity)
//    //{
//    //  string category = nameof(TestCRUDEntity);
//    //  List<Dictionary<string, object>> listRS = new List<Dictionary<string, object>>();

//    //  var properties = typeof(TestCRUDEntity).GetProperties();
//    //  foreach (var property in properties)
//    //  {
//    //    switch (property.Name)
//    //    {
//    //      case "Id":
//    //        listRS.Add(new Dictionary<string, object> {
//    //          {"display_name", "Id"},
//    //          {"key", "Id"},
//    //          {"value", entity.Id},
//    //          {"type", EnumFormDataType.hidden.ToStringValue()},
//    //        });
//    //        break;
//    //      case "CreatedBy":
//    //      case "CreatedAt":
//    //      case "UpdatedBy":
//    //      case "UpdatedAt":
//    //      case "DeletedFlag":
//    //        listRS.Add(new Dictionary<string, object> {
//    //          {"display_name", GetDisplayName(property.Name, category)},
//    //          {"key", property.Name},
//    //          {"value", property.GetValue(entity)},
//    //          {"type", EnumFormDataType.label.ToStringValue()},
//    //        });
//    //        break;
//    //      default:
//    //        if (property.PropertyType == typeof(string)
//    //          || Nullable.GetUnderlyingType(property.PropertyType) == typeof(string))
//    //        {
//    //          listRS.Add(new Dictionary<string, object> {
//    //            {"display_name", GetDisplayName(property.Name, category)},
//    //            {"key", property.Name},
//    //            {"value", property.GetValue(entity)},
//    //            {"type", EnumFormDataType.textbox.ToStringValue()},
//    //          });
//    //        }
//    //        else if (property.PropertyType == typeof(DateTime)
//    //          || Nullable.GetUnderlyingType(property.PropertyType) == typeof(DateTime))
//    //        {
//    //          listRS.Add(new Dictionary<string, object> {
//    //            {"display_name", GetDisplayName(property.Name, category)},
//    //            {"key", property.Name},
//    //            {"value", property.GetValue(entity)},
//    //            {"type", EnumFormDataType.datetime.ToStringValue()},
//    //            //{"min", DateTime.Now},
//    //            //{"max", DateTime.Now},
//    //          });
//    //        }
//    //        else if (property.PropertyType == typeof(int)
//    //          || property.PropertyType == typeof(long)
//    //          || Nullable.GetUnderlyingType(property.PropertyType) == typeof(int)
//    //          || Nullable.GetUnderlyingType(property.PropertyType) == typeof(long))
//    //        {
//    //          listRS.Add(new Dictionary<string, object> {
//    //            {"display_name", GetDisplayName(property.Name, category)},
//    //            {"key", property.Name},
//    //            {"value", property.GetValue(entity)},
//    //            {"type", EnumFormDataType.number.ToStringValue()},
//    //            //{"min", 0},
//    //            //{"max", 1},
//    //          });
//    //        }
//    //        else if (property.PropertyType == typeof(bool)
//    //          || Nullable.GetUnderlyingType(property.PropertyType) == typeof(bool))
//    //        {
//    //          listRS.Add(new Dictionary<string, object> {
//    //            {"display_name", GetDisplayName(property.Name, category)},
//    //            {"key", property.Name},
//    //            {"value", property.GetValue(entity)},
//    //            {"type", EnumFormDataType.checkbox.ToStringValue()},
//    //          });
//    //        }
//    //        break;
//    //    }
//    //  }
//    //  return listRS;
//    //}

//    //public static List<Dictionary<string, object>> GetFormDataObj(TEntity entity)
//    //{
//    //  //if (entity is TestCRUDEntity)
//    //  //{
//    //  //  return GetFormDataObjElement(entity as TestCRUDEntity);
//    //  //}
//    //  //else if (entity is EntryRequestEntity)
//    //  //{
//    //  //  return GetFormDataObjElement(entity as EntryRequestEntity);
//    //  //}

//    //  //else if (entity is AreaCodeEntity)
//    //  //{
//    //  //  return GetFormDataObjElement(entity as AreaCodeEntity);
//    //  //}
//    //  //else if (entity is EntryRequestTypeEntity)
//    //  //{
//    //  //  return GetFormDataObjElement(entity as EntryRequestTypeEntity);
//    //  //}
//    //  //else if (entity is EntryTransactionTypeEntity)
//    //  //{
//    //  //  return GetFormDataObjElement(entity as EntryTransactionTypeEntity);
//    //  //}
//    //  //else if (entity is VehicleTypeEntity)
//    //  //{
//    //  //  return GetFormDataObjElement(entity as VehicleTypeEntity);
//    //  //}
//    //  //else if (entity is GoodTypeEntity)
//    //  //{
//    //  //  return GetFormDataObjElement(entity as GoodTypeEntity);
//    //  //}
//    //  //else if (entity is CameraEntity)
//    //  //{
//    //  //  return GetFormDataObjElement(entity as CameraEntity);
//    //  //}
//    //  //else if (entity is AlarmTypeEntity)
//    //  //{
//    //  //  return GetFormDataObjElement(entity as AlarmTypeEntity);
//    //  //}
//    //  //else if (entity is ComponentMasterEntity)
//    //  //{
//    //  //  return GetFormDataObjElement(entity as ComponentMasterEntity);
//    //  //}
//    //  //else if (entity is PageMasterEntity)
//    //  //{
//    //  //  return GetFormDataObjElement(entity as PageMasterEntity);
//    //  //}
//    //  //else if (entity is PageWEntity)
//    //  //{
//    //  //  return GetFormDataObjElement(entity as PageWEntity);
//    //  //}
//    //  //else if (entity is ComponentWEntity)
//    //  //{
//    //  //  return GetFormDataObjElement(entity as ComponentWEntity);
//    //  //}
//    //  //else if (entity is ComponentParamEntity)
//    //  //{
//    //  //  return GetFormDataObjElement(entity as ComponentParamEntity);
//    //  //}
//    //  //else if (entity is ComponentParamMasterEntity)
//    //  //{
//    //  //  return GetFormDataObjElement(entity as ComponentParamMasterEntity);
//    //  //}
//    //  //else
//    //  {
//    //    throw new NotImplementedException();
//    //  }
//    //}
//    public virtual List<Dictionary<string, object>> GetFormDataObjElement(BaseCRUDEntity entity)
//    {
//      return new List<Dictionary<string, object>>();
//    }
//    public static object GetDisplayName(string fieldName, string category = "system")
//    {
//      Dictionary<string, string> dicFieldName2DisplayName = new Dictionary<string, string>()
//      {
//        // TODO: get from db, trans en/vi
//      };
//      if (dicFieldName2DisplayName.ContainsKey(fieldName))
//      {
//        return dicFieldName2DisplayName[fieldName];
//      }
//      return fieldName;
//    }
//  }

//}
