using iSoft.DBLibrary.DBConnections.Factory;
using iSoft.Common.Enums.DBProvider;
using Serilog;
using iSoft.Common.ConfigsNS;
using SourceBaseBE.Database.Repository;
using SourceBaseBE.Database.DBContexts;
using MathNet.Numerics.Statistics.Mcmc;

using System;
using iSoft.Common.Exceptions;
using System.Data;
using Microsoft.EntityFrameworkCore.Storage;
using iSoft.Common.Models.RequestModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using iSoft.Database.Entities;
using System.Linq;
using SourceBaseBE.MainService.Models;
using SourceBaseBE.Database.Enums;

using iSoft.Database.Extensions;
using iSoft.Common.Models;
using SourceBaseBE.Database.Entities;
using static iSoft.Common.ConstCommon;
using iSoft.Common;
using iSoft.Common.Enums;
using iSoft.Database.Models;
using UserEntity = SourceBaseBE.Database.Entities.UserEntity;
using ISoftProjectEntity = SourceBaseBE.Database.Entities.ISoftProjectEntity;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PRPO.Database.Helpers;
using SourceBaseBE.Database.Models.RequestModels;
using SourceBaseBE.Database.Models.ResponseModels;

namespace SourceBaseBE.MainService.Services
{
	public class WorkingDayApprovalService : BaseCRUDService<WorkingDayApprovalEntity>
	{
		private UserRepository _authUserRepository;
		public WorkingDayApprovalRepository _repositoryImp;
		public LanguageRepository languageRepository;

		/*[GEN-1]*/

		public WorkingDayApprovalService(CommonDBContext dbContext, ILogger<WorkingDayApprovalService> logger)
		  : base(dbContext, logger)
		{
			_repository = new WorkingDayApprovalRepository(_dbContext);
			_repositoryImp = (WorkingDayApprovalRepository)_repository;
			_authUserRepository = new UserRepository(_dbContext);
			languageRepository = new LanguageRepository(_dbContext);
			/*[GEN-2]*/
		}
		public override WorkingDayApprovalEntity GetById(long id, bool isDirect = false, bool isTracking = true)
		{
			var entity = _repository.GetById(id, isDirect, isTracking);
			var entityRS = (WorkingDayApprovalEntity)_authUserRepository.FillTrackingUser(entity);
			return entityRS;
		}
		public override async Task<WorkingDayApprovalEntity> GetByIdAsync(long id, bool isDirect = false, bool isTracking = true)
		{
			var entity = await _repository.GetByIdAsync(id, isDirect);
			var entityRS = (WorkingDayApprovalEntity)_authUserRepository.FillTrackingUser(entity);
			return entityRS;
		}
		public List<WorkingDayApprovalEntity> GetList(PagingFilterRequestModel pagingReq = null)
		{
			if (pagingReq == null) throw new ArgumentNullException("Invalid argument");

			Dictionary<string, object> filterParams = StringToDictionaryHelper.ToStringAndObj(pagingReq.FilterStr);
			SearchModel searchParams = StringToDictionaryHelper.ToDicOrString(pagingReq.SearchStr, true);
			Dictionary<string, long> sortParams = StringToDictionaryHelper.ToStringLongTest(pagingReq.SortStr);
			//


			// columns
			var props = JsonPropertyHelper<DetailAttendanceResponse>.GetDisplayNames();
			var keys = JsonPropertyHelper<DetailAttendanceResponse>.GetJsonPropertyNames();
			var lang = string.IsNullOrEmpty(pagingReq.Language) ? EnumLanguage.EN.ToString() : pagingReq.Language.ToUpper();
			var columns = this.languageRepository.ReadColumnLanguage(props, keys, lang);
			columns = DetailAttendanceResponse.AddKeySearchFilterable(columns);
			var list = _repository.GetList(pagingReq);
			var listRS = _authUserRepository.FillTrackingUser(list.Cast<BaseCRUDEntity>().ToList()).Cast<WorkingDayApprovalEntity>().ToList();
			return listRS;
		}

        public List<WorkingDayApprovalEntity> GetList(List<long>? ids)
        {
            var list = _repositoryImp.GetList(ids);
            return list;
        }

        public override List<Dictionary<string, object>> GetFormDataObjElement(WorkingDayApprovalEntity entity)
		{
			string entityName = nameof(WorkingDayApprovalEntity);
			var listRS = new List<Dictionary<string, object>>();
			List<object> objectList = null;
			var properties = typeof(WorkingDayApprovalEntity).GetProperties();
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
		public override WorkingDayApprovalEntity Upsert(WorkingDayApprovalEntity entity, long? userId = null)
		{

			/*[GEN-3]*/
			var upsertedEntity = ((WorkingDayApprovalRepository)_repository).Upsert(entity/*[GEN-4]*/, userId);
			var entityRS = (WorkingDayApprovalEntity)_authUserRepository.FillTrackingUser(upsertedEntity);
			return entityRS;
		}
		public override int Delete(long id, long? userId = null, bool isSoftDelete = true)
		{
			int deletedCount = _repository.Delete(id, userId, isSoftDelete);
			return deletedCount;
		}
        public List<WorkingDayApprovalEntity> DeleteMulti(List<WorkingDayApprovalEntity> timeSheetApprovals, long? userId = null, bool isSoftDelete = true)
        {
            var deletedEntities = _repositoryImp.DeleteMulti(timeSheetApprovals, userId, isSoftDelete).ToList();
            return deletedEntities;
        }

        /// <summary>
        /// GetListIdChildren (@GenCRUD)
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="childEntity"></param>
        /// <returns></returns>
        private List<long> GetListIdChildren(WorkingDayApprovalEntity entity, string childEntity)
		{
			switch (childEntity)
			{


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

				/*[GEN-6]*/
				default:
					break;
			}
			return listRS;
		}

		//public WorkingDayApprovalEntity UpsertTestTransaction(WorkingDayApprovalEntity entity, long? userId = null)
		//{
		//  using (var transaction = this._dbContext.Database.BeginTransaction())
		//  {
		//    try
		//    {
		//      _repository...
		//      _userRepository...
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