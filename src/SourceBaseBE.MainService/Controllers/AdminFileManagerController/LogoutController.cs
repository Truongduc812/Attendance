using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Runtime.Intrinsics.X86;
using static iSoft.Common.Messages;
using iSoft.Common.Enums;
using static iSoft.Common.ConstCommon;
using iSoft.Database.Entities;
using SourceBaseBE.MainService.Models;

namespace SourceBaseBE.MainService.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class LogoutController : Controller
  {

    private readonly ILogger logger;
    [TempData]
    public string message { get; set; }

    public LogoutController(ILoggerFactory loggerFactory)
    {
    }
    public ActionResult Index()
    {
      HttpContext.Session.Remove("userinfo");
      return Redirect(Url.Action("Index", "Login"));
    }
  }
}
