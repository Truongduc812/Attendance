using Microsoft.AspNetCore.Mvc;
using SourceBaseBE.Database.Models.ResponseModels;
using SourceBaseBE.Database.Entities;
using SourceBaseBE.Database.Models.RequestModels;
using SourceBaseBE.MainService.Services;
using Microsoft.Extensions.Logging;

namespace SourceBaseBE.MainService.Controllers
{
  [ApiController]
  [Route("api/v1/ISoftProject")]
  public class ISoftProjectController : BaseCRUDController<ISoftProjectEntity, ISoftProjectRequestModel, ISoftProjectResponseModel>
  {
    private ISoftProjectService _service;
    public ISoftProjectController(ISoftProjectService service, ILogger<ISoftProjectController> logger)
      : base(service, logger)
    {
      
      _baseCRUDService = service;
      _service = (ISoftProjectService)_baseCRUDService;
    }
  }
}