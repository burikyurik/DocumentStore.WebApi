using System.IO;
using DocumentStore.Application.Command;
using FluentValidation;

namespace DocumentStore.Application.CommandHandlers
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1710:Identifiers should have correct suffix", Justification = "<Pending>")]
    public class UploadDocumentCommandValidator : AbstractValidator<UploadDocumentCommand>
    {
        //TODO inject from container
        private const long MaxFileSize = 5242880;//5Mb
        private const string AllowType = ".pdf";
        public UploadDocumentCommandValidator()
        {
            RuleFor(x => x.DocumentDto.Length).LessThanOrEqualTo(MaxFileSize).WithMessage($"File too large, limit is {MaxFileSize} Byte");
            RuleFor(x => Path.GetExtension(x.DocumentDto.Name).ToLowerInvariant()).NotEmpty().WithMessage("File Extension is empty");
            RuleFor(x => Path.GetExtension(x.DocumentDto.Name).ToLowerInvariant()).Equal(AllowType).WithMessage($"Not Allowed File Extension. Allowed Extension {AllowType}");
        }
    }
}