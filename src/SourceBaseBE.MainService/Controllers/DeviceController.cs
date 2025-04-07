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
using iSoft.Common.ConfigsNS;
using iSoft.DBLibrary.DBConnections.Factory;
using iSoft.DBLibrary.DBConnections.Interfaces;
using Microsoft.AspNetCore.Authorization;
using iSoft.Common.Models.RequestModels;
using Microsoft.Extensions.Logging;

namespace SourceBaseBE.MainService.Controllers
{
	[ApiController]
	[Route("api/v1/Device")]
	public class DeviceController : BaseCRUDController<DeviceEntity, DeviceRequestModel, DeviceResponseModel>
	{
		private DeviceService _service;
		public DeviceController(DeviceService service, ILogger<DeviceController> logger)
		  : base(service, logger)
		{
			_baseCRUDService = service;
			_service = (DeviceService)_baseCRUDService;
		}
		[HttpGet("get-list")]
		public async override Task<IActionResult> GetList([FromQuery] PagingRequestModel pagingRequestModel)
		{
			try
			{
				var listRet = _service.GetList(pagingRequestModel, true);
				return this.ResponseOk(listRet);
			}
			catch (Exception ex)
			{
				var message = ex.Message;
				this._logger.LogError(ex.Message);
				return this.ResponseError(new Messages.Message(iSoft.Common.Enums.EnumMessageType.Error, "400", ex.Message));

			}
		}

		[HttpGet]
		public async Task<IActionResult> GetDeviceById([FromQuery] int Id)
		{
			try
			{
				var device = await _service.GetByIdAsync(Id, isTracking: false);
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
		public async override Task<IActionResult> Upsert([FromForm] DeviceRequestModel requestModel)
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