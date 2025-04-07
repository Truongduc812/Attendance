using Microsoft.AspNetCore.Mvc;
using SourceBaseBE.Database.Models.ResponseModels;
using SourceBaseBE.Database.Entities;
using SourceBaseBE.Database.Models.RequestModels;
using SourceBaseBE.MainService.Services;
using Microsoft.Extensions.Logging;

namespace SourceBaseBE.MainService.Controllers
{
  [ApiController]
  [Route("api/v1/AuthToken")]
  public class AuthTokenController : BaseCRUDController<AuthTokenEntity, AuthTokenRequestModel, AuthTokenResponseModel>
  {
    private AuthTokenService _service;
    public AuthTokenController(AuthTokenService service, ILogger<AuthTokenController> logger)
      : base(service, logger)
    {
      
      _baseCRUDService = service;
      _service = (AuthTokenService)_baseCRUDService;
    }
  }
}