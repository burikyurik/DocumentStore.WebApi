using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;

namespace DocumentStore.Application.Validation
{
    public class CommandValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IValidator<TRequest> validator;

        public CommandValidationBehavior(IValidator<TRequest> validator=null)
        {
            this.validator = validator;
        }

        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var validationResult = validator?.Validate(request);

            if (validationResult == null || validationResult.IsValid)
                return next();

            var errorBuilder = new StringBuilder();

            errorBuilder.AppendLine("Validation reason: ");

            foreach (var error in validationResult.Errors)
            {
                errorBuilder.AppendLine(error.ErrorMessage);
            }

            throw new ValidationException(errorBuilder.ToString());
        }
    }
}
