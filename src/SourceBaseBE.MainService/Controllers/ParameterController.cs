using Microsoft.AspNetCore.Mvc;
using Serilog;
using iSoft.Common.Exceptions;
using iSoft.Common;
using System;
using SourceBaseBE.Database.Models.ResponseModels;
using SourceBaseBE.Database.Entities;
using SourceBaseBE.Database.Models.RequestModels;
using SourceBaseBE.MainService.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using iSoft.Common.Models.RequestModels;
using SourceBaseBE.MainService.Models.RequestModels;
using Microsoft.Extensions.Logging;

namespace SourceBaseBE.MainService.Controllers
{
	[ApiController]
	[Route("api/v1/Parameter")]
	public class ParameterController : BaseCRUDController<ParameterEntity, ParameterRequestModel, ParameterResponseModel>
	{
		private ParameterService _service;
		public ParameterController(ParameterService service, ILogger<ParameterController> logger)
		  : base(service, logger)
		{
			
			_baseCRUDService = service;
			_service = (ParameterService)_baseCRUDService;
		}
		[HttpGet("get-list")]
		public async override Task<IActionResult> GetList([FromQuery] PagingRequestModel pagingRequestModel)
		{
			try
			{
				var listRet = _service.GetList(pagingRequestModel);
				return this.ResponseOk(listRet);
			}
			catch (Exception ex)
			{
				var message = ex.Message;
				this._logger.LogError(ex.Message);
				return this.ResponseError(new Messages.Message(iSoft.Common.Enums.EnumMessageType.Error, "400", ex.Message));

			}
		}
		[HttpGet("get-list-by-device")]
		public async  Task<IActionResult> GetListByDeviceId([FromQuery] ParameterRequestPagingModel pagingRequestModel)
		{
			try
			{
				var listRet = _service.GetListByDevice(pagingRequestModel);
				return this.ResponseOk(listRet);
			}
			catch (Exception ex)
			{
				var message = ex.Message;
				this._logger.LogError(ex.Message);
				return this.ResponseError(new Messages.Message(iSoft.Common.Enums.EnumMessageType.Error, "400", ex.Message));

			}
		}
		[HttpGet("{id:int}")]
		public async Task<IActionResult> GetListDevice(int id)
		{
			try
			{
				var device = await _service.GetByIdAsync(id, isTracking: false);
				return this.ResponseOk(device);
			}
			catch (Exception ex)
			{
				var message = ex.Message;
				this._logger.LogError(ex.Message);
				return this.ResponseError(new Messages.Message(iSoft.Common.Enums.EnumMessageType.Error, "400", message));

			}
		}
		[HttpPost]
		[Authorize]
		[Route("upsert")]
		public async override Task<IActionResult> Upsert([FromForm] ParameterRequestModel requestModel)
		{
			if (requestModel == null)
			{
				return this.ResponseError(new Messages.Message(iSoft.Common.Enums.EnumMessageType.Error, "400", "Invalid input parameter"));
			}
			try
			{
				_service.Upsert(requestModel.GetEntity());
				return this.Ok("UpsertDevice Success");
			}
			catch (Exception ex)
			{
				var message = ex.Message;
				this._logger.LogError(ex.Message);
				return this.ResponseError(new Messages.Message(iSoft.Common.Enums.EnumMessageType.Error, "400", message));

			}
		}
		[HttpPost]
		[Authorize]
		[Route("delete")]
		public virtual async Task<IActionResult> Delete([FromQuery] long Id)
		{
			if (Id <= 0)
			{
				return this.ResponseError(new Messages.Message(iSoft.Common.Enums.EnumMessageType.Error, "400", "Invalid input parameter"));
			}
			try
			{
				_service.Delete(Id);
				return this.Ok("DeleteDevice  Success");
			}
			catch (Exception ex)
			{
				var message = ex.Message;
				this._logger.LogError(ex.Message);
				return this.ResponseError(new Messages.Message(iSoft.Common.Enums.EnumMessageType.Error, "400", message));

			}
		}
	}
}