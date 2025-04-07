using iSoft.Common.ConfigsNS;
using iSoft.Common.Exceptions;
using iSoft.Common.ExtensionMethods;
using iSoft.Common.Utils;
using iSoft.Common;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using iSoft.RabbitMQ.Payload;
using System.Threading;
using SourceBaseBE.CommonFunc.DataService;
using SourceBaseBE.CommonFunc.EnvConfigData;
using iSoft.Common.Models.ConfigModel.Subs;
using iSoft.Redis.Services;
using iSoft.Common.Enums;
using iSoft.Common.Services;
using iSoft.InfluxDB.Services;

namespace iSoft.RabbitMQ.Services
{
  public class SearchDataConsumerHostedService : ConsumerHostedService
  {
    internal ServerConfigModel _redisConfig;
    private ImportDataFactory importDataFactory = null;


    public SearchDataConsumerHostedService(
      ILogger<ConsumerHostedService> logger, 
      RabbitMQService rabbitMQService, 
      string queueName)
      : base(logger, rabbitMQService, queueName)
    {
      initElasticSearch();
    }

    private void initElasticSearch()
    {
      try
      {
        CachedFunc.SetRedisConfig(CommonConfig.GetConfig().RedisConfig);

        var envConfigData = EnvConfigData.Ins;
        importDataFactory = new ImportDataFactory(null, null, envConfigData.GetListEnvConfigModel(), CommonConfig.GetConfig().ElasticSearchConfig);
      }
      catch (Exception ex)
      {
        _logger.LogMsg(Messages.ErrException, ex);
      }
    }

    public override async Task handleMessage(DeliveryObj deliveryMessage)
    {
      string errMessage = "";
      string dataJson = "";
      string funcName = "handleMessage";
      DevicePayloadMessage? message = null;
      try
      {
        DateTime startTime = DateTime.Now;
        message = deliveryMessage.GetData<DevicePayloadMessage>(ref dataJson, ref errMessage);

        if (message == null)
        {
          this._logger.LogMsg(Messages.ErrInputInvalid_0_1, funcName, $"err: message = null, {errMessage}, json: {dataJson}");
          this.RemoveErrorMessage(deliveryMessage);
          return;
        }
        if (message.Data == null || message.Data.Count <= 0 || message.ConnectionId == null || message.MessageId == null)
        {
          this._logger.LogMsg(Messages.ErrInputInvalid_0_1, funcName, message.MessageId + ", " + $"json: {dataJson}");
          this.RemoveErrorMessage(deliveryMessage);
          return;
        }

        if (!cached.ContainsKey($"log_es_message_seconds_10m_all"))
        {
          cached.AddToCache($"log_es_message_seconds_10m_all", true, 60 * 10);
          this._logger.LogMsg(Messages.IBegin_0_1, $"[searchdata10m] All", message.MessageId + ", message: " + message.ToJson());
        }

        await importDataFactory.ImportESData(message, "_main");

        this.RemoveSuccessMessage(deliveryMessage);
        this._logger.LogMsg(Messages.ISuccess_0_1, funcName, message.MessageId + ", " + DateTimeUtil.GetHumanStr(DateTime.Now - startTime));
        return;
      }
      catch (JsonReaderException ex)
      {
        this._logger.LogMsg(Messages.ErrBaseException.SetParameters($"json: {dataJson}", ex));
        this.RemoveErrorMessage(deliveryMessage);
      }
      catch (RetryException ex)
      {
        this._logger.LogWarning($"RetryException, {message.MessageId}, " + ex.Message);
        this.RetryMessage(deliveryMessage);
      }
      catch (DBException ex)
      {
        this._logger.LogMsg(Messages.ErrDBException.SetParameters($"json: {dataJson}", ex));
        this.RetryMessage(deliveryMessage);
        //throw ex;
      }
      catch (CriticalException ex)
      {
        this._logger.LogMsg(Messages.ErrDBException.SetParameters($"json: {dataJson}", ex));
        this.RetryMessage(deliveryMessage);
        //throw ex;
      }
      catch (BaseException ex)
      {
        this._logger.LogMsg(Messages.ErrBaseException.SetParameters($"json: {dataJson}", ex));
        this.RetryMessage(deliveryMessage);
        //throw ex;
      }
      catch (Exception ex)
      {
        this._logger.LogMsg(Messages.ErrException.SetParameters($"json: {dataJson}", ex));
        this.RetryMessage(deliveryMessage);
        //throw ex;
      }

    }

  }
}
