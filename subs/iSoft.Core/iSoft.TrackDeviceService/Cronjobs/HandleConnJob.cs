using Common.Utils;
using ConnectionCommon.Connection;
using iSoft.Common;
using iSoft.Common.Enums;
using iSoft.Common.Models.RemoteConfigModels.Subs;
using iSoft.Common.Util;
using iSoft.ConnectionCommon.IndustrialConn.TcpGroup.MCProtocol;
using iSoft.ConnectionCommon.MessageQueueNS;
using iSoft.ConnectionCommon.SocketIOs;
using iSoft.TrackDeviceService.ConfigsNS;
using iSoft.TrackDeviceService.Models;
using iSoft.TrackDeviceService.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UDF.TrackDeviceService.Services.Interfaces;

namespace iSoft.TrackDeviceService.CronjobsNS
{
	public class HandleConnJob
	{
		private Timer _timer;
		private readonly ILogger<HandleConnJob> logger;
		private readonly ILoggerFactory loggerFactory;
		//private volatile RealtimeService realtimeService;
		private volatile IInputService inputService;
		private volatile IOutputService outputService;
		private volatile TrackDeviceConfigService trackDeviceConfigService;
		private RabbitMQConfigModel lastMQConfig;
		private IConfiguration Configuration;
		private volatile bool isInitDone = false;
		private volatile bool isIniting = false;
		private bool isDevRealtime = true;
		public HandleConnJob(ILoggerFactory loggerFactory, TrackDeviceConfigService trackDeviceConfigService,
			IInputService connectionService,
			IOutputService outputService,
			IConfiguration configuration)
		{
			this.loggerFactory = loggerFactory;
			this.logger = loggerFactory.CreateLogger<HandleConnJob>();
			this.Configuration = configuration;
			this.inputService = connectionService;
			this.trackDeviceConfigService = trackDeviceConfigService;
			this.outputService = outputService;
			_timer = new Timer(pushData, null, TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(30));
		}
		public async Task Start(TrackDeviceConfig trackDeviceConfig)
		{
			if (isIniting || trackDeviceConfig == null) return;
			try
			{
				isIniting = true;
				await inputService.StopAll();
			
				// get params
				var inputConn = trackDeviceConfigService.GetConnectionsWithRelationByCommGroup(eComGroup.Input).Result;
				var outputConn = trackDeviceConfigService.GetConnectionsWithRelationByCommGroup(eComGroup.Output).Result;
		
				var listConn = new List<IConnection>();
				if (inputConn == null || inputConn.Count <= 0)
				{
					listConn.Add(new MCProtocolConn2()
					{
						Connection = MCProtocolConn2.CreateTemplate("Mitsu1", new List<ConnectionParam>()
						{
							new ConnectionParam()
							{
								 Param= new Param()
								 {
										Address="D0",
										ReadWrite= eReadWrite.ReadWrite,
										Name="D0",
										Type="Int16"
								 },
							}
						})
					});
				}
				await outputService.InitAllConnections(outputConn);
				await inputService.InitAllConnections(inputConn);
				isInitDone = true;
				isIniting = false;
			}
			catch (Exception ex)
			{
				isInitDone = false;
				throw ex;
			}

		}
		private async void pushData(object state)
		{
			try
			{
				var newConfig = RemoteConfig.GetConfig()?.RabbitMQConfig;
				if (!newConfig.Equals(lastMQConfig))
				{
					lastMQConfig = newConfig;
					isInitDone = false;

				}
				if (!isInitDone && lastMQConfig != null)
				{
				
					await Start(RemoteConfig.GetConfig()?.TrackDeviceConfig);
				}
			}
			catch (Exception ex)
			{
				this.logger.LogMsg(Messages.ErrException.SetParameters(ex));
			}
		}

		public void Stop()
		{
			_timer.Dispose();
		}
	}
}
