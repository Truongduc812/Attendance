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
  public class LoginController : Controller
  {

    private readonly ILogger logger;
    [TempData]
    public string message { get; set; }

    public LoginController(ILoggerFactory loggerFactory)
    {
    }
    private ActionResult Error(string error)
    {
      ViewData["Error"] = error;
      return View("Login");
    }
    // GET: LoginController
    public ActionResult Index()
    {
      return View("Login");
    }

    //GET: LoginController/Create
    [HttpPost]
    [Route("PostLogin")]
    public ActionResult PostLogin([FromForm] LoginRequestModel loginInfo)
    {
      try
      {
        if (loginInfo.username == "root" && loginInfo.password == "vuletech@113")
        {
          var user = new UserEntity()
          {
            Id = 0,
            Username = loginInfo.username,
            Password = loginInfo.password,
            Role = EnumUserRole.Root.ToString(),
          };
          HttpContext.Session.SetString("userinfo", JsonConvert.SerializeObject(user));
          return Redirect(Url.Action("Setting", "Home"));
        }
        else
        {
          return Error("Invalid username or password!");
        }
      }
      catch (Exception ex)
      {
        message = "Login fail!";
        //return Redirect(Url.Action("PostLogin", "Home"));
        return Error("Server error!");
      }

    }



  }
}
