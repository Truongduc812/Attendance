using iSoft.Common.ConfigsNS;
using iSoft.Common.Enums.DBProvider;
using iSoft.Common.Exceptions;
using iSoft.Common.Models.ConfigModel;
using iSoft.Common.Models.ConfigModel.Subs;
using iSoft.Common.ResponseObjectNS;
using iSoft.Common.Security;
using iSoft.Common.Services;
using iSoft.Common.Utils;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog;

namespace iSoft.Common.Util
{
	public class ConfigUtil
	{

		public static SourceBaseBEConfigModel FillEnvKey(SourceBaseBEConfigModel configModel)
		{
			if (Environment.GetEnvironmentVariable("ENV") == null)
			{
				DotNetEnv.Env.Load();
			}

			configModel.ENV = Environment.GetEnvironmentVariable("ENV");

			configModel.Version = Environment.GetEnvironmentVariable("Version");

			configModel.AuthenticationSecret = Environment.GetEnvironmentVariable("AuthenticationSecret");

			string masterDatabaseDatabaseType = Environment.GetEnvironmentVariable("MasterDatabaseConfig_DatabaseType");
			string masterDatabaseAddress = Environment.GetEnvironmentVariable("MasterDatabaseConfig_Address");
			string masterDatabasePort = Environment.GetEnvironmentVariable("MasterDatabaseConfig_Port");
			string masterDatabaseDatabaseName = Environment.GetEnvironmentVariable("MasterDatabaseConfig_DatabaseName");
			string masterDatabaseUsername = Environment.GetEnvironmentVariable("MasterDatabaseConfig_Username");
			string masterDatabasePassword = Environment.GetEnvironmentVariable("MasterDatabaseConfig_Password");

			string traceDatabaseDatabaseType = Environment.GetEnvironmentVariable("TraceDatabaseConfig_DatabaseType");
			string traceDatabaseAddress = Environment.GetEnvironmentVariable("TraceDatabaseConfig_Address");
			string traceDatabasePort = Environment.GetEnvironmentVariable("TraceDatabaseConfig_Port");
			string traceDatabaseDatabaseName = Environment.GetEnvironmentVariable("TraceDatabaseConfig_DatabaseName");
			string traceDatabaseUsername = Environment.GetEnvironmentVariable("TraceDatabaseConfig_Username");
			string traceDatabasePassword = Environment.GetEnvironmentVariable("TraceDatabaseConfig_Password");

			string rabbitMQAddress = Environment.GetEnvironmentVariable("RabbitMQConfig_Address");
			string rabbitMQPort = Environment.GetEnvironmentVariable("RabbitMQConfig_Port");
			string rabbitMQUsername = Environment.GetEnvironmentVariable("RabbitMQConfig_Username");
			string rabbitMQPassword = Environment.GetEnvironmentVariable("RabbitMQConfig_Password");

			string elasticSearchAddress = Environment.GetEnvironmentVariable("ElasticSearchConfig_Address");
			string elasticSearchPort = Environment.GetEnvironmentVariable("ElasticSearchConfig_Port");
			string elasticSearchUsername = Environment.GetEnvironmentVariable("ElasticSearchConfig_Username");
			string elasticSearchPassword = Environment.GetEnvironmentVariable("ElasticSearchConfig_Password");

			string redisAddress = Environment.GetEnvironmentVariable("RedisConfig_Address");
			string redisPort = Environment.GetEnvironmentVariable("RedisConfig_Port");
			string redisUsername = Environment.GetEnvironmentVariable("RedisConfig_Username");
			string redisPassword = Environment.GetEnvironmentVariable("RedisConfig_Password");

			string socketIOAddress = Environment.GetEnvironmentVariable("SocketIOConfig_Address");
			string socketIOPort = Environment.GetEnvironmentVariable("SocketIOConfig_Port");

			string remoteConfigAddress = Environment.GetEnvironmentVariable("RemoteConfig_Address");
			string remoteConfigPort = Environment.GetEnvironmentVariable("RemoteConfig_Port");

			string remoteConfigAPIKey = Environment.GetEnvironmentVariable("RemoteConfig_APIKey");

			string influxDBAddress = Environment.GetEnvironmentVariable("InfluxDBConfig_Address");
			string influxDBPort = Environment.GetEnvironmentVariable("InfluxDBConfig_Port");
			string influxDBUsername = Environment.GetEnvironmentVariable("InfluxDBConfig_Username");
			string influxDBPassword = Environment.GetEnvironmentVariable("InfluxDBConfig_Password");
			string influxDBToken = Environment.GetEnvironmentVariable("InfluxDBConfig_Token");
			string influxDBOrganization = Environment.GetEnvironmentVariable("InfluxDBConfig_Organization");
			string influxDBDatabaseName = Environment.GetEnvironmentVariable("InfluxDBConfig_DatabaseName");

			string FCMConfig_Server_ServerKey = Environment.GetEnvironmentVariable("FCMConfig_Server_ServerKey");
			string FCMConfig_Server_MessagingSenderId = Environment.GetEnvironmentVariable("FCMConfig_Server_MessagingSenderId");
			string FCMConfig_Server_DefaultIcon = Environment.GetEnvironmentVariable("FCMConfig_Server_DefaultIcon");
			string FCMConfig_Server_DefaultVibrate = Environment.GetEnvironmentVariable("FCMConfig_Server_DefaultVibrate");
			string FCMConfig_Client_ApiKey = Environment.GetEnvironmentVariable("FCMConfig_Client_ApiKey");
			string FCMConfig_Client_AuthDomain = Environment.GetEnvironmentVariable("FCMConfig_Client_AuthDomain");
			string FCMConfig_Client_ProjectId = Environment.GetEnvironmentVariable("FCMConfig_Client_ProjectId");
			string FCMConfig_Client_StorageBucket = Environment.GetEnvironmentVariable("FCMConfig_Client_StorageBucket");
			string FCMConfig_Client_MessagingSenderId = Environment.GetEnvironmentVariable("FCMConfig_Client_MessagingSenderId");
			string FCMConfig_Client_AppId = Environment.GetEnvironmentVariable("FCMConfig_Client_AppId");

			try
			{
				configModel.MasterDatabaseConfig = new DBServerConfigModel((EnumDBProvider)int.Parse(masterDatabaseDatabaseType),
																		  masterDatabaseAddress,
																		  int.Parse(masterDatabasePort),
																		  masterDatabaseDatabaseName,
																		  masterDatabaseUsername,
																		  masterDatabasePassword);
			}
			catch { }
			try
			{
				configModel.TraceDatabaseConfig = new DBServerConfigModel((EnumDBProvider)int.Parse(traceDatabaseDatabaseType),
																		traceDatabaseAddress,
																		int.Parse(traceDatabasePort),
																		traceDatabaseDatabaseName,
																		traceDatabaseUsername,
																		traceDatabasePassword);
			}
			catch { }
			try
			{
				configModel.RabbitMQConfig = new ServerConfigModel(rabbitMQAddress,
																		int.Parse(rabbitMQPort),
																		rabbitMQUsername,
																		rabbitMQPassword);
			}
			catch { }
			try
			{
				configModel.ElasticSearchConfig = new ServerConfigModel(elasticSearchAddress,
																		int.Parse(elasticSearchPort),
																		elasticSearchUsername,
																		elasticSearchPassword);
			}
			catch { }
			try
			{
				configModel.RedisConfig = new ServerConfigModel(redisAddress,
																		int.Parse(redisPort),
																		redisUsername,
																		redisPassword);
			}
			catch { }
			try
			{
				var isInt = int.TryParse(socketIOPort, out int intPort);
				if (isInt)
					configModel.SocketIOConfig = new ServerConfigModel(socketIOAddress,
							intPort, "", "");
				else
					configModel.SocketIOConfig = new ServerConfigModel(socketIOAddress,
							0, "", "");
			}
			catch { }
			try
			{
				configModel.RemoteConfig = new ServerConfigModel(remoteConfigAddress,
																		int.Parse(remoteConfigPort),
																		"",
																		"");
			}
			catch { }
			try
			{
				configModel.RemoteConfigAPIKey = remoteConfigAPIKey;
			}
			catch { }
			try
			{
				configModel.InfluxDBConfig = new InfluxDBServerConfigModel(influxDBAddress,
																		int.Parse(influxDBPort),
																		influxDBOrganization,
																		influxDBDatabaseName,
																		influxDBUsername,
																		influxDBPassword,
																		influxDBToken);
			}
			catch { }
			try
			{
				configModel.FCMConfig = new FCMConfigModel(new FMCServerModel(FCMConfig_Server_ServerKey,
																			FCMConfig_Server_MessagingSenderId,
																			FCMConfig_Server_DefaultIcon,
																			FCMConfig_Server_DefaultVibrate),
														  new FMCClientModel(FCMConfig_Client_ApiKey,
																			  FCMConfig_Client_AuthDomain,
																			  FCMConfig_Client_ProjectId,
																			  FCMConfig_Client_StorageBucket,
																			  FCMConfig_Client_MessagingSenderId,
																			  FCMConfig_Client_AppId));
			}
			catch { }

			return configModel;
		}
		public static T GetAppSetting<T>(string key)
		{
			try
			{
				var filePath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
				string json = File.ReadAllText(filePath);
				dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);

				var sectionPath = key.Split(":")[0];

				if (key.Split(":").Length >= 2 && !string.IsNullOrEmpty(sectionPath))
				{
					var keyPath = key.Split(":")[1];
					return (T)jsonObj[sectionPath][keyPath];
				}
				else
				{
					return (T)jsonObj[sectionPath];
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public static void AddOrUpdateAppSetting<T>(string key, T value)
		{
			try
			{
				var filePath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
				string json = File.ReadAllText(filePath);
				dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);

				var sectionPath = key.Split(":")[0];

				if (key.Split(":").Length >= 2 && !string.IsNullOrEmpty(sectionPath))
				{
					var keyPath = key.Split(":")[1];
					jsonObj[sectionPath][keyPath] = value;
				}
				else
				{
					jsonObj[sectionPath] = value; // if no sectionpath just set the value
				}

				string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
				File.WriteAllText(filePath, output);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public static async Task<RemoteConfigModel> GetRemoteConfig(string getVersion, bool isRefresh)
		{
			string funcName = "GetRemoteConfig";
			var backupFilePath = Path.Combine("Configs", "Backup", "Lastest.txt");
			var localFilePath = Path.Combine("Configs", "Local.json");

			try
			{
				string jsonStr = FileUtil.ReadFile(localFilePath);
				if (jsonStr == null)
				{
					var dic = new Dictionary<string, string>();
					var xApiKey = CommonConfig.GetConfig().RemoteConfigAPIKey;
					var remoteConfigURL = $"{CommonConfig.GetConfig().RemoteConfig.Address}:{CommonConfig.GetConfig().RemoteConfig.Port}/api/v1/config/getconfig" + $"?v={getVersion}";
					dic.Add("X-Api-Key", xApiKey);
					HttpServiceResponse httpServiceResponse = await HttpService.GetAsync(remoteConfigURL, dic);

					if (httpServiceResponse.StatusCode == System.Net.HttpStatusCode.OK)
					{
						if (string.IsNullOrEmpty(httpServiceResponse.ResponseString))
						{
							if (isRefresh)
							{
								Log.Logger.Information($"{funcName} RemoteConfig not change, v={getVersion}");
								return null;
							}
						}
						ResponseObject ro = JsonConvert.DeserializeObject<ResponseObject>(httpServiceResponse.ResponseString);
						if (ro.Data == null || string.IsNullOrEmpty(ro.Data.ToString()))
						{
							if (isRefresh)
							{
								Log.Logger.Information($"{funcName} RemoteConfig not change, v={getVersion}");
								return null;
							}
							throw new Exception($"{funcName} Remote config data is null, version: " + getVersion);
						}

						string receivedStr = ro.Data.ToString();

						try
						{
							FileUtil.WriteFile(backupFilePath, receivedStr);
						}
						catch (Exception ex)
						{
							Log.Logger.LogMsg(Messages.ErrBaseException, $"Save backup file error, {backupFilePath}");
						}

						jsonStr = DataCipher.DecryptASE(receivedStr);
						var rs = JsonConvert.DeserializeObject<RemoteConfigModel>(jsonStr);

						Log.Logger.LogMsg(Messages.ISuccess_0_1, funcName, rs.GetLogStr());
						return rs;
					}
					else
					{
						throw new BaseException("StatusCode error");
					}
				}
				else
				{
					var rs = JsonConvert.DeserializeObject<RemoteConfigModel>(jsonStr);

					Log.Logger.Information($"{funcName}, [LOADED LOCAL CONFIG FILE]");
					return rs;
				}
			}
			catch (Exception ex)
			{
				var errMessage = Messages.ErrException.SetParameters($"{funcName} error", ex.Message);
				Log.Logger.LogMsg(errMessage);

				if (isRefresh)
				{
					return null;
				}
				string receivedStr = FileUtil.ReadFile(backupFilePath);
				if (receivedStr != null)
				{
					string jsonStr = DataCipher.DecryptASE(receivedStr);
					var rs = JsonConvert.DeserializeObject<RemoteConfigModel>(jsonStr);

					Log.Logger.LogInformation($"{funcName}, [LOADED REMOTE CONFIG LASTEST BACKUP FILE]");
					return rs;
				}
				else
				{
					return null;
				}
			}
		}
	}
}
