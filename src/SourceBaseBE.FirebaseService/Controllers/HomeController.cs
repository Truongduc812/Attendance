using System;
using System.Diagnostics;
using iSoft.Common;
using iSoft.Database.Entities;
using iSoft.Firebase.Models;
using iSoft.Firebase.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace iSoft.Firebase.Controllers
{
  //[Authorize]
  [Route("[controller]")]
  public class HomeController : Controller
  {
    private readonly ILogger<FirebaseController> logger;
    private readonly FirebaseService firebaseService;
    public HomeController(ILoggerFactory loggerFactory, FirebaseService firebaseService)
    {
      this.logger = loggerFactory.CreateLogger<FirebaseController>();
      this.firebaseService = firebaseService;
    }

    [HttpGet]
    [Route("")]
    public IActionResult Index()
    {
      return View();
    }

    [HttpPost]
    [Route("StoreToken")]
    public IActionResult StoreToken(string token)
    {
      try
      {
        token = token == null ? string.Empty : token;
        string funcName = nameof(token);

        FCMEntity entity = new FCMEntity();
        entity.UserId = 8; // TODO: get userID
        entity.Token = token;

        var rs = firebaseService.Upsert(entity);

        this.logger.LogMsg(Messages.ISuccess_0_1, funcName, rs);
        return this.ResponseOk(rs);
      }
      catch (Exception ex)
      {
        var message = Messages.ErrException.SetParameters(ex);
        this.logger.LogMsg(message);
        return this.ResponseError(message);
      }
    }

    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

  }
}