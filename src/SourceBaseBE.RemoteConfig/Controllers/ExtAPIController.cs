using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using iSoft.RemoteConfig.AnnontationFilter;
using System.IO;
using Newtonsoft.Json;
using System;
using Microsoft.Extensions.Logging;
using iSoft.Common.Enums;
using iSoft.Common;
using Microsoft.Extensions.Options;

using DotNetEnv;
using iSoft.Common.Utils;
using System.Collections.Generic;
using iSoft.Common.Security;
using static iSoft.Common.Messages;
using iSoft.RemoteConfig.Models;
using iSoft.Common.Models.ConfigModel;

namespace iSoft.RemoteConfig.Controllers
{
    [ApiController]
  [Route("api/v1/[controller]")]
  public class ExtAPIController : ControllerBase
  {
    private readonly ILogger<ConfigController> logger;
    public static Dictionary<string, int> dicAPIKey2Version = new Dictionary<string, int>();
    private static ApiKeyConfigModel apiKeyConfigModel = null;
    public ExtAPIController(ILoggerFactory loggerFactory)
    {
      this.logger = loggerFactory.CreateLogger<ConfigController>();
    }

    [HttpGet("get-data")]
    [ApiKeyAuthenticationFilter]
    public IActionResult GetData()
    {
      const string funcName = nameof(GetData);
      Message errMessage = null;
      try
      {
        this.logger.LogMsg(Messages.IFuncStart_0, funcName);

        var rs = new TestDataModel("Data1", 1);

        this.logger.LogMsg(Messages.ISuccess_0_1, funcName, rs);
        return this.ResponseOk(rs);
      }
      catch (Exception ex)
      {
        errMessage = Messages.ErrException.SetParameters(ex);
        this.logger.LogMsg(errMessage);
        return this.ResponseErrorCode(errMessage);
      }
    }
  }
}
