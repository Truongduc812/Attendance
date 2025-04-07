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
	[Route("api/v1/MasterDataEmployee")]
	public class MasterDataEmployeeController : BaseCRUDController<MasterDataEmployeeEntity, MasterDataEmployeeRequestModel, MasterDataEmployeeResponseModel>
	{
		private MasterDataService _service;
		public MasterDataEmployeeController(MasterDataService service, ILogger<MasterDataEmployeeController> logger)
		  : base(service, logger)
		{
			_baseCRUDService = service;
			_service = (MasterDataService)_baseCRUDService;
		}
	}
}