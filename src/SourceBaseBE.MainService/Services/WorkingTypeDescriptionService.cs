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
	public class WorkingTypeDescriptionService : BaseCRUDService<WorkingTypeDescriptionEntity>
	{
		private UserRepository _authUserRepository;
		public WorkingTypeDescriptionRepository _repositoryImp;
		public LanguageRepository _languageRepository;
		public DepartmentAdminRepository _departmentAdminRepository;
		public DepartmentRepository _departmentRepository;

        /*[GEN-1]*/

        public WorkingTypeDescriptionService(CommonDBContext dbContext, ILogger<WorkingTypeDescriptionService> logger)
		  : base(dbContext, logger)
		{
			_repository = new WorkingTypeDescriptionRepository(_dbContext);
			_repositoryImp = (WorkingTypeDescriptionRepository)_repository;
			_authUserRepository = new UserRepository(_dbContext);
			_languageRepository = new LanguageRepository(_dbContext);
            _departmentAdminRepository = new DepartmentAdminRepository(_dbContext);
			_departmentRepository = new DepartmentRepository(_dbContext);

            /*[GEN-2]*/
        } 
		/// <summary>
		/// UpsertIfNotExist (@GenCRUD)
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="userId"></param>
		/// <returns></returns>
		public override WorkingTypeDescriptionEntity Upsert(WorkingTypeDescriptionEntity entity, long? userId = null)
		{

			/*[GEN-3]*/
			var upsertedEntity =base.Upsert(entity/*[GEN-4]*/, userId);
			var entityRS = (WorkingTypeDescriptionEntity)_authUserRepository.FillTrackingUser(upsertedEntity);
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
		private List<long> GetListIdChildren(WorkingTypeDescriptionEntity entity, string childEntity)
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


		public IEnumerable<WorkingTypeDescriptionEntity> InsertMulti(IEnumerable<WorkingTypeDescriptionEntity> entity, long? userId = null)
		{

			/*[GEN-3]*/
			var upsertedEntity = ((WorkingTypeDescriptionRepository)_repository).InsertMulti(entity/*[GEN-4]*/, userId);
			return upsertedEntity;
		}

		public IEnumerable<WorkingTypeDescriptionEntity> UpSertMulti(IEnumerable<WorkingTypeDescriptionEntity> entity, long? userId = null)
		{

			/*[GEN-3]*/
			var upsertedEntity = ((WorkingTypeDescriptionRepository)_repository).UpSertMulti(entity/*[GEN-4]*/, userId);
			return upsertedEntity;
		}
		public WorkingTypeDescriptionEntity GetByName(string name, bool isDirect = false)
		{
			var entity = _repositoryImp.GetByName(name, isDirect);
			var entityRS = (WorkingTypeDescriptionEntity)_authUserRepository.FillTrackingUser(entity);
			return entityRS;
		}

        public WorkingTypeDescriptionEntity GetByWorkingTypeId(long workingTypeId, bool isDirect = false)
        {
            var entity = _repositoryImp.GetByWorkingTypeId(workingTypeId, isDirect);
            var entityRS = (WorkingTypeDescriptionEntity)_authUserRepository.FillTrackingUser(entity);
            return entityRS;
        }

        //public AdminDepartmentPagingResponseModel GetListAdminSetting(
        //  EnumDepartmentAdmin enumDepartmentAdmin,
        //  PagingParamRequestModel pagingReq = null)
        //{
        //	List<DepartmentListEmployeeResponseModel> listResponseModel = null;

        //	Dictionary<string, object> filterParams = StringToDictionaryHelper.ToStringAndObj(pagingReq.FilterStr);
        //	SearchModel searchParams = StringToDictionaryHelper.ToDicOrString(pagingReq.SearchStr, true);
        //	Dictionary<string, long> sortParams = StringToDictionaryHelper.ToStringLongTest(pagingReq.SortStr);
        //	var ret = _repositoryImp.GetListAdminDepartment(enumDepartmentAdmin, pagingReq, filterParams, searchParams, sortParams);
        //	var lang = string.IsNullOrEmpty(pagingReq.Language) ? EnumLanguage.EN.ToString() : pagingReq.Language.ToUpper();
        //	var disPlayProps = JsonPropertyHelper<DepartmentAdminListResponseModel>.GetFilterProperties();
        //	var columns = this._languageRepository.ReadColumnLanguage(disPlayProps, lang);
        //	columns = DepartmentAdminListResponseModel.AddKeySearchFilterable(columns);
        //	ret.Columns = columns;
        //	return ret;

        //}
        public override object GetDisplayName(string name, string entityName)
		{
			return $"{name}";
		}


	}
}