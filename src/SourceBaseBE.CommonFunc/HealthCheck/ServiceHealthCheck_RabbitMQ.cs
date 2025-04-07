using Microsoft.Extensions.Diagnostics.HealthChecks;
using RabbitMQ.Client;
using iSoft.RabbitMQ.Services;
using iSoft.Common.ConfigsNS;

namespace SourceBaseBE.CommonFunc.HealthCheck
{
  public sealed class ServiceHealthCheck_RabbitMQ : IHealthCheck
  {
    public async Task<HealthCheckResult> CheckHealthAsync(
      HealthCheckContext context,
      CancellationToken cancellationToken = new())
    {
      try
      {
        string connStr = RabbitMQService.GetConnectionString(CommonConfig.GetConfig().RabbitMQConfig);
        var factory = new ConnectionFactory()
        {
          Uri = new Uri(connStr)
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        return HealthCheckResult.Healthy();
      }
      catch (Exception ex)
      {
        return HealthCheckResult.Unhealthy();
      }

    }
  }
}
