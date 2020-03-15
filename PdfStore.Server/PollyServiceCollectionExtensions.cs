using System;
using System.Drawing;
using DocumentStore.Application.Validation;
using DocumentStore.Infrastructure;
using DocumentStore.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.CircuitBreaker;
using Polly.Wrap;

namespace DocumentStore.Application
{
    public static class PollyServiceCollectionExtensions
    {
        public static IServiceCollection AddPolly(this IServiceCollection services)
        {
            // Define our waitAndRetry policy: keep retrying with 200ms gaps.
            var waitAndRetryPolicy = Policy
                .Handle<Exception>(e =>
                    !(e is BrokenCircuitException) && !(e is ValidationException)) // Exception filtering!  We don't retry if the inner circuit-breaker judges the underlying system is out of commission!
                .WaitAndRetryForeverAsync(
                    attempt => TimeSpan.FromMilliseconds(200));

            // Define our CircuitBreaker policy: Break if the action fails 4 times in a row.
            var circuitBreakerPolicy = Policy
                .Handle<Exception>(e =>!(e is ValidationException) )
                .CircuitBreakerAsync(
                    4,
                    TimeSpan.FromSeconds(3),
                    (ex, breakDelay) =>
                    {
                        //TODO add log message
                    },
                    () =>
                    {
                        //TODO add log message
                    },
                    () =>
                    {
                        //TODO add log message
                    }
                );

            services.AddScoped(provider => Policy.WrapAsync(waitAndRetryPolicy, circuitBreakerPolicy));
            return services;
        }
    }
}