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
  [Route("api/v1/HolidayWorkingType")]
  public class HolidayWorkingTypeController : BaseCRUDController<HolidayWorkingTypeEntity, HolidayWorkingTypeRequestModel, HolidayWorkingTypeResponseModel>
  {
    private HolidayWorkingTypeService _service;
    public HolidayWorkingTypeController(HolidayWorkingTypeService service, ILogger<HolidayWorkingTypeController> logger)
      : base(service, logger)
    {
      _baseCRUDService = service;
      _service = (HolidayWorkingTypeService)_baseCRUDService;
    }
  }
}