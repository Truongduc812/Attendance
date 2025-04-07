using Microsoft.AspNetCore.Mvc;
using Serilog;
using iSoft.Common.Exceptions;
using iSoft.Common;
using System;
using SourceBaseBE.Database.Models.ResponseModels;
using SourceBaseBE.Database.Entities;
using SourceBaseBE.Database.Models.RequestModels;
using SourceBaseBE.MainService.Services;
using Microsoft.Extensions.Logging;

namespace SourceBaseBE.MainService.Controllers
{
  [ApiController]
  [Route("api/v1/Language")]
  public class LanguageController : BaseCRUDController<LanguageEntity, LanguageRequestModel, LanguageResponseModel>
  {
    private LanguageService _service;
    public LanguageController(LanguageService service, ILogger<LanguageController> logger)
      : base(service, logger)
    {
      _baseCRUDService = service;
      _service = (LanguageService)_baseCRUDService;
    }
  }
}