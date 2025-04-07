using ConnectionCommon.Connection;
using iSoft.Common.Errors;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UDF.TrackDeviceService.Services.Interfaces;

namespace iSoft.TrackDeviceService.Services.Interfaces
{
	public interface IConnService
	{
		IConnService SetConnections(List<IConnection> connections);
		List<IConnection> Connections { get; protected set; }
		event EventHandler<Error> OnErrorOccure;
		IConnService SetLogger(ILogger loger);
		IConnection GetConnectionService(Connection connType);
		Task InitAllConnections(List<Connection> conns);
		Task InitAllConnections(List<IConnection> conns);
		Task StartAllConnections();
		public Task StopAll();
		void HandleErrorConnection(Exception error);
		IConnService AppendConnection(IConnection connection);
		Task<IConnService> AppendAndStartConnection(IConnection connection);
		Task<IConnService> AppendAndStartConnection(Connection connection);

	}
}
