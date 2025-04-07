using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using iSoft.Database.Models.ResponseModels;
using iSoft.Database.Entities;
using iSoft.Database.Models.RequestModels;
using iSoft.Auth.Services;
using iSoft.Common.Exceptions;
using iSoft.Common.Models.RequestModels;
using iSoft.Common.Models.ResponseModels;
using iSoft.Common;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace iSoft.Auth.Controllers
{
  [ApiController]
  [Route("api/v1/ISoftProject")]
  public class ISoftProjectController : BaseCRUDController<ISoftProjectEntity, ISoftProjectRequestModel, ISoftProjectResponseModel>
  {
    private readonly ILogger<ISoftProjectController> _logger;
    private ISoftProjectService _serviceImp;
    public ISoftProjectController(ILoggerFactory loggerFactory, ISoftProjectService service)
      : base(loggerFactory, service)
    {
      _logger = loggerFactory.CreateLogger<ISoftProjectController>();
      _baseCRUDService = service;
      _serviceImp = (ISoftProjectService)_baseCRUDService;
    }
  }
}