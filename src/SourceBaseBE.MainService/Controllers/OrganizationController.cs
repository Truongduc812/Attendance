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
  [Route("api/v1/Organization")]
  public class OrganizationController : BaseCRUDController<OrganizationEntity, OrganizationRequestModel, OrganizationResponseModel>
  {
    private OrganizationService _service;
    public OrganizationController(OrganizationService service, ILogger<OrganizationController> logger)
      : base(service, logger)
    {
      
      _baseCRUDService = service;
      _service = (OrganizationService)_baseCRUDService;
    }
  }
}