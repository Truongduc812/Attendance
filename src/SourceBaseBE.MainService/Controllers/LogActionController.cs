using iSoft.Common;
using iSoft.SocketIOClientNS.Services;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using SocketIOClient;
using System.Diagnostics;
using System.Threading.Tasks;
using iSoft.Common.ConfigsNS;
using System.IO;
using System;
using SourceBaseBE.MainService.Services;
using SourceBaseBE.MainService.Models.RequestModels;
using iSoft.Common.Models.RequestModels;

namespace SourceBaseBE.MainService.Controllers
{
	[ApiController]
	[Route("api/v1/LogActionRequest")]
	public class LogActionController : ControllerBase
	{
		private ILogger _logger = Serilog.Log.Logger;
		private readonly WorkingDayUpdateService workingDayUpdateService;
		private readonly WorkingDayApprovalService workingDayApprovalService;
		public LogActionController(WorkingDayApprovalService workingDayApprovalService, WorkingDayUpdateService workingDayUpdateService)
		{
			this.workingDayApprovalService = workingDayApprovalService;
			this.workingDayUpdateService = workingDayUpdateService;
		}

		//[HttpGet]
		//[Route("list-logs")]
		//public async Task<IActionResult> GetListLogs(PagingFilterRequestModel req )
		//{
		//	Messages.Message errMessage = null;

		//	try
		//	{
		//		var listData= 
			
		//	}
		//	catch (Exception ex)
		//	{
		//		errMessage = Messages.ErrException.SetParameters(ex);
		//	}
		//	this._logger.LogMsg(errMessage);
		//	return this.ResponseError(errMessage);
		//}


	}
}
