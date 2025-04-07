using iSoft.Common.ConfigsNS;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace iSoft.Common.Cronjobs.DefaultCronjobs
{
  public class GetRemoteConfigCronjob : IHostedService, IDisposable
  {
    private int executionCount = 0;
    private readonly ILogger<GetRemoteConfigCronjob> _logger;
    private Timer _timer;

    public GetRemoteConfigCronjob(ILogger<GetRemoteConfigCronjob> logger)
    {
      _logger = logger;
    }

    public Task StartAsync(CancellationToken stoppingToken)
    {
      _logger.LogInformation(nameof(GetRemoteConfigCronjob) + " start");

      _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));

      return Task.CompletedTask;
    }

    private async void DoWork(object state)
    {
      try
      {
        var count = Interlocked.Increment(ref executionCount);

        _logger.LogInformation(nameof(GetRemoteConfigCronjob) + " is working. Count: {Count}", count);

        await RemoteConfigs.RefreshConfig();
      }
      catch (Exception ex)
      {
        _logger.LogMsg(Messages.ErrException, ex);
      }
    }

    public Task StopAsync(CancellationToken stoppingToken)
    {
      _logger.LogInformation(nameof(GetRemoteConfigCronjob) + " stop");

      _timer?.Change(Timeout.Infinite, 0);

      return Task.CompletedTask;
    }

    public void Dispose()
    {
      _timer?.Dispose();
    }
  }
}