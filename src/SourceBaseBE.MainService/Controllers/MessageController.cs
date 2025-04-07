using Microsoft.AspNetCore.Mvc;
using Serilog;
using iSoft.Common.Exceptions;
using iSoft.Common;
using System;
using SourceBaseBE.Database.Models.ResponseModels;
using SourceBaseBE.Database.Entities;
using SourceBaseBE.MainService.Services;
using Microsoft.Extensions.Logging;
using SourceBaseBE.Database.Models.RequestModels.Generate;
using iSoft.Common.Models.RequestModels;
using iSoft.Common.Models.ResponseModels;
using MathNet.Numerics.Statistics.Mcmc;
using System.Collections.Generic;
using System.Threading.Tasks;
using SourceBaseBE.MainService.CustomAttributes;
using iSoft.Common.CommonFunctionNS;

namespace SourceBaseBE.MainService.Controllers
{
	[ApiController]
	[Route("api/v1/Message")]
	public class MessageController : BaseCRUDController<MessageEntity, MessageRequestModel, MessageResponseModel>
	{
		private MessageService _service;
		public MessageController(MessageService service, ILogger<MessageController> logger)
		  : base(service, logger)
		{
			_baseCRUDService = service;
			_service = (MessageService)_baseCRUDService;
		}

		[HttpGet]
    [UserPermission]
    [Route("get-list-by-user")]
		public async Task<IActionResult> GetListByUser([FromQuery] PagingRequestModel request)
		{
			string funcName = nameof(GetList);
			Messages.Message errMessage = null;

			try
			{
				this._logger.LogMsg(Messages.IFuncStart_0, funcName);

				PagingResponseModel rs = new PagingResponseModel();

				long? currentUserId = CommonFunction.GetCurrentUserId(this.HttpContext);
				if (currentUserId == null)
				{
					return NotFound();
				}

				List<MessageEntity> listEntity = _service.GetListByUser((long)currentUserId, request);

				if (listEntity == null)
				{
					return this.ResponseError(null);
				}

				long totalRecord = _service.GetTotalRecordByUserId((long)currentUserId);

				var listItemResponse = new MessageResponseModel().SetData(listEntity);

				rs.ListData = listItemResponse;
				rs.TotalRecord = totalRecord;

				this._logger.LogMsg(Messages.ISuccess_0_1, funcName, listEntity.Count);

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
	}
}