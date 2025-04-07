using iSoft.RemoteConfig.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace iSoft.RemoteConfig.Controllers
{
  //[Authorize(Roles = "Root")]
  [ApiController]
  [SessionFilter]
  [Route("/file-manager")]
  public class FileManagerController : Controller
  {
    public IActionResult Index()
    {
      var loginUser = HttpContext.Session.GetString("userinfo");
      if (loginUser == null)
      {
        return Redirect(Url.Action("Login", "Home"));
      }
      return View();
    }
  }
}
