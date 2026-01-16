using Microsoft.Extensions.Logging;
using Polly;

namespace CityPedidos.Infrastructure.Resilience
{
    public interface IResilienceExecutor
    {
        Task ExecuteAsync(Func<Task> action);
        Task<T> ExecuteAsync<T>(Func<Task<T>> action);
    }

    public class ResilienceExecutor : IResilienceExecutor
    {
        private readonly IAsyncPolicy _policy;
        private readonly ILogger<ResilienceExecutor> _logger;

        public ResilienceExecutor(ILogger<ResilienceExecutor> logger)
        {
            _logger = logger;

            _policy = PollyPolicies
                .CreateRetryPolicy(_logger)
                .WrapAsync(
                    PollyPolicies.CreateCircuitBreakerPolicy(_logger)
                );
        }

        public async Task ExecuteAsync(Func<Task> action)
        {
            await _policy.ExecuteAsync(action);
        }

        public async Task<T> ExecuteAsync<T>(Func<Task<T>> action)
        {
            return await _policy.ExecuteAsync(action);
        }
    }
}
