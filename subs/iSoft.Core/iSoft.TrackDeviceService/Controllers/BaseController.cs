using iSoft.DBLibrary.DBConnections.Factory;
using iSoft.DBLibrary.Entities;
using iSoft.DBLibrary.Enums.Provider;
using iSoft.DBLibrary.SQLBuilder;
using iSoft.Extensions.EntityFrameworkCore.Repository;
using iSoft.TrackDeviceService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace iSoft.TrackDeviceService.Api.Controllers
{
	[ApiController]
	[Route("api/v1/[controller]")]
	public class BaseController<T> : ControllerBase where T : BaseEntity
	{
		protected IGenericRepository<T> repository;
		protected IInputService cnnService;
		private readonly ILogger<T> logger;
		protected virtual string TableName { get; set; }
		public BaseController(IGenericRepository<T> repository, IInputService connectionService, ILoggerFactory logger)
		{
			var config = iSoft.TrackDeviceService.ConfigsNS.RemoteConfig.GetConfig().MasterDatabaseConfig;
			this.repository = repository;
			this.cnnService = connectionService;
			this.logger = logger.CreateLogger<T>();
		}
	}
}
