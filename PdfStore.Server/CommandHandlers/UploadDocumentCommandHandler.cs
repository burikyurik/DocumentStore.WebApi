using System;
using System.Threading;
using System.Threading.Tasks;
using DocumentStore.Application.Command;
using DocumentStore.Infrastructure;
using DocumentStore.Infrastructure.Models;
using MediatR;

namespace DocumentStore.Application.CommandHandlers
{
    public class UploadDocumentCommandHandler : IRequestHandler<UploadDocumentCommand>
    {
        private readonly IFileRepository fileRepository;
        private readonly IDomainRepository domainRepository;

        public UploadDocumentCommandHandler(IFileRepository fileRepository, IDomainRepository domainRepository)
        {
            this.fileRepository = fileRepository;
            this.domainRepository = domainRepository;
        }
        public async Task<Unit> Handle(UploadDocumentCommand request, CancellationToken cancellationToken)
        {
            var fileLocation = await fileRepository.UploadFile(request.DocumentDto.Name, request.DocumentDto.FileData, request.DocumentDto.ContentType);
            await domainRepository.Add(new Document(Guid.NewGuid(), request.DocumentDto.Name, fileLocation, request.DocumentDto.Length, request.DocumentDto.ContentType), cancellationToken);
            return Unit.Value;
        }
    }
}
