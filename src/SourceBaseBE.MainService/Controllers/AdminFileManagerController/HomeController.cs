using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
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
using iSoft.Common.Util;
using iSoft.Common.ExtensionMethods;
using SourceBaseBE.CustomAttributes;
using SourceBaseBE.MainService.Models;

namespace SourceBaseBE.MainService.Controllers
{
  //[Authorize(Roles = "Admin,Root")]
  [ApiController]
  [SessionFilter]
  [Route("[controller]")]
  public class HomeController : Controller
  {
    private readonly ILogger logger;
    public HomeController(ILoggerFactory loggerFactory)
    {
      this.logger = loggerFactory.CreateLogger<HomeController>();
    }

    //[HttpGet]
    //[Route("Logout")]
    //public ActionResult Logout()
    //{
    //  HttpContext.Session.Remove("userinfo");
    //  return Redirect(Url.Action("Index", "Login"));
    //}

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

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}