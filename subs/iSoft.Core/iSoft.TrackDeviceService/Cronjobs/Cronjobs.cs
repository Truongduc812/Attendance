using iSoft.Common;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.Extensions.Logging;
using UDF.TrackDeviceService.Services;
using UDF.TrackDeviceService.Services.Interfaces;
using iSoft.ConnectionCommon.IndustrialConn.TcpGroup.EtherCAT;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using System.Threading;
using iSoft.TrackDeviceService.Services;
using iSoft.TrackDeviceService.CronjobsNS;

namespace UDF.TrackDeviceService.CronjobsNS
{
	public class Cronjobs : BackgroundService
	{

		private readonly ILogger<Cronjobs> logger;
		public GetRemoteConfigJob getRemoteConfigJob;
		public HandleConnJob handleDataJob;
		public Cronjobs(ILoggerFactory loggerFactory,
			TrackDeviceConfigService trackDeviceConfigService,
			IInputService push, IConfiguration configuration,
			HandleConnJob handleConnJob
			)
		{
			this.logger = loggerFactory.CreateLogger<Cronjobs>();
			getRemoteConfigJob = new GetRemoteConfigJob(loggerFactory);
			handleDataJob = handleConnJob;
		}

		public void StartAllJob()
		{
			string funcName = "StartAllJob";
			try
			{
				getRemoteConfigJob.Start();
			}
			catch (Exception ex)
			{
				this.logger.LogError($"{funcName} getRemoteConfigJob.Start() Error Exception, ex: {ex.Message}");
				try
				{
					getRemoteConfigJob.Stop();
				}
				catch (Exception ex2)
				{
					this.logger.LogError($"{funcName} getRemoteConfigJob.Stop() Error Exception, ex: {ex2.Message}");
				}
			}

			try
			{
				//handleDataJob.Start();
				//writeDataService.Run();
			}
			catch (Exception ex)
			{
				this.logger.LogError($"{funcName} handleDataJob.Start() Error Exception, ex: {ex.Message}");
				try
				{
					handleDataJob.Stop();
				}
				catch (Exception ex2)
				{
					this.logger.LogError($"{funcName} handleDataJob.Stop() Error Exception, ex: {ex2.Message}");
				}
			}
		}

		protected override Task ExecuteAsync(CancellationToken stoppingToken)
		{
			return Task.Run(() =>
			{
				StartAllJob();

			}, stoppingToken);
			//while (!stoppingToken.IsCancellationRequested)
			//{
			//}
		}
	}
}
