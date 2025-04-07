using Microsoft.Extensions.Diagnostics.HealthChecks;
using iSoft.Common.ConfigsNS;
using SocketIOClient;
using iSoft.SocketIOClientNS.Services;

namespace SourceBaseBE.CommonFunc.HealthCheck
{
  public sealed class ServiceHealthCheck_SocketIO : IHealthCheck
  {
    public async Task<HealthCheckResult> CheckHealthAsync(
    HealthCheckContext context,
    CancellationToken cancellationToken = new())
    {
      try
      {
        var socketConfig = CommonConfig.GetConfig().SocketIOConfig;
        string address = string.Format("{0}:{1}", socketConfig.Address, socketConfig.Port);

        var tcs = new TaskCompletionSource<HealthCheckResult>();

        var client = SocketIOClientService.NewConnection(address, (SocketIO? client) =>
        {
          client.OnConnected += async (sender, e) =>
          {
            tcs.TrySetResult(HealthCheckResult.Healthy());
          };

          client.OnDisconnected += async (sender, e) =>
          {
            tcs.TrySetResult(HealthCheckResult.Unhealthy());
          };

          client.OnError += async (sender, e) =>
          {
            tcs.TrySetResult(HealthCheckResult.Unhealthy());
          };
        });

        if (client != null && client.Connected)
        {
          return HealthCheckResult.Healthy();
        }

        // Wait for the task to complete or timeout
        var result = await Task.WhenAny(tcs.Task, Task.Delay(TimeSpan.FromSeconds(10), cancellationToken));
        return result == tcs.Task ? tcs.Task.Result : HealthCheckResult.Unhealthy();
      }
      catch (Exception ex)
      {
        return HealthCheckResult.Unhealthy();
      }
    }

  }
}
