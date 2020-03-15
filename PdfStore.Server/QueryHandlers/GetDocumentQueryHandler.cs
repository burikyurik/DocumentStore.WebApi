using System;
using System.Threading;
using System.Threading.Tasks;
using DocumentStore.Application.Dtos;
using DocumentStore.Application.Query;
using DocumentStore.Application.Validation;
using DocumentStore.Infrastructure;
using MediatR;

namespace DocumentStore.Application.QueryHandlers
{
    public class GetDocumentQueryHandler : IRequestHandler<DownloadDocumentQuery, DocumentDto>
    {
        private readonly IFileRepository fileRepository;
        private readonly IDomainRepository domainRepository;
        public GetDocumentQueryHandler(IFileRepository fileRepository, IDomainRepository domainRepository)
        {
            this.fileRepository = fileRepository;
            this.domainRepository = domainRepository;
        }

        public async Task<DocumentDto> Handle(DownloadDocumentQuery request, CancellationToken cancellationToken)
        {
            if(request==null)
                throw new ArgumentNullException(nameof(request));

            var documentInfo = await domainRepository.GetDocumentByLocation(request.Location);
            if (documentInfo == null)
                throw new ValidationException($"Document with Location: {request.Location} doesn't exist.");

            var fileData = await fileRepository.GetFile(documentInfo.Name);

            return new DocumentDto(documentInfo.Name, documentInfo.ContentType, documentInfo.FileSize, fileData);
        }
    }
}