using System;
using System.Threading;
using System.Threading.Tasks;
using DocumentStore.Application.Command;
using DocumentStore.Application.Validation;
using DocumentStore.Infrastructure;
using MediatR;

namespace DocumentStore.Application.CommandHandlers
{
    public class DeleteDocumentCommandHandler : IRequestHandler<DeleteDocumentCommand>
    {
        private readonly IFileRepository fileRepository;
        private readonly IDomainRepository domainRepository;
        public DeleteDocumentCommandHandler(IFileRepository fileRepository, IDomainRepository domainRepository)
        {
            this.fileRepository = fileRepository;
            this.domainRepository = domainRepository;
        }
        public async Task<Unit> Handle(DeleteDocumentCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var document = await domainRepository.GetDocumentByLocation(request.FileLocation);
            if(document==null)
                throw new ValidationException($"Document with Location: {request.FileLocation} doesn't exist.");

            await fileRepository.DeleteFile(document.Name, cancellationToken);
            await domainRepository.Delete(document, cancellationToken);
            return Unit.Value;
        }
    }
}