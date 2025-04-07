
using ConnectionCommon.Connection;
using iSoft.TrackDeviceService.Services;
using iSoft.TrackDeviceService.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UDF.TrackDeviceService.Services.Interfaces
{
	public interface IOutputService : IConnService
	{
		public Task Run();

	}
}
