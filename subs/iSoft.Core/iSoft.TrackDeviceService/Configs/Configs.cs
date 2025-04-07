using Common.Utils;
using iSoft.Common;
using iSoft.Common.Models.RemoteConfigModels;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Serilog.Events;
using Serilog;
using iSoft.Common.Services;
using iSoft.Common.Util;
using iSoft.Common.Exceptions;
using iSoft.Common.ResponseObjectNS;
using iSoft.Common.Utils;
using iSoft.Common.Security;

namespace iSoft.TrackDeviceService.ConfigsNS
{
	public class RemoteConfig
	{
		private static readonly Microsoft.Extensions.Logging.ILogger logger = LoggerFactory.Create(builder =>
		{
			// Cấu hình Serilog
			Log.Logger = new LoggerConfiguration()
					//.MinimumLevel.Error()
					.MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
					.WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level}] - {Message}{NewLine}{Exception}")
					.CreateLogger();

			builder.AddConsole().AddDebug().ClearProviders().AddSerilog(Log.Logger);

		}).CreateLogger<RemoteConfig>();

		private static UDFConfigModel instance;

		public static UDFConfigModel GetConfig()
		{
			if (instance == null)
			{
				Task.Run(async () => { instance = await GetRemoteConfig(); }).Wait();
			}
			if (instance == null)
			{
				//throw new BaseException("GetRemoteConfig error");
			}
			return instance;
		}

		public static async Task RefreshConfig()
		{
			var newConfig = await GetRemoteConfig(true);
			if (newConfig != null)
			{
				instance = newConfig;
			}
		}

		private static async Task<UDFConfigModel> GetRemoteConfig(bool isRefresh = false)
		{
			string funcName = "GetRemoteConfig";

			try
			{
				//logger.LogMsg(Messages.IFuncStart_0, funcName);

				var dic = new Dictionary<string, string>();
				var xApiKey = ConfigUtil.GetAppSetting<string>("AppSettings:X-Api-Key");
				var remoteConfigURL = ConfigUtil.GetAppSetting<string>("AppSettings:RemoteConfigURL") + $"?v={(instance == null ? "0" : instance.Version)}";
				dic.Add("X-Api-Key", xApiKey);
				HttpServiceResponse httpServiceResponse = await HttpService.GetAsync(remoteConfigURL, dic);

				if (httpServiceResponse.StatusCode == System.Net.HttpStatusCode.OK)
				{
					if (string.IsNullOrEmpty(httpServiceResponse.ResponseString))
					{
						if (isRefresh)
						{
							logger.LogInformation($"{funcName} RemoteConfig not change, v={instance.Version}");
							return null;
						}
					}
					ResponseObject ro = JsonConvert.DeserializeObject<ResponseObject>(httpServiceResponse.ResponseString);
					if (ro.Data == null || string.IsNullOrEmpty(ro.Data.ToString()))
					{
						if (isRefresh)
						{
							logger.LogInformation($"{funcName} RemoteConfig not change, v={instance.Version}");
							return null;
						}
						throw new Exception($"{funcName} Remote config data is null, version: " + instance.Version);
					}
					string jsonStr = DataCipher.DecryptASE(ro.Data.ToString());
					var rs = JsonConvert.DeserializeObject<UDFConfigModel>(jsonStr);

					logger.LogMsg(Messages.ISuccess_0_1, funcName, rs.GetLogStr());
					return rs;
				}
				else
				{
					throw new Exception($"{funcName} GetAsync error, response: " + httpServiceResponse.ResponseString);
				}
				//logger.LogMsg(Messages.ISuccess_0_1, funcName, config.GetLogStr());
			}
			catch (Exception ex)
			{
				var errMessage = Messages.ErrException.SetParameters($"{funcName} error", ex.Message);
				logger.LogMsg(errMessage);
				return null;
			}
		}
	}
}
