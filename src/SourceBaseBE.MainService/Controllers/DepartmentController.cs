using Microsoft.AspNetCore.Mvc;
using Serilog;
using iSoft.Common.Exceptions;
using iSoft.Common;
using System;
using SourceBaseBE.Database.Models.ResponseModels;
using SourceBaseBE.Database.Entities;
using SourceBaseBE.Database.Models.RequestModels;
using SourceBaseBE.MainService.Services;
using Microsoft.Extensions.Logging;
using MathNet.Numerics.Statistics.Mcmc;
using System.Collections.Generic;
using System.Threading.Tasks;
using iSoft.Common.Models.RequestModels;
using iSoft.Common.Models.ResponseModels;
using Microsoft.AspNetCore.Authorization;
using iSoft.Common.Enums;
using SourceBaseBE.MainService.CustomAttributes;
using iSoft.Redis.Services;

namespace SourceBaseBE.MainService.Controllers
{
	[ApiController]
	[Route("api/v1/Department")]
	public class DepartmentController : BaseCRUDController<DepartmentEntity, DepartmentRequestModel, DepartmentResponseModel>
	{
		private DepartmentService _service;
		public DepartmentController(DepartmentService service, ILogger<DepartmentController> logger)
		  : base(service, logger)
		{
			_baseCRUDService = service;
			_service = (DepartmentService)_baseCRUDService;
		}
		[HttpGet]
		[Route("get-form-data")]
		public override async Task<IActionResult> GetCreateFormData([FromQuery] long? Id)
		{
			string funcName = nameof(GetCreateFormData);
			Messages.Message errMessage = null;

			try
			{
				this._logger.LogMsg(Messages.IFuncStart_0, funcName);

				List<Dictionary<string, object>> formDataObj = null;
				if (Id == null)
				{
					formDataObj = _service.GetFormDataObjElement(new DepartmentEntity());
				}
				else
				{
					var entity = this._service.GetById((long)Id);
					if (entity == null)
					{
						return NotFound();
					}
					formDataObj = _service.GetFormDataObjElement(entity);
				}

				this._logger.LogMsg(Messages.ISuccess_0_1, funcName, formDataObj);
				return this.ResponseJSonObj(formDataObj);
			}
			catch (DBException ex)
			{
				errMessage = Messages.ErrDBException.SetParameters(ex);
			}
			catch (BaseException ex)
			{
				errMessage = Messages.ErrBaseException.SetParameters(ex);
			}
			catch (Exception ex)
			{
				errMessage = Messages.ErrException.SetParameters(ex);
			}
			this._logger.LogMsg(errMessage);
			return this.ResponseError(errMessage);
		}

		[HttpPost]
		//[Authorize]
		[UserPermission(EnumUserRole.Root, EnumDepartmentAdmin.Admin3)]
		[Route("upsert")]
		public override async Task<IActionResult> Upsert([FromForm] DepartmentRequestModel model)
		{
			//CachedFunc.ClearRedisAll();
			return await base.Upsert(model);
		}

		[HttpPost]
		//[Authorize]
		[UserPermission(EnumUserRole.Root, EnumDepartmentAdmin.Admin3)]
		[Route("delete")]
		public override async Task<IActionResult> Delete([FromQuery] long Id)
		{
			//CachedFunc.ClearRedisAll();
			return await base.Delete(Id);
		}
	}
}