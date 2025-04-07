using Microsoft.AspNetCore.Mvc;
using Serilog;
using iSoft.Common.Exceptions;
using iSoft.Common;
using System;
using SourceBaseBE.Database.Models.ResponseModels;
using SourceBaseBE.Database.Entities;
using SourceBaseBE.MainService.Services;
using Microsoft.Extensions.Logging;
using SourceBaseBE.Database.Models.RequestModels.Generate;

namespace SourceBaseBE.MainService.Controllers
{
    [ApiController]
  [Route("api/v1/GenTemplate")]
  public class GenTemplateController : BaseCRUDController<GenTemplateEntity, GenTemplateRequestModel, GenTemplateResponseModel>
  {
    private GenTemplateService _service;
    public GenTemplateController(GenTemplateService service, ILogger<GenTemplateController> logger)
      : base(service, logger)
    {
      _baseCRUDService = service;
      _service = (GenTemplateService)_baseCRUDService;
    }
  }
}