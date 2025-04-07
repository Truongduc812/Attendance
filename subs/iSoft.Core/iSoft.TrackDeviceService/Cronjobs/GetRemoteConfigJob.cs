using Common.Utils;
using iSoft.Common;
using iSoft.Common.Models.RemoteConfigModels;
using iSoft.TrackDeviceService.ConfigsNS;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;

namespace UDF.TrackDeviceService.CronjobsNS
{
	public class GetRemoteConfigJob
	{
		private Timer _timer;
		private readonly ILogger<GetRemoteConfigJob> logger;

		public GetRemoteConfigJob(ILoggerFactory loggerFactory)
		{
			this.logger = loggerFactory.CreateLogger<GetRemoteConfigJob>();
		}

		public void Start()
		{
			_timer = new Timer(getRemoteConfig, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
		}

		private async void getRemoteConfig(object state)
		{
			await RemoteConfig.RefreshConfig();
		}

		public void Stop()
		{
			_timer.Dispose();
		}
	}
}
