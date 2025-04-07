using iSoft.Common;
using iSoft.Common.ConfigsNS;
using iSoft.Common.Utils;
using iSoft.ConnectionCommon.MessageQueueNS;
using iSoft.RabbitMQ;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SourceBaseBE.VirtualDeviceService.Services
{
  public class VirtualDeviceHostedService : IHostedService, IDisposable
  {
    private int executionCount = 0;
    private readonly ILogger<VirtualDeviceHostedService> _logger;
    PushDataService _pushDataService;
    PushRealtimeService _pushRealtimeService;

    public VirtualDeviceHostedService(ILogger<VirtualDeviceHostedService> logger, PushDataService pushDataService, PushRealtimeService pushRealtimeService)
    {
      _logger = logger;
      _pushDataService = pushDataService;
      _pushRealtimeService = pushRealtimeService;
    }

    public Task StartAsync(CancellationToken stoppingToken)
    {
      _logger.LogInformation(nameof(VirtualDeviceHostedService) + " start.");

      //MessageQueue.Init(MessageQueueConfig.GetQueueConfig(), CommonConfig.GetConfig().RabbitMQConfig).Wait();
      //_pushRealtimeService.Init(CommonConfig.GetConfig().SocketIOConfig);

      DoWork(null);

      return Task.CompletedTask;
    }

    private async void DoWork(object state)
    {
      try
      {
        while (true)
        {
          for (int i = 0; i < 1; i++)
          {
            PushMessageToRabbitAndSocketIO();
          }

          Thread.Sleep(10000);
        }
      }
      catch (Exception ex)
      {
        _logger.LogMsg(Messages.ErrException, ex);
      }
    }

    public async void PushMessageToRabbitAndSocketIO()
    {
      var count = Interlocked.Increment(ref executionCount);

      _logger.LogInformation(nameof(VirtualDeviceHostedService) + " is working. Count: {Count}", count);

      var listMessage = _pushDataService.CreateTestMessage();

      try
      {
        for (int i = 0; i < listMessage.Count; i++)
        {
          listMessage[i].ExecuteAt = DateTime.Now;
          await _pushDataService.PushMessageAsyncTest(ExchangeName.SourceBaseBEEnvEx, listMessage[i]);
          Thread.Sleep(NumberUtil.GetRandomInt(0, 200));
        }
      }
      catch (Exception ex)
      {
        _logger.LogMsg(Messages.ErrException, ex);
      }

      //try
      //{
      //  await _pushRealtimeService.SendMessage(message);
      //}
      //catch (Exception ex)
      //{
      //_logger.LogMsg(Messages.ErrException, ex);
      //}
    }

    public Task StopAsync(CancellationToken stoppingToken)
    {
      _logger.LogInformation(nameof(VirtualDeviceHostedService) + " is stopping.");

      return Task.CompletedTask;
    }

    public void Dispose()
    {
      //throw new NotImplementedException();
    }
  }
}