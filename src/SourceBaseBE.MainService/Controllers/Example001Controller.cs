using Microsoft.AspNetCore.Mvc;
using SourceBaseBE.Database.Models.ResponseModels;
using SourceBaseBE.Database.Entities;
using SourceBaseBE.Database.Models.RequestModels;
using SourceBaseBE.MainService.Services;
using Microsoft.Extensions.Logging;

namespace SourceBaseBE.MainService.Controllers
{
  [ApiController]
  [Route("api/v1/Example001")]
  public class Example001Controller : BaseCRUDController<Example001Entity, Example001RequestModel, Example001ResponseModel>
  {
    private Example001Service _service;
    public Example001Controller(Example001Service service, ILogger<Example001Controller> logger)
      : base(service, logger)
    {
      
      _baseCRUDService = service;
      _service = (Example001Service)_baseCRUDService;
    }
  }
}