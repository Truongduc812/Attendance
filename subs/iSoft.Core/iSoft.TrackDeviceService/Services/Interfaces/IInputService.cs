using ConnectionCommon.Connection;
using iSoft.Common.Errors;
using iSoft.ConnectionCommon.IndustrialConn.TcpGroup.EtherCAT;
using iSoft.ConnectionCommon.MessageBroker;
using iSoft.TrackDeviceService.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace iSoft.TrackDeviceService.Services
{
	public interface IInputService : IConnService
	{
		Task InitAllConnections();
		Task InitAllConnectionsParrallel();
		Task<object> InputDataReceiveHandler(object data);
		Task StartAllConnectionsParrallel();
		Task StartConnectionWithId(ulong id);
		Task StartConnectionWithName(string name);
		
	}
}