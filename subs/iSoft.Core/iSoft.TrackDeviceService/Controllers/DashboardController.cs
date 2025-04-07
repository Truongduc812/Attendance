using Common.Utils;
using iSoft.Common;
using iSoft.Common.Exceptions;
using iSoft.Common.ResponseObjectNS;
using iSoft.DBLibrary.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using static iSoft.Common.Messages;
using System.IO;
using System.Net.Http;
using System.Collections.Generic;

namespace UDF.TrackDeviceService.Controllers
{
	[ApiController]
	[Route("api/v1/[controller]")]
	public class DashboardController : ControllerBase
	{
		private readonly ILogger<DashboardController> logger;
		public DashboardController(ILoggerFactory loggerFactory)
		{
			this.logger = loggerFactory.CreateLogger<DashboardController>();
		}

		[HttpGet("/dashboard")]
		public async Task<IActionResult> Dashboard()
		{
			string funcName = "Dashboard";

			this.logger.LogMsg(Messages.IFuncStart_0, funcName);

			this.logger.LogMsg(Messages.ISuccess_0_1, funcName, "Dashboard Works");

			return this.ResponseOk("iSoft JSC TrackDeviceService API@Copyright2023ByTrungDung");
		}


	}
}
