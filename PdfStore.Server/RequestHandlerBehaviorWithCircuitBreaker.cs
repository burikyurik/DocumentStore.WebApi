using System.Threading;
using System.Threading.Tasks;
using CircuitBreaker.Net;
using MediatR;

namespace DocumentStore.Application
{
    public class RequestHandlerBehaviorWithCircuitBreaker<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ICircuitBreaker circuitBreaker;

        public RequestHandlerBehaviorWithCircuitBreaker(ICircuitBreaker circuitBreaker)
        {
            this.circuitBreaker = circuitBreaker;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            //return await next();
            return await circuitBreaker.ExecuteAsync(async () => await next());
        }
    }
}
