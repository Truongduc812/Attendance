using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using InfoPortal.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using static iSoft.Common.Messages;
using iSoft.Common;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using iSoft.RemoteConfig.Models;
using iSoft.Common.Util;
using iSoft.Common.ExtensionMethods;
using iSoft.RemoteConfig.Helpers;

namespace iSoft.RemoteConfig.Controllers
{
    //[Authorize(Roles = "Admin,Root")]
    [SessionFilter]
  [ApiController]
  [Route("[controller]")]
  public class HomeController : Controller
  {
    private readonly ILogger logger;
    public HomeController(ILoggerFactory loggerFactory)
    {
      this.logger = loggerFactory.CreateLogger<HomeController>();
    }
    [HttpGet]
    [Route("Logout")]
    public ActionResult Logout()
    {
      HttpContext.Session.Remove("userinfo");
      return Redirect(Url.Action("Index", "Login"));
    }
    [HttpGet]
    [Route("Setting")]
    public IActionResult Setting()
    {


      ViewBag.linkSaveSettings = Url.Action("SaveSettings", "Home");
      ViewBag.homePageUrl = Url.Action("Setting", "Home");
      ViewBag.settings = new MySettingsModel("abc");
      //ViewData[""]
      //ViewBag.topFolders = this.GetTopFolders();
      return View("SettingView");
    }



    [TempData]
    public string message { get; set; }

    [HttpPost]
    [Route("SaveSettings")]
    public IActionResult SaveSettings([FromBody] MySettingsModel settings)
    {
      string funcName = "SaveSettings";
      Message errMessage = null;
      try
      {
        this.logger.LogMsg(Messages.IFuncStart_0, funcName);
        this.logger.LogInformation("{0}", funcName);

        ConfigController.dicAPIKey2Version.Clear();

        var rs = "done";
        this.logger.LogMsg(Messages.ISuccess_0_1, funcName, rs);

        return this.ResponseOk(rs);
      }
      catch (Exception ex)
      {
        this.logger.LogInformation("{0} Input: {1}", funcName, settings.ToJson());
        errMessage = Messages.ErrException.SetParameters(ex);
        this.logger.LogMsg(errMessage);
        return this.ResponseError(errMessage);
      }

    }

    private List<string> GetTopFolders()
    {

      var pathroot = ConfigUtil.GetAppSetting<string>("rootPathDefault");
      string searchPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", $"{pathroot}");

      List<string> filePaths = Directory
        .GetDirectories(searchPath, "*", SearchOption.TopDirectoryOnly)
        .Select(path => Path.GetFileName(path))
        .Where(path => !path.Contains("."))
        .ToList();

      return filePaths;
    }

    //public IActionResult Index()
    //{
    //  var oeeUrlDefault = _configuration.GetValue<string>("OEEUrlDefault");
    //  var displayTimeDefault = _configuration.GetValue<int>("displayTimeDefault");
    //  var linkGetData = Url.Action("GetData", "Home");
    //  ViewBag.cModel = new HomeModel(oeeUrlDefault, displayTimeDefault, linkGetData);
    //  return View("HomeView");
    //}

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}