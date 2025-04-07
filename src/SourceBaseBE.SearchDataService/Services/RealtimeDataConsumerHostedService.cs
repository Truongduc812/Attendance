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
using RabbitMQ.Client.Events;
using iSoft.Common.Services;
using iSoft.InfluxDB.Services;

namespace iSoft.RabbitMQ.Services
{
  public class RealtimeDataConsumerHostedService : ConsumerHostedService
  {
    public RealtimeDataConsumerHostedService(
      ILogger<ConsumerHostedService> logger,
      RabbitMQService rabbitMQService,
      string queueName)
      : base(logger, rabbitMQService, queueName)
    {

    }
    public override async Task Init(CancellationToken stoppingToken)
    {
      while (true)
      {
        try
        {
          if (RabbitMQConnectionStatus == EnumConnectionStatus.Connected)
          {
            //await ReadMessages(handleMessage);
            break;
          }
          RabbitMQConnectionStatus = initRabbitMQ(_rabbitMQService, _queueProperties.QueueName);
        }
        catch (Exception ex)
        {
          _logger.LogMsg(Messages.ErrException, ex);
        }
        Thread.Sleep(10000);
      }
    }
    public override async Task ReadMessages(Func<DeliveryObj, Task> handleMessageFunction)
    {
      await Task.CompletedTask;
    }
  }
}
