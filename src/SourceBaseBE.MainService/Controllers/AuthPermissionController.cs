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
  [Route("api/v1/AuthPermission")]
  public class AuthPermissionController : BaseCRUDController<AuthPermissionEntity, AuthPermissionRequestModel, AuthPermissionResponseModel>
  {
    private AuthPermissionService _service;
    public AuthPermissionController(AuthPermissionService service, ILogger<AuthPermissionController> logger)
      : base(service, logger)
    {
      
      _baseCRUDService = service;
      _service = (AuthPermissionService)_baseCRUDService;
    }
  }
}