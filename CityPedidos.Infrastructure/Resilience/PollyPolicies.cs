using Polly;
using Polly.Retry;
using Polly.CircuitBreaker;

namespace CityPedidos.Infrastructure.Resilience
{
    public static class PollyPolicies
    {
        public static AsyncRetryPolicy RetryPolicy =>
            Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(
                    3,
                    retry => TimeSpan.FromSeconds(Math.Pow(2, retry))
                );

        public static AsyncCircuitBreakerPolicy CircuitBreakerPolicy =>
            Policy
                .Handle<Exception>()
                .CircuitBreakerAsync(
                    5,
                    TimeSpan.FromSeconds(30)
                );
    }
}
