using System;
using System.Threading;
using System.Threading.Tasks;
using CircuitBreaker.Net;
using MediatR;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Polly.Wrap;

namespace DocumentStore.Application
{
    public class RequestHandlerBehaviorWithCircuitBreaker<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly AsyncPolicyWrap policyWrap;
        private readonly ILogger<RequestHandlerBehaviorWithCircuitBreaker<TRequest, TResponse>> logger;

        public RequestHandlerBehaviorWithCircuitBreaker(AsyncPolicyWrap policy, ILogger<RequestHandlerBehaviorWithCircuitBreaker<TRequest, TResponse>> logger)
        {
            this.policyWrap = policy;
            this.logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            try
            {
                // Retry the following call according to the policy wrap
                return await policyWrap.ExecuteAsync(() => next());
            }
            catch (BrokenCircuitException circuitException)
            {
                logger.LogError($"Request {request.GetType().Name} failed with Broken Circuit. Exception message: {circuitException.Message}");
                throw;
            }
            catch (Exception e)
            {
                logger.LogError($"Request {request.GetType().Name} failed with Exception message: {e.Message}");
                throw;
            }
        }
    }
}
