using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using ValidationException = DocumentStore.Application.Validation.ValidationException;

namespace DocumentStore.Application.CommandHandlers
{
    public abstract class BaseCommandHandlerWithValidation<TCommand> : IRequestHandler<TCommand> where TCommand: IRequest
    {
        private readonly IValidator<TCommand> validator;

        protected BaseCommandHandlerWithValidation(IValidator<TCommand> validator)
        {
            this.validator = validator;
        }
        public async Task<Unit> Handle(TCommand request, CancellationToken cancellationToken)
        {
            var validationResult = validator?.Validate(request);

            if (validationResult == null || validationResult.IsValid)
                return await ProcessHandle(request, cancellationToken);

            var errorBuilder = new StringBuilder();

            errorBuilder.AppendLine("Validation reason: ");

            foreach (var error in validationResult.Errors)
            {
                errorBuilder.AppendLine(error.ErrorMessage);
            }

            throw new ValidationException(errorBuilder.ToString());
        }

        protected abstract Task<Unit> ProcessHandle(TCommand request, CancellationToken cancellationToken);
    }
}