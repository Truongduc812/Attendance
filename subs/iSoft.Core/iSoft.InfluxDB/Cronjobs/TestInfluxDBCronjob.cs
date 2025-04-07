using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using iSoft.Common;
using iSoft.Common.Services;
using iSoft.InfluxDB.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace iSoft.InfluxDB.Cronjobs
{
  public class TestInfluxDBCronjob : IHostedService, IDisposable
  {
    private int executionCount = 0;
    private readonly ILogger<TestInfluxDBCronjob> _logger;
    private Timer _timer;
    InfluxDBService _influxDBService;
    private static readonly Random _random = new Random();

    public TestInfluxDBCronjob(ILogger<TestInfluxDBCronjob> logger, InfluxDBService influxDBService)
    {
      _logger = logger;
      _influxDBService = influxDBService;
    }

    public Task StartAsync(CancellationToken stoppingToken)
    {
      _logger.LogInformation(nameof(TestInfluxDBCronjob) + " start");

      _timer = new Timer(DoWork, null, TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(5));

      return Task.CompletedTask;
    }

    private async void DoWork(object state)
    {
      try
      {
        var count = Interlocked.Increment(ref executionCount);

        _logger.LogInformation(nameof(TestInfluxDBCronjob) + " is working. Count: {Count}", count);

        //_influxDBService.Write(write =>
        //{
        //  var point = PointData.Measurement("message_queue")
        //                        .Tag("action", "push")
        //                        .Tag("type", "push")
        //                        .Field("value", _random.Next(1, 10))
        //                        .Timestamp(DateTime.Now, WritePrecision.Ns);

        //  write.WritePoint(point, "request", "i-soft");

        //});

        //_influxDBService.Write(write =>
        //{
        //  var point = PointData.Measurement("message_queue")
        //                        .Tag("action", "receive")
        //                        .Tag("type", "ack")
        //                        .Field("value", _random.Next(10, 20))
        //                        .Timestamp(DateTime.Now, WritePrecision.Ns);

        //  write.WritePoint(point, "request", "i-soft");

        //});

        //_influxDBService.Write(write =>
        //{
        //  var point = PointData.Measurement("message_queue")
        //                        .Tag("action", "receive")
        //                        .Tag("type", "retry")
        //                        .Field("value", _random.Next(20, 30))
        //                        .Timestamp(DateTime.Now, WritePrecision.Ns);

        //  write.WritePoint(point, "request", "i-soft");

        //});
      }
      catch (Exception ex)
      {
        _logger.LogMsg(Messages.ErrException, ex);
      }
    }

    public Task StopAsync(CancellationToken stoppingToken)
    {
      _logger.LogInformation(nameof(TestInfluxDBCronjob) + " stop");

      _timer?.Change(Timeout.Infinite, 0);

      return Task.CompletedTask;
    }

    public void Dispose()
    {
      _timer?.Dispose();
    }
  }
}