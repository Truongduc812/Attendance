using iSoft.Common.Enums;
using iSoft.Common.Exceptions;
using iSoft.Common.Services;
using iSoft.Common.Utils;
using System.Text;
using static iSoft.Common.ConstCommon;
using iSoft.ElasticSearch.Models;
using Newtonsoft.Json;
using iSoft.Common.ExtensionMethods;
using iSoft.Common.Models.ConfigModel.Subs;
using iSoft.ElasticSearch.ESScriptPattern;
using iSoft.Common;
using static iSoft.Common.Messages;
using iSoft.Common.Cached;
using Serilog.Core;

namespace iSoft.ElasticSearch.Services
{
  public class ESRestfullAPIService
  {
    private MemCached cached = new MemCached(60);
    ServerConfigModel esConfig = null;
    public ESRestfullAPIService(ServerConfigModel esConfig)
    {
      this.esConfig = esConfig;
    }
    public async Task CreateStreamIndexAsync(List<IESFieldName> listEnvKey2Config, string esIndexName, string esPatternSearch)
    {
      if (esConfig == null || listEnvKey2Config.Count <= 0)
      {
        return;
      }
      StringBuilder sb = new StringBuilder();
      foreach (var fieldConfig in listEnvKey2Config)
      {
        sb.Append(getESFieldScript(fieldConfig.GetESFieldName2(), fieldConfig.GetESDataType()));
      }
      string indexName = esIndexName; //listEnvKey2Config.FirstOrDefault().GetESIndexName();
      string json = ESScript.ESScriptCreateIndexTempate;
      json = json.Replace("@esPatternSearch", esPatternSearch)
                 .Replace("@fields", StringUtil.RemoveLastChar(sb.ToString()));

      var dicHeader = new Dictionary<string, string>();
      //dicHeader.Add("kbn-version", "7.17.5");
      //dicHeader.Add("Content-Type", "application/json");

      string base64Credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{esConfig.Username}:{esConfig.Password}"));
      dicHeader.Add("Authorization", $"Basic {base64Credentials}");

      HttpServiceResponse httpServiceResponse = await HttpService.PostAsync(
        $"{esConfig.Address}:{esConfig.Port}/_index_template/{indexName}",
        HttpMethod.Put, dicHeader, null, json);
      if (httpServiceResponse.StatusCode != System.Net.HttpStatusCode.OK)
      {
        throw new CriticalException($"[CreateStreamIndexAsync] Create ES Index Error, indexKey: {indexName}, response: {httpServiceResponse.ResponseString}");
      }
    }

    public async Task PushStreamDataAsync(long ConnectionId,
                                          DateTime ExecuteAt,
                                          string MessageId,
                                          int ShiftId,
                                          Dictionary<string, IESFieldName> dicEnvKey2Config,
                                          Dictionary<string, object> dicEnvKey2Value,
                                          string esIndexName)
    {
      Dictionary<string, object> dicData = new Dictionary<string, object>();
      dicData.Add("@timestamp", DateTimeUtil.GetDateTimeStr(DateTime.Now, ConstDateTimeFormat.YYYYMMDDTHHMMSSFFF_07_00));
      dicData.Add("ConnectionId".ToLower(), ConnectionId);
      //dicData.Add("ExecuteAt".ToLower(), DateTimeUtil.GetDateTimeStr(DateTimeUtil.GetLocalDateTime(ExecuteAt), "yyyy-MM-ddTHH:mm:ss.fffZ"));
      dicData.Add("ExecuteAt".ToLower(), DateTimeUtil.GetDateTimeStr(ExecuteAt, ConstDateTimeFormat.YYYYMMDDTHHMMSSFFF_07_00));
      dicData.Add("MessageId".ToLower(), MessageId);
      dicData.Add("ShiftId".ToLower(), ShiftId);
      foreach (var keyVal in dicEnvKey2Config)
      {
        if (dicEnvKey2Value.ContainsKey(keyVal.Key))
        {
          dicData.Add(keyVal.Value.GetESFieldName2(), keyVal.Value.GetValueFromObject(dicEnvKey2Value[keyVal.Key]));
        }
        else
        {
          //dicData.Add(keyVal.Value.GetESFieldName(), null);
        }
      }
      string json = dicData.ToJson();

      string indexName = esIndexName; //dicEnvKey2Config.FirstOrDefault().Value.GetESIndexName();
      var dicHeader = new Dictionary<string, string>();
      //dicHeader.Add("kbn-version", "7.17.5");
      //dicHeader.Add("Content-Type", "application/json");

      string base64Credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{esConfig.Username}:{esConfig.Password}"));
      dicHeader.Add("Authorization", $"Basic {base64Credentials}");

      HttpServiceResponse httpServiceResponse = await HttpService.PostAsync(
        $"{esConfig.Address}:{esConfig.Port}/{indexName}/_doc",
        HttpMethod.Post, dicHeader, null, json);
      if (httpServiceResponse.StatusCode != System.Net.HttpStatusCode.Created)
      {
        throw new CriticalException($"[PushStreamData] Push ES Index Error, indexKey: {indexName}, response: {httpServiceResponse.ResponseString}");
      }
    }

    static object lockObj1 = new object();
    static DateTime lastErrorTime = DateTime.MinValue;
    public async Task<bool> IsExistsMessageIdAsync(string esPatternSearch, string? MessageId)
    {
      //const string keyCachedException = $"IsExistsMessageIdAsync";
      try
      {
        lock (lockObj1)
        {
          if (DateTimeUtil.CompareDateTime(lastErrorTime, DateTime.Now) < 15)
          {
            throw new RetryException($"Call API err just now, IsExistsMessageIdAsync, lastErrorTime = {DateTimeUtil.GetDateTimeStr(lastErrorTime)}");
          }
        }

        if (esConfig == null)
        {
          throw new BaseException("esConfig == null");
        }
        if (MessageId == null)
        {
          throw new BaseException("MessageId == null");
        }
        string json = ESScript.ESScriptIsExistsMessageIdTemplate;
        json = json.Replace("@messageId", MessageId);

        var dicHeader = new Dictionary<string, string>();
        //dicHeader.Add("kbn-version", "7.17.5");
        //dicHeader.Add("Content-Type", "application/json");

        string base64Credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{esConfig.Username}:{esConfig.Password}"));
        dicHeader.Add("Authorization", $"Basic {base64Credentials}");

        HttpServiceResponse httpServiceResponse = await HttpService.PostAsync(
          $"{esConfig.Address}:{esConfig.Port}/{esPatternSearch}/_search",
          HttpMethod.Get,
          dicHeader,
          null,
          json);
        if (httpServiceResponse.StatusCode != System.Net.HttpStatusCode.OK)
        {
          throw new CriticalException($"[IsExistsMessageIdAsync] Error, esPatternSearch: {esPatternSearch}, response: {httpServiceResponse.ResponseString}");
        }

        SearchResult? searchResult = JsonConvert.DeserializeObject<SearchResult>(httpServiceResponse.ResponseString);
        if (searchResult != null
          && searchResult.Hits != null
          && searchResult.Hits.Total != null
          && searchResult.Hits.Total.Value != null)
        {
          if (searchResult.Hits.Total.Value >= 1)
          {
            return true;
          }
        }
      }
      catch (BaseException ex)
      {
        throw ex;
      }
      catch (HttpRequestException ex)
      {
        lock (lockObj1)
        {
          lastErrorTime = DateTime.Now;
        }
        throw ex;
      }
      catch (Exception ex)
      {
        lock (lockObj1)
        {
          lastErrorTime = DateTime.Now;
        }
        throw ex;
      }
      return false;
    }

    private string getESFieldScript(string fieldName, EnumDataType dataType)
    {
      switch (dataType)
      {
        case EnumDataType.String50:
        case EnumDataType.String255:
        case EnumDataType.String:
          return @"                
              ""@fieldName"": {
                  ""type"": ""keyword"",
                  ""index"": true,
                  ""index_options"": ""docs"",
                  ""eager_global_ordinals"": false,
                  ""norms"": false,
                  ""split_queries_on_whitespace"": false,
                  ""doc_values"": true,
                  ""store"": false
              },".Replace("@fieldName", fieldName);
        case EnumDataType.Bool:
          return @"                
                ""@fieldName"": {
                    ""type"": ""boolean""
                },".Replace("@fieldName", fieldName);
        case EnumDataType.Short:
          return @"                
                ""@fieldName"": {
                    ""type"": ""short"",
                    ""ignore_malformed"": true,
                    ""coerce"": true
                },".Replace("@fieldName", fieldName);
        case EnumDataType.Int:
          return @"                     
                ""@fieldName"": {
                    ""type"": ""integer"",
                    ""ignore_malformed"": true,
                    ""coerce"": true
                },".Replace("@fieldName", fieldName);
        case EnumDataType.Long:
          return @"                     
                ""@fieldName"": {
                    ""type"": ""long"",
                    ""ignore_malformed"": true,
                    ""coerce"": true
                },".Replace("@fieldName", fieldName);
        case EnumDataType.Double:
          return @"                     
                ""@fieldName"": {
                    ""type"": ""double"",
                    ""ignore_malformed"": true,
                    ""coerce"": true
                },".Replace("@fieldName", fieldName);
      }
      return "";
    }
  }
}
