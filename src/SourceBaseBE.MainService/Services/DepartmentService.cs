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
using iSoft.Common.Models.ResponseModels;
using iSoft.Common.Utils;
using iSoft.ExcelHepler;
using OfficeOpenXml;
using PRPO.Database.Helpers;
using SourceBaseBE.Database.Models.RequestModels;
using SourceBaseBE.Database.Models.ResponseModels;
using System.IO;
using SourceBaseBE.Database.Helpers;
using iSoft.Common.Enums;

namespace SourceBaseBE.MainService.Services
{
	public class DepartmentService : BaseCRUDService<DepartmentEntity>
	{
		private UserRepository _authUserRepository;
		public DepartmentRepository _repositoryImp;
		public LanguageRepository _languageRepository;
		public DepartmentAdminRepository _departmentAdminRepository;
		public DepartmentRepository _departmentRepository;

        /*[GEN-1]*/

        public DepartmentService(CommonDBContext dbContext, ILogger<DepartmentService> logger)
		  : base(dbContext, logger)
		{
			_repository = new DepartmentRepository(_dbContext);
			_repositoryImp = (DepartmentRepository)_repository;
			_authUserRepository = new UserRepository(_dbContext);
			_languageRepository = new LanguageRepository(_dbContext);
            _departmentAdminRepository = new DepartmentAdminRepository(_dbContext);
			_departmentRepository = new DepartmentRepository(_dbContext);

            /*[GEN-2]*/
        }

		public override DepartmentEntity GetById(long id, bool isDirect = false, bool isTracking = true)
		{
			var entity = _repository.GetById(id, isDirect, isTracking);
			var entityRS = (DepartmentEntity)_authUserRepository.FillTrackingUser(entity);
			return entityRS;
		}
		public override async Task<DepartmentEntity> GetByIdAsync(long id, bool isDirect = false, bool isTracking = true)
		{
			var entity = await _repository.GetByIdAsync(id, isDirect);
			var entityRS = (DepartmentEntity)_authUserRepository.FillTrackingUser(entity);
			return entityRS;
		}
		public override List<DepartmentEntity> GetList(PagingRequestModel pagingReq = null)
		{
			var list = _repository.GetList(pagingReq);
			var listRS = _authUserRepository.FillTrackingUser(list.Cast<BaseCRUDEntity>().ToList()).Cast<DepartmentEntity>().ToList();
			return listRS;
		}
        public List<DepartmentEntity> GetDepartmentByListIds(List<long?> ips, PagingRequestModel pagingReq = null)
        {
            var list = _repositoryImp.GetDepartmentByListIds(ips, pagingReq);
            var listRS = _authUserRepository.FillTrackingUser(list.Cast<BaseCRUDEntity>().ToList()).Cast<DepartmentEntity>().ToList();
            return listRS;
        }
        public override List<Dictionary<string, object>> GetFormDataObjElement(DepartmentEntity entity)
		{
			string entityName = nameof(DepartmentEntity);
			var listRS = new List<Dictionary<string, object>>();
			List<object> objectList = null;
			var properties = typeof(DepartmentEntity).GetProperties()
													 .Where(p => p.Name == "Id" || p.Name == "Name" || p.Name == "Description" || p.Name == "Notes")
													 .ToList();
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
	{"isRequire",formDataTypeAttr.IsRequired }
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
	{"isRequire",formDataTypeAttr.IsRequired }
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
	{"isRequire",formDataTypeAttr.IsRequired }

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
	{"isRequire",formDataTypeAttr.IsRequired }
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
	{"isRequire",formDataTypeAttr.IsRequired }
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
	{"isRequire",formDataTypeAttr.IsRequired}
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
	{"isRequire",formDataTypeAttr.IsRequired}

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
	{"isRequire",formDataTypeAttr.IsRequired}

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
	{"isRequire",formDataTypeAttr.IsRequired}
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
	{"isRequire",formDataTypeAttr.IsRequired}
  });
							addedFlag = true;
							break;
						case EnumFormDataType.hidden:
							listRS.Add(new Dictionary<string, object> {
	  {"display_name", GetDisplayName(property.Name, entityName)},
	  {"key", property.Name},
	  {"value",property.GetValue(entity)},
	  {"type", formDataTypeAttr.TypeName.ToStringValue()},
	});


							addedFlag = true;
							break;
						default:
							listRS.Add(new Dictionary<string, object> {
	  {"display_name", GetDisplayName(property.Name, entityName)},
	  {"key", property.Name},
	  {"value", property.GetValue(entity)},
	  {"type", formDataTypeAttr.TypeName.ToStringValue()},
	  {"isRequire",formDataTypeAttr.IsRequired}
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
		public override DepartmentEntity Upsert(DepartmentEntity entity, long? userId = null)
		{

			/*[GEN-3]*/
			var upsertedEntity =base.Upsert(entity/*[GEN-4]*/, userId);
			var entityRS = (DepartmentEntity)_authUserRepository.FillTrackingUser(upsertedEntity);
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
		private List<long> GetListIdChildren(DepartmentEntity entity, string childEntity)
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


		public IEnumerable<DepartmentEntity> InsertMulti(IEnumerable<DepartmentEntity> entity, long? userId = null)
		{

			/*[GEN-3]*/
			var upsertedEntity = ((DepartmentRepository)_repository).InsertMulti(entity/*[GEN-4]*/, userId);
			return upsertedEntity;
		}

		public IEnumerable<DepartmentEntity> UpSertMulti(IEnumerable<DepartmentEntity> entity, long? userId = null)
		{

			/*[GEN-3]*/
			var upsertedEntity = ((DepartmentRepository)_repository).UpSertMulti(entity/*[GEN-4]*/, userId);
			return upsertedEntity;
		}
		public DepartmentEntity GetByName(string name, bool isDirect = false)
		{
			var entity = _repositoryImp.GetByName(name, isDirect);
			var entityRS = (DepartmentEntity)_authUserRepository.FillTrackingUser(entity);
			return entityRS;
		}

		public List<DepartmentEntity> GetAll(MasterDataRequestModel masterDataRequest)
		{
			if (masterDataRequest.Department == true)
			{
				return _repositoryImp.GetAll();
			}
			else
			{
				return null;
			}
		}
		public List<DepartmentEntity> GetAll()
		{
			var list = _repositoryImp.GetAll();
			var listRS = _authUserRepository.FillTrackingUser(list.Cast<BaseCRUDEntity>().ToList()).Cast<DepartmentEntity>().ToList();
			return listRS;
		}

		public AdminDepartmentPagingResponseModel GetListAdminSetting(
		  EnumDepartmentAdmin enumDepartmentAdmin,
		  PagingParamRequestModel pagingReq = null)
		{
			List<DepartmentListEmployeeResponseModel> listResponseModel = null;

			Dictionary<string, object> filterParams = StringToDictionaryHelper.ToStringAndObj(pagingReq.FilterStr);
			SearchModel searchParams = StringToDictionaryHelper.ToDicOrString(pagingReq.SearchStr, true);
			Dictionary<string, long> sortParams = StringToDictionaryHelper.ToStringLongTest(pagingReq.SortStr);
			var ret = _repositoryImp.GetListAdminDepartment(enumDepartmentAdmin, pagingReq, filterParams, searchParams, sortParams);
			var lang = string.IsNullOrEmpty(pagingReq.Language) ? EnumLanguage.EN.ToString() : pagingReq.Language.ToUpper();
			var disPlayProps = JsonPropertyHelper<DepartmentAdminListResponseModel>.GetFilterProperties();
			var columns = this._languageRepository.ReadColumnLanguage(disPlayProps, lang);
			columns = DepartmentAdminListResponseModel.AddKeySearchFilterable(columns);
			ret.Columns = columns;
			return ret;

		}
        public UserPagingResponseModel GetListUserAdmin(
          EnumDepartmentAdmin enumDepartmentAdmin,
          PagingParamRequestModel pagingReq = null)
        {
            List<UserPagingResponseModel> listResponseModel = null;

            Dictionary<string, object> filterParams = StringToDictionaryHelper.ToStringAndObj(pagingReq.FilterStr);
            SearchModel searchParams = StringToDictionaryHelper.ToDicOrString(pagingReq.SearchStr, true);
            Dictionary<string, long> sortParams = StringToDictionaryHelper.ToStringLongTest(pagingReq.SortStr);
			var listAdmin = _departmentAdminRepository.GetAll();
            var listDepartment = _departmentRepository.GetAll();
            var ret = _repositoryImp.GetListUserAdmin(enumDepartmentAdmin, pagingReq, filterParams, searchParams, sortParams, listAdmin,listDepartment);
            var lang = string.IsNullOrEmpty(pagingReq.Language) ? EnumLanguage.EN.ToString() : pagingReq.Language.ToUpper();
            var disPlayProps = JsonPropertyHelper<UserAdminListResponseModel>.GetFilterProperties();
            var columns = this._languageRepository.ReadColumnLanguage(disPlayProps, lang);
            columns = UserAdminListResponseModel.AddKeySearchFilterable(columns);
            ret.Columns = columns;
            return ret;

        }

        public override object GetDisplayName(string name, string entityName)
		{
			return $"{name}";
		}


	}
}