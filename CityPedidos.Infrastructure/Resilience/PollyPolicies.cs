using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using Polly.CircuitBreaker;

namespace CityPedidos.Infrastructure.Resilience
{
    public static class PollyPolicies
    {
        public static AsyncRetryPolicy CreateRetryPolicy(ILogger logger) =>
            Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(
                    3,
                    retry => TimeSpan.FromSeconds(Math.Pow(2, retry)),
                    (exception, timeSpan, retryCount, context) =>
                    {
                        logger.LogWarning(
                            exception,
                            "Retry {RetryCount} after {Delay}s",
                            retryCount,
                            timeSpan.TotalSeconds);
                    });

        public static AsyncCircuitBreakerPolicy CreateCircuitBreakerPolicy(ILogger logger) =>
            Policy
                .Handle<Exception>()
                .CircuitBreakerAsync(
                    5,
                    TimeSpan.FromSeconds(30),
                    onBreak: (ex, breakDelay) =>
                    {
                        logger.LogError(
                            ex,
                            "Circuit breaker OPEN for {Delay}s",
                            breakDelay.TotalSeconds);
                    },
                    onReset: () =>
                    {
                        logger.LogInformation("Circuit breaker RESET");
                    });
    }
}
