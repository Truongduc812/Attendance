using System;
using System.Collections.Generic;
using System.IO;
using iSoft.Common.Models.ConfigModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace iSoft.RemoteConfig.AnnontationFilter
{
    public class ApiKeyAuthenticationFilter : Attribute, IAuthorizationFilter
  {
    //private Dictionary<string, bool> dicApiKey2Flag = new Dictionary<string, bool>()
    //{
    //  { "b65b0f60-cde8-4f95-830c-124e01de2361",true}
    //};
    private Dictionary<string, bool> dicApiKey2Flag = null;

    public void OnAuthorization(AuthorizationFilterContext context)
    {
      if (!context.HttpContext.Request.Headers.TryGetValue(Constant.ConstXApiKeyName, out var apiKey))
      {
        context.Result = new UnauthorizedResult();
        return;
      }

      if (dicApiKey2Flag == null)
      {
        dicApiKey2Flag = new Dictionary<string, bool>();
        ApiKeyConfigModel apiKeyConfig = getApiKeyConfig();

        for (int i = 0; i < apiKeyConfig.ApiKeys.Length; i++)
        {
          var apiKeyModel = apiKeyConfig.ApiKeys[i];
          if (apiKeyModel.IsEnable != false)
          {
            dicApiKey2Flag.Add(apiKeyModel.ApiKey, true);
          }
        }
      }

      if (dicApiKey2Flag == null || !dicApiKey2Flag.ContainsKey(apiKey))
      {
        context.Result = new UnauthorizedResult();
        return;
      }
    }

    private ApiKeyConfigModel getApiKeyConfig()
    {
      StreamReader sr = null;
      sr = new StreamReader(Constant.ConstApiKeyJsonFilePath);
      string apiKeyJsonData = sr.ReadToEnd();
      sr.Close();

      var apiKeyConfig = JsonConvert.DeserializeObject<ApiKeyConfigModel>(apiKeyJsonData);
      return apiKeyConfig;
    }
  }
}
