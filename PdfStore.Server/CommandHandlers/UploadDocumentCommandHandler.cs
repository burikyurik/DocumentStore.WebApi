using System;
using System.Threading;
using System.Threading.Tasks;
using DocumentStore.Application.Command;
using DocumentStore.Infrastructure;
using DocumentStore.Infrastructure.Models;
using FluentValidation;
using MediatR;

namespace DocumentStore.Application.CommandHandlers
{
    public class UploadDocumentCommandHandler : BaseCommandHandlerWithValidation<UploadDocumentCommand>
    {
        private readonly IFileRepository fileRepository;
        private readonly IDomainRepository domainRepository;

        public UploadDocumentCommandHandler(IFileRepository fileRepository, IDomainRepository domainRepository, IValidator<UploadDocumentCommand> validator) : base(validator)
        {
            this.fileRepository = fileRepository;
            this.domainRepository = domainRepository;
        }
        protected override async Task<Unit> ProcessHandle(UploadDocumentCommand request, CancellationToken cancellationToken)
        {
            var fileLocation = await fileRepository.UploadFile(request.DocumentDto.Name, request.DocumentDto.FileData, request.DocumentDto.ContentType);
            await domainRepository.Add(new Document(Guid.NewGuid(), request.DocumentDto.Name, fileLocation, request.DocumentDto.Length, request.DocumentDto.ContentType), cancellationToken);
            return Unit.Value;
        }
    }
}
