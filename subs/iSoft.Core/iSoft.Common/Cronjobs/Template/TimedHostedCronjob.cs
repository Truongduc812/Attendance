using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace iSoft.Common.Cronjobs.Template
{
  public class TimedHostedCronjob : IHostedService, IDisposable
  {
    private int executionCount = 0;
    private readonly ILogger<TimedHostedCronjob> _logger;
    private Timer _timer;

    public TimedHostedCronjob(ILogger<TimedHostedCronjob> logger)
    {
      _logger = logger;
    }

    public Task StartAsync(CancellationToken stoppingToken)
    {
      _logger.LogInformation("Timed Hosted Service start.");

      _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

      return Task.CompletedTask;
    }

    private void DoWork(object state)
    {
      try
      {
        var count = Interlocked.Increment(ref executionCount);

        _logger.LogInformation("Timed Hosted Service is working. Count: {Count}", count);
      }
      catch (Exception ex)
      {
        _logger.LogMsg(Messages.ErrException, ex);
      }
    }

    public Task StopAsync(CancellationToken stoppingToken)
    {
      _logger.LogInformation("Timed Hosted Service is stopping.");

      _timer?.Change(Timeout.Infinite, 0);

      return Task.CompletedTask;
    }

    public void Dispose()
    {
      _timer?.Dispose();
    }
  }
}