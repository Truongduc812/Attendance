using Common.Utils;
using ConnectionCommon;
using ConnectionCommon.Connection;
using ConnectionCommon.MessageBroker;
using iSoft.Common.Errors;
using iSoft.Common.Models.RemoteConfigModels.Subs;
using iSoft.Common.Utils;
using iSoft.ConnectionCommon.IndustrialConn;
using iSoft.ConnectionCommon.IndustrialConn.TcpGroup.EtherCAT;
using iSoft.ConnectionCommon.IndustrialConn.TcpGroup.MCProtocol;
using iSoft.ConnectionCommon.IndustrialConn.TcpGroup.Modbus;
using iSoft.ConnectionCommon.MessageBroker;
using iSoft.ConnectionCommon.MessageQueueNS;
using iSoft.ConnectionCommon.SocketIOs;
using iSoft.TrackDeviceService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using IConnection = ConnectionCommon.Connection.IConnection;

namespace iSoft.TrackDeviceService.Services
{
	public class ReadDeviceService : BaseService, IInputService
	{
		private Microsoft.Extensions.Logging.ILogger _logger;
		public List<IConnection> Connections { get; set; } = new List<IConnection>();
		public event EventHandler<Error> OnErrorOccure = null;
		private readonly ILoggerFactory loggerFactory;
		private readonly Microsoft.AspNetCore.Hosting.IApplicationLifetime appLifetime;
		private readonly StoreHolderService storeHolderService;
		public ReadDeviceService(ILoggerFactory loggerFactory,
			Microsoft.AspNetCore.Hosting.IApplicationLifetime appLifetime,
			StoreHolderService storeHolderService
			)
		{
			try
			{
				this.loggerFactory = loggerFactory;
				this._logger = loggerFactory.CreateLogger<ReadDeviceService>();
				this.appLifetime = appLifetime;
				this.storeHolderService = storeHolderService;
			}
			catch (Exception ex)
			{

				_logger.LogError(ex.Message);
			}
		}

		public async Task InitAllConnections()
		{
			if (Connections == null || Connections.Count <= 0)
				return;
			Connections.ForEach(async (cnn) =>
			{
				try
				{
					//await cnn.Init(cnn.Connection, loggerFactory, RabbitConfig);
				}
				catch (Exception ex)
				{
					OnErrorOccure?.Invoke(this, new Error()
					{
						Description = ex.Message,
						CreatedAt = DateTime.Now,
						Code = $"{cnn.Connection.Id} fail to init"
					});
				}
			});
		}

		public async Task InitAllConnectionsParrallel()
		{
			//if (Connections == null || Connections.Count <= 0)
			//	return;
			//var newConfig = UDF.MainDataServiceNS.ConfigsNS.RemoteConfig.GetConfig().RabbitMQConfig;
			//var result = Parallel.ForEach(Connections, async (cnn) =>
			//			{
			//				try
			//				{
			//					await cnn.Init(cnn.Connection, loggerFactory, newConfig);
			//				}
			//				catch (Exception ex)
			//				{
			//					OnErrorOccure?.Invoke(this, new Error()
			//					{
			//						Description = ex.Message,
			//						CreatedAt = DateTime.Now,
			//						Code = $"{cnn.Connection.Id} fail to init parallel"
			//					});
			//				}
			//			});
		}

		public async Task<object> InputDataReceiveHandler(object data)
		{
			try
			{
				var receive = data as iSoft.ConnectionCommon.Model.iMagPayloadMessage;
				if (receive == null)
				{
					OnErrorOccure?.Invoke(this, new Error()
					{
						Code = $"connection {receive.ConnectionId} receive data error",
						CreatedAt = DateTime.Now,
						Description = "industry data can not cast"
					});
					_logger.LogError($"Read Error ");
					return false;
				}
				if (receive.ArrEnv == null || receive.ArrEnv.Count == 0)
				{
					Log.Logger.Information($"ConnectionId {receive.ConnectionId} have no ArrEnv\r\n----------------\r\n");
					return true;
				}
				// 
				// update to store
				await storeHolderService.UpdateData(receive.ConnectionId.GetValueOrDefault(), receive);
				//
				return true;
			}
			catch (Exception ex)
			{

				Log.Logger.Error($"InputDataReceiveHandler Exception: {ex.Message}", ex);
				OnErrorOccure?.Invoke(this, new Error()
				{
					Code = "InputDataReceiveHandler Exception",
					CreatedAt = DateTime.Now,
					Description = ex.StackTrace
				});
				return false;
			}
		}
		private void RestartService()
		{
			appLifetime.StopApplication();

			string _currentProcess = Path.GetFullPath(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);

			Process.Start(_currentProcess);
		}
		public void HandleErrorConnection(Exception exception)
		{
			if (exception.Message.Contains("Symbol version is invalid"))
			{
				RestartService();
			}
		}
		public async Task StartAllConnections()
		{
			if (Connections == null || Connections.Count <= 0)
				return;
			Connections.ForEach(async (cnn) =>
			{
				try
				{
					await cnn.Connect();
					await cnn.StartRead(InputDataReceiveHandler, HandleErrorConnection);
				}
				catch (Exception ex)
				{
					OnErrorOccure?.Invoke(this, new Error()
					{
						Description = ex.Message,
						CreatedAt = DateTime.Now,
						Code = $"{cnn.Connection.Id} fail to connect"
					});
				}
			});
		}

		public async Task StartAllConnectionsParrallel()
		{
			if (Connections == null || Connections.Count <= 0)
				return;
			var result = Parallel.ForEach(Connections, async (cnn) =>
			{
				try
				{
					await cnn.Connect();
					await cnn.StartRead(InputDataReceiveHandler, HandleErrorConnection);
				}
				catch (Exception ex)
				{
					OnErrorOccure?.Invoke(this, new Error()
					{
						Description = ex.Message,
						CreatedAt = DateTime.Now,
						Code = $"{cnn.Connection.Id} fail to connect parallel"
					});
					await StartConnectionWithId(cnn.Connection.Id);
				}
			});

		}
		public async Task StartConnectionWithId(ulong id)
		{
			var cnn = Connections.FirstOrDefault(s => s.Connection.Id == id);
			await cnn.Connect();
		}

		public async Task StartConnectionWithName(string name)
		{
			var cnn = Connections.FirstOrDefault(s => s.Connection.DisplayName == name);
			await cnn.Connect();
		}

		public IConnService SetConnections(List<IConnection> connections)
		{
			Connections = connections;
			return this;
		}

		public IConnService AppendConnection(IConnection connection)
		{
			this.Connections.Add(connection);
			return this;
		}

		public async Task<IConnService> AppendAndStartConnection(IConnection connection)
		{
			this.Connections.Add(connection);
			Task.Run(async () =>
			{
				try
				{
					await connection.Init(connection.Connection);
					await connection.Connect();
					await connection.StartRead(InputDataReceiveHandler, HandleErrorConnection);
				}
				catch (Exception ex)
				{

					throw ex;
				}
			});

			return this;
		}

		public IConnService SetLogger(Microsoft.Extensions.Logging.ILogger loger)
		{
			this._logger = loger;
			return this;
		}
		public IConnection GetConnectionService(Connection connection)
		{
			switch (connection.ConnType)
			{
				case eConnType.ADS:
					return new EtherCAT()
					{
						Connection = connection,
					};
					break;
				case eConnType.ModbusTCP:
					return new ModbusConn2()
					{
						Connection = connection,
					};
				case eConnType.ModbusRTU:
					return new ModbusConn2()
					{
						Connection = connection,
					};
				case eConnType.MCProtocol:
					return new MCProtocolConn2()
					{
						Connection = connection,
					};
				case eConnType.SLMP:
					return new MCProtocolConn2()
					{
						Connection = connection,
					};
					break;
				case eConnType.Excel:
					break;
				case eConnType.TCP_IP:
					break;
				case eConnType.Rabbit:
					return RabbitConn.GetInstance();
					break;
				case eConnType.Kafka:
					return new KafkaConn()
					{
						Connection = connection,
					};
					break;
				case eConnType.OPC_UA:
					break;
				case eConnType.SerialText:
					break;
				case eConnType.SocketIO:
					return new SocketIoClient(loggerFactory.CreateLogger<SocketIoClient>())
					{
						Connection = connection,
					};
					break;
				default:
					return null;
					break;
			}
			return null;
		}
		public async Task<IConnService> AppendAndStartConnection(Connection connectionConfig)
		{
			Task.Run(async () =>
		 {
			 var iConnection = GetConnectionService(connectionConfig);
			 Connections.Add(iConnection);
			 await iConnection.Init(connectionConfig);
			 await iConnection.Connect();
			 await iConnection.StartRead(InputDataReceiveHandler, HandleErrorConnection);
		 });

			return this;
		}

		public async Task InitAllConnections(List<Connection> conns)
		{
			foreach (var con in conns)
			{
				await this.AppendAndStartConnection(con);
			}
			await storeHolderService.InitConnectionDatas(conns);
		}
		public async Task InitAllConnections(List<IConnection> conns)
		{
			foreach (var con in conns)
			{
				await this.AppendAndStartConnection(con);
			}
			await storeHolderService.InitConnectionDatas(conns.Select(x => x.Connection).ToList());
		}
		public async Task StopAll()
		{
			foreach (var con in Connections)
			{
				await con.StopRead();
				con.Dispose();
			}
		}
	}
}
