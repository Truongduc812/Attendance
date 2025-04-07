using iSoft.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace iSoft.Auth.Controllers
{
  [ApiController]
  [Route("api/v1/Dashboard")]
  public class DashboardController : ControllerBase
  {
    private readonly ILogger<DashboardController> logger;
    public DashboardController(ILoggerFactory loggerFactory)
    {
      this.logger = loggerFactory.CreateLogger<DashboardController>();
    }

    [HttpGet]
    public async Task<IActionResult> Dashboard()
    {
      string funcName = "Dashboard";

      this.logger.LogMsg(Messages.IFuncStart_0, funcName);

      this.logger.LogMsg(Messages.ISuccess_0_1, funcName, "Dashboard Works");

      return this.ResponseOk("Dashboard Works");
    }
  }
}
