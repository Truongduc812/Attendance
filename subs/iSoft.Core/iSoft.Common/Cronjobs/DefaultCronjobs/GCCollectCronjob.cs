using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace iSoft.Common.Cronjobs.DefaultCronjobs
{
  public class GCCollectCronjob : IHostedService, IDisposable
  {
    private int executionCount = 0;
    private readonly ILogger<GCCollectCronjob> _logger;
    private Timer _timer;

    public GCCollectCronjob(ILogger<GCCollectCronjob> logger)
    {
      _logger = logger;
    }

    public Task StartAsync(CancellationToken stoppingToken)
    {
      _logger.LogInformation(nameof(GCCollectCronjob) + " start");

      _timer = new Timer(DoWork, null, TimeSpan.FromSeconds(300), TimeSpan.FromSeconds(300));

      return Task.CompletedTask;
    }

    private async void DoWork(object state)
    {
      try
      {
        var count = Interlocked.Increment(ref executionCount);

        _logger.LogInformation(nameof(GCCollectCronjob) + " is working. Count: {Count}", count);

        GC.Collect();
      }
      catch (Exception ex)
      {
        _logger.LogMsg(Messages.ErrException, ex);
      }
    }

    public Task StopAsync(CancellationToken stoppingToken)
    {
      _logger.LogInformation(nameof(GCCollectCronjob) + " stop");

      _timer?.Change(Timeout.Infinite, 0);

      return Task.CompletedTask;
    }

    public void Dispose()
    {
      _timer?.Dispose();
    }
  }
}