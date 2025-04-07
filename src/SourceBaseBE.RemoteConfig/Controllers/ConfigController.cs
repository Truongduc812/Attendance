using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using iSoft.RemoteConfig.AnnontationFilter;
using System.IO;
using Newtonsoft.Json;
using System;
using Microsoft.Extensions.Logging;
using iSoft.Common.Enums;
using iSoft.Common;
using Microsoft.Extensions.Options;
using DotNetEnv;
using iSoft.Common.Utils;
using System.Collections.Generic;
using iSoft.Common.Security;
using iSoft.Common.Enums.DBProvider;
using iSoft.Common.ExtensionMethods;
using iSoft.Common.Models.ConfigModel;

namespace iSoft.RemoteConfig.Controllers
{
    [ApiController]
  [Route("api/v1/[controller]")]
  public class ConfigController : ControllerBase
  {
    private readonly ILogger<ConfigController> logger;
    public static Dictionary<string, int> dicAPIKey2Version = new Dictionary<string, int>();
    private static ApiKeyConfigModel apiKeyConfigModel = null;
    public ConfigController(ILoggerFactory loggerFactory)
    {
      this.logger = loggerFactory.CreateLogger<ConfigController>();
    }

    [HttpGet("getconfig")]
    //[ApiKeyAuthenticationFilter]
    public IActionResult GetConfig([FromQuery] string v)
    {
      const string funcName = "GetConfig";
      StreamReader sr = null;
      try
      {

        var request = Request;
        var apiKey = request.Headers[Constant.ConstXApiKeyName].ToString();

        //if (dicAPIKey2Version.ContainsKey(apiKey) && dicAPIKey2Version[apiKey].ToString().Trim() == v)
        //{
        //  logger.LogInformation(funcName + " " + apiKey + ", version config not change, v=" + v);
        //  return this.ResponseOk("");
        //}

        sr = new StreamReader(Constant.ConstApiKeyJsonFilePath);
        string apiKeyJsonData = sr.ReadToEnd();
        sr.Close();

        var apiKeyConfig = JsonConvert.DeserializeObject<ApiKeyConfigModel>(apiKeyJsonData);
        apiKeyConfigModel = apiKeyConfig;

        for (int i = 0; i < apiKeyConfig.ApiKeys.Length; i++)
        {
          var apiKeyModel = apiKeyConfig.ApiKeys[i];

          if (apiKeyModel.ApiKey == apiKey)
          {
            sr = new StreamReader(apiKeyModel.JsonFilePath);
            string jsonData = sr.ReadToEnd();
            sr.Close();
            var config = JsonConvert.DeserializeObject<RemoteConfigModel>(jsonData);

            if (!dicAPIKey2Version.ContainsKey(apiKey))
            {
              dicAPIKey2Version.Add(apiKey, int.Parse(config.Version));
            }
            else
            {
              dicAPIKey2Version[apiKey] = int.Parse(config.Version);
            }

            logger.LogMsg(Messages.ISuccess_0_1, funcName, apiKeyModel.Name, config.GetLogStr());
            return this.ResponseOk(DataCipher.EncryptASE(config.ToJson()));
          }
        }
        this.logger.LogError("apiKey not found, apiKey: {0}", apiKey);
        return NotFound("apiKey not found");

      }
      catch (Exception ex)
      {
        this.logger.LogMsg(Messages.ErrBaseException, "Get config error", ex);
        //this.logger.LogError("Get config error, ex: {0}", ex.Message);
        try
        {
          if (sr != null)
          {
            sr.Close();
          }
        }
        catch { }

        return BadRequest(ex.Message);
      }
    }
  }
}
