using System;
using System.IO;
using System.Threading.Tasks;
using elFinder.NetCore;
using elFinder.NetCore.Drivers.FileSystem;
using iSoft.Common.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SourceBaseBE.CustomAttributes;

namespace SourceBaseBE.MainService.Controllers
{
  //[Authorize(Roles = "Admin,Root")]
  [ApiController]
  [SessionFilter]
  [Route("el-finder-file-system")]
  public class FileSystemController : Controller
  {
    private readonly IConfiguration _configuration;
    IWebHostEnvironment _env;

    public FileSystemController(IWebHostEnvironment env, IConfiguration configuration)
    {
      _env = env;
      _configuration = configuration;
    }

    // Url để client-side kết nối đến backend
    // /el-finder-file-system/connector
    [Route("connector")]
    public async Task<IActionResult> Connector()
    {
      var connector = GetConnector();
      return await connector.ProcessAsync(Request);
    }

    // Địa chỉ để truy vấn thumbnail
    // /el-finder-file-system/thumb
    [Route("thumb/{hash}")]
    public async Task<IActionResult> Thumbs(string hash)
    {
      var connector = GetConnector();
      return await connector.GetThumbnailAsync(HttpContext.Request, HttpContext.Response, hash);
    }

    private Connector GetConnector()
    {
      var rootPathDefault = ConfigUtil.GetAppSetting<string>("rootPathDefault");

      // Thư mục gốc lưu trữ là wwwwroot/[rootPathDefault] (đảm bảo có tạo thư mục này)
      string pathroot = rootPathDefault;

      var driver = new FileSystemDriver();

      string absoluteUrl = UriHelper.BuildAbsolute(Request.Scheme, Request.Host);
      var uri = new Uri(absoluteUrl);

      // .. ... wwwwroot/[rootPathDefault]
      string rootDirectory = Path.Combine(_env.WebRootPath, pathroot);

      // https://localhost:5001/[rootPathDefault]/
      string url = $"{uri.Scheme}://{uri.Authority}/{pathroot}/";
      string urlthumb = $"{uri.Scheme}://{uri.Authority}/el-finder-file-system/thumb/";


      var root = new RootVolume(rootDirectory, url, urlthumb)
      {
        //IsReadOnly = !User.IsInRole("Administrators")
        IsReadOnly = false, // Can be readonly according to user's membership permission
        IsLocked = false, // If locked, files and directories cannot be deleted, renamed or moved
        Alias = "Data", // Beautiful name given to the root/home folder
        ThumbnailSize = 0,
        MaxUploadSizeInKb = 1024 * 100,
      };


      driver.AddRoot(root);

      return new Connector(driver)
      {
        // This allows support for the "onlyMimes" option on the client.
        MimeDetect = MimeDetectOption.Internal
      };
    }
  }
}
