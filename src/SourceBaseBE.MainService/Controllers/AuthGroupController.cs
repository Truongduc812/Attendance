using Microsoft.AspNetCore.Mvc;
using SourceBaseBE.Database.Models.ResponseModels;
using SourceBaseBE.Database.Entities;
using SourceBaseBE.Database.Models.RequestModels;
using SourceBaseBE.MainService.Services;
using Microsoft.Extensions.Logging;

namespace SourceBaseBE.MainService.Controllers
{
  [ApiController]
  [Route("api/v1/AuthGroup")]
  public class AuthGroupController : BaseCRUDController<AuthGroupEntity, AuthGroupRequestModel, AuthGroupResponseModel>
  {
    private AuthGroupService _service;
    public AuthGroupController(AuthGroupService service, ILogger<AuthGroupController> logger)
      : base(service, logger)
    {
      _baseCRUDService = service;
      _service = (AuthGroupService)_baseCRUDService;
    }
  }
}