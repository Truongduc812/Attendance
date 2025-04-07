using iSoft.Common.ConfigsNS;
using iSoft.Common.MetricsNS;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace iSoft.Common.Cronjobs.DefaultCronjobs
{
  public class ResetMetricsCronjob : IHostedService, IDisposable
  {
    private int executionCount = 0;
    private readonly ILogger<ResetMetricsCronjob> _logger;
    private Timer _timer;

    public ResetMetricsCronjob(ILogger<ResetMetricsCronjob> logger)
    {
      _logger = logger;
    }

    public Task StartAsync(CancellationToken stoppingToken)
    {
      _logger.LogInformation(nameof(ResetMetricsCronjob) + " start");

      _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(60));

      return Task.CompletedTask;
    }

    private async void DoWork(object state)
    {
      try
      {
        var count = Interlocked.Increment(ref executionCount);

        _logger.LogInformation(nameof(ResetMetricsCronjob) + " is working. Count: {Count}", count);

        GaugeMetrics.ResetRateMetrics();
      }
      catch (Exception ex)
      {
        _logger.LogMsg(Messages.ErrException, ex);
      }
    }

    public Task StopAsync(CancellationToken stoppingToken)
    {
      _logger.LogInformation(nameof(ResetMetricsCronjob) + " stop");

      _timer?.Change(Timeout.Infinite, 0);

      return Task.CompletedTask;
    }

    public void Dispose()
    {
      _timer?.Dispose();
    }
  }
}