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

        public ResilienceExecutor()
        {
            _policy = PollyPolicies.RetryPolicy
                .WrapAsync(PollyPolicies.CircuitBreakerPolicy);
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
