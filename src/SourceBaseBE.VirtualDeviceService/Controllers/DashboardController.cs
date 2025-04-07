using SourceBaseBE.Database.DTOs;
using SourceBaseBE.Database.Entities;
using iSoft.Common;
using iSoft.Common.Exceptions;
using iSoft.Common.ResponseObjectNS;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using static iSoft.Common.Messages;
using SourceBaseBE.VirtualDeviceService.Services;
using Microsoft.Extensions.DependencyInjection;
using iSoft.RabbitMQ.Payload;

namespace SourceBaseBE.VirtualDeviceService.Controllers
{
	[ApiController]
	[Route("api/v1/[controller]")]
	public class DashboardController : ControllerBase
	{
		private readonly ILogger<DashboardController> _logger;
		public DashboardController(ILogger<DashboardController> logger)
		{
			_logger = logger;
		}

		[HttpGet]
		[Route("")]
		public async Task<IActionResult> Dashboard()
		{
			string funcName = "Dashboard";

			_logger.LogMsg(Messages.IFuncStart_0, funcName);

			_logger.LogMsg(Messages.ISuccess_0_1, funcName, "Dashboard Works");

			return this.ResponseOk("Dashboard Works");
		}
		[HttpPost]
		[Route("/fake-data")]
		public async Task<IActionResult> FakeData(DevicePayloadMessage payload)
		{
			string funcName = "FakeData";

			_logger.LogMsg(Messages.IFuncStart_0, funcName);

			_logger.LogMsg(Messages.ISuccess_0_1, funcName, "Dashboard Works");

			return this.ResponseOk("Dashboard Works");
		}
	}
}
