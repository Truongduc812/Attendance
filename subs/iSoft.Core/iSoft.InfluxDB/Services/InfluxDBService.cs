using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using iSoft.Common.Models.ConfigModel.Subs;
using iSoft.Common.Utils;
using Serilog;

namespace iSoft.InfluxDB.Services
{
  public class InfluxDBService
  {
    private InfluxDBServerConfigModel _config;
    public ILogger _logger = Serilog.Log.Logger;

    public InfluxDBService(InfluxDBServerConfigModel config)
    {
      _config = config;
    }
    public static string GetConnectionString(InfluxDBServerConfigModel config)
    {
      return $"{config.GetHostName()}/?org=i-soft&bucket=request&token={config.Token}";
    }

    public void Write(Action<WriteApi> action)
    {
      using var client = InfluxDBClientFactory.Create(_config.GetHostName(), _config.Token.ToCharArray());
      using var write = client.GetWriteApi();
      action(write);
    }

    public async Task<T> QueryAsync<T>(Func<QueryApi, Task<T>> action)
    {
      using var client = InfluxDBClientFactory.Create(_config.GetHostName(), _config.Token.ToCharArray());
      var query = client.GetQueryApi();
      return await action(query);
    }

    //public async Task TrackMetricsPushMessage(string exchangeNameOrQueueName, bool isOk = true)
    //{
    //  try
    //  {
    //    this.Write(write =>
    //    {
    //      var point = PointData.Measurement("message_queue_" + exchangeNameOrQueueName.RemoveSpecialChar().ToLower())
    //                            .Tag("action", "push")
    //                            .Tag("type", isOk ? "push" : "error")
    //                            .Field("value", 0)
    //                            .Timestamp(DateTime.Now, WritePrecision.Ns);
    //      write.WritePoint(point, "request", "i-soft");

    //    });
    //  }
    //  catch (Exception ex)
    //  {
    //    _logger.LogError("TrackMetricsPushMessage error, " + ex.Message);
    //  }
    //}

    //public async Task TrackMetricsReceiveMessage(string queueName, bool isAck, bool isRetry = false, int retryCount = -1)
    //{
    //  try
    //  {
    //    this.Write(write =>
    //    {
    //      var point = PointData.Measurement("message_queue_" + queueName.RemoveSpecialChar().ToLower())
    //                            .Tag("action", "receive")
    //                            .Tag("type", isAck ? "ack" : (isRetry ? "retry" : "error"))
    //                            .Field("value", retryCount)
    //                            .Timestamp(DateTime.Now, WritePrecision.Ns);

    //      write.WritePoint(point, "request", "i-soft");

    //    });
    //  }
    //  catch (Exception ex)
    //  {
    //    _logger.LogError("TrackMetricsReceiveMessage error, " + ex.Message);
    //  }
    //}
  }
}
