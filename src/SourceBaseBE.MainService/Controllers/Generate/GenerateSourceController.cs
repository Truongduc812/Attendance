using Microsoft.AspNetCore.Mvc;
using Serilog;
using static iSoft.Common.Messages;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;
using SourceBaseBE.MainService.Services.Generate;
using SourceBaseBE.Database.Models.RequestModels.Generate;

namespace SourceBaseBE.MainService.Controllers.Generate
{
    [Authorize(Roles = "Root")]
  [ApiController]
  [Route("api/v1/GenerateSource")]
  public class GenerateSourceController : ControllerBase
  {
    private ILogger _logger = Serilog.Log.Logger;
    private GenerateSourceService _service;
    public GenerateSourceController(GenerateSourceService service)
    {
      
      _service = service;
    }

    [HttpPost]
    [Route("generate-crud-entity")]
    public async Task<IActionResult> GenerateCRUDEntity([FromForm] GenerateCRUDEntityRequestModel model)
    {
      string funcName = nameof(GenerateCRUDEntity);
      Message errMessage = null;

      try
      {
        _logger.LogMsg(IFuncStart_0, funcName);

        _service.BackupEntity(model.EntityName);
        _service.CloneFile(model.EntityName);
        _service.ReplaceFileData(model.EntityName);
        _service.UpdateByEntityBackup(model.EntityName);
        _service.UpdateReferenceFile(model.EntityName);

        _logger.LogMsg(ISuccess_0_1, funcName, "Done");
        return this.ResponseJSonObj("Done");
      }
      catch (Exception ex)
      {
        errMessage = ErrException.SetParameters(ex);
      }
      _logger.LogMsg(errMessage);
      return this.ResponseError(errMessage);
    }
  }
}