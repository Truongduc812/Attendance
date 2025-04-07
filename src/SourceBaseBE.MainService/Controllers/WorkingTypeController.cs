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
using iSoft.Common.Models.RequestModels;
using iSoft.Common.Models.ResponseModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.ComponentModel;
using iSoft.Common.Enums;
using SourceBaseBE.MainService.CustomAttributes;

namespace SourceBaseBE.MainService.Controllers
{
	[ApiController]
	[Route("api/v1/WorkingType")]
	public class WorkingTypeController : ControllerBase
	{
		internal Microsoft.Extensions.Logging.ILogger _logger;
		private WorkingTypeService _service;

		public WorkingTypeController(ILogger<WorkingTypeController> logger, WorkingTypeService service)
		{
			this._logger = logger;
			this._service = service;
		}

		[HttpGet]
		[Route("symbol")]
		public async Task<IActionResult> GetListEmployeeDepartmentSetting([FromQuery] PagingFilterRequestModel request)
		{
			string funcName = nameof(GetListEmployeeDepartmentSetting);
			Messages.Message errMessage = null;
			try
			{
				this._logger.LogMsg(Messages.IFuncStart_0, funcName);

				var rs = _service.GetListSymbol(request);

				this._logger.LogMsg(Messages.ISuccess_0_1, funcName, rs.TotalRecord);

				return this.ResponseJSonObj(rs);
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
		[Route("import")]
		[UserPermission(EnumDepartmentAdmin.Admin3)]
		[RequestSizeLimit(1024 * 1000 * 800)]
		public async Task<IActionResult> ImportFileSymbel([FromForm] WorkingTypeRequestModel request)
		{
			string funcName = nameof(ImportFileSymbel);
			Messages.Message errMessage = null;

			try
			{
				this._logger.LogMsg(Messages.IFuncStart_0, funcName);

				var ret = await _service.ImportFileSymbel(request);
				this._logger.LogMsg(Messages.ISuccess_0_1, funcName, ret);
				if (!ret)
				{
					return this.ResponseError(Messages.ErrImportExcelDataFormat);
				}
				return this.ResponseOk(ret);
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
			return this.ResponseError(Messages.ErrImportExcelProcessing);
		}

		[HttpGet]
    [UserPermission(EnumDepartmentAdmin.Admin3)]
    [Route("export")]
		public async Task<IActionResult> ExportSymbol([FromQuery] PagingRequestModel request)
		{
			string funcName = nameof(ExportSymbol);
			Messages.Message errMessage = null;

			try
			{
				this._logger.LogMsg(Messages.IFuncStart_0, funcName);


				var ret = await _service.ExportSymbol(request);

				//this._logger.LogMsg(Messages.ISuccess_0_1, funcName, ret.());


				return ret;
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

		[HttpGet]
		[Route("get-form-data")]
		public async Task<IActionResult> GetFormData([FromQuery] long? Id)
		{
			string funcName = nameof(GetFormData);
			Messages.Message errMessage = null;

			try
			{
				this._logger.LogMsg(Messages.IFuncStart_0, funcName);

				List<Dictionary<string, object>> formDataObj = null;
				if (Id == null)
				{
					formDataObj = _service.GetFormDataObjElement(new WorkingTypeEntity());
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
	}
}