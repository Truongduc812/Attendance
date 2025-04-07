using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Threading.Tasks;
using System.Threading;

namespace SourceBaseBE.CommonFunc.HealthCheck
{
    public sealed class ServiceHealthCheck : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(
          HealthCheckContext context, 
          CancellationToken cancellationToken = new())
        {
            // All is well!
            return HealthCheckResult.Healthy();
        }
    }
}
