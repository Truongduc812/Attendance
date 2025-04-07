using iSoft.Common;
using iSoft.Common.ConfigsNS;
using iSoft.Common.Enums;
using iSoft.Redis.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog.Core;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SourceBaseBE.TraceDataServiceNS.Cronjobs
{
  public class CheckConnectRedisJob : IHostedService, IDisposable
  {
    private int executionCount = 0;
    private readonly ILogger<CheckConnectRedisJob> _logger;
    private Timer _timer;

    public CheckConnectRedisJob(ILogger<CheckConnectRedisJob> logger)
    {
      _logger = logger;
    }

    public Task StartAsync(CancellationToken stoppingToken)
    {
      _logger.LogInformation(nameof(CheckConnectRedisJob) + " start.");

      _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

      return Task.CompletedTask;
    }

    private async void DoWork(object state)
    {
      try
      {
        var count = Interlocked.Increment(ref executionCount);

        _logger.LogInformation(nameof(CheckConnectRedisJob) + " is working. Count: {Count}", count);

        await CheckConnectRedis(null);
      }
      catch (Exception ex)
      {
        _logger.LogMsg(Messages.ErrException, ex);
      }
    }
    private async Task CheckConnectRedis(object state)
    {
      var connectStatus = CachedFunc.TestConnection();
      if (connectStatus == EnumConnectionStatus.Connected)
      {
        _logger.LogMsg(Messages.ISuccess_0_1, nameof(CheckConnectRedis), connectStatus.ToString());
      }
      else if (connectStatus == EnumConnectionStatus.Error)
      {
        _logger.LogMsg(Messages.ErrBaseException, nameof(CheckConnectRedis), connectStatus.ToString(), CommonConfig.GetConfig().RedisConfig.GetLogStr());
      }
    }

    public Task StopAsync(CancellationToken stoppingToken)
    {
      _logger.LogInformation(nameof(CheckConnectRedisJob) + " is stopping.");

      _timer?.Change(Timeout.Infinite, 0);

      return Task.CompletedTask;
    }

    public void Dispose()
    {
      _timer?.Dispose();
    }
  }
}