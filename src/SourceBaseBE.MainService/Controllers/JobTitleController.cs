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
  [Route("api/v1/JobTitle")]
  public class JobTitleController : BaseCRUDController<JobTitleEntity, JobTitleRequestModel, JobTitleResponseModel>
  {
    private JobTitleService _service;
    public JobTitleController(JobTitleService service, ILogger<JobTitleController> logger)
      : base(service, logger)
    {
      _baseCRUDService = service;
      _service = (JobTitleService)_baseCRUDService;
    }
  }
}