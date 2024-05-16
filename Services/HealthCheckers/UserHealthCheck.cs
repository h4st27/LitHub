using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Libra.Services.HealthChecker
{
    public class UserHealthCheck : IHealthCheck
    {
        private readonly string _param;

        public UserHealthCheck(string param)
        {
            _param = param;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(HealthCheckResult.Healthy(description: _param));
        }

        public static Task WriteResponse(HttpContext context, HealthReport healthReport)
        {
            var hEntry = healthReport.Entries.Values.FirstOrDefault();
            return context.Response.WriteAsync($"{hEntry.Status} => {hEntry.Description}. {context.Request.Headers["Authorization"]}");
        }
    }
}
