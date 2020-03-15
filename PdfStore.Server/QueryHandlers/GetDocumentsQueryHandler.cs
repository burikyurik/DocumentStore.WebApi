using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DocumentStore.Application.Dtos;
using DocumentStore.Application.Query;
using DocumentStore.Infrastructure;
using DocumentStore.Infrastructure.Models;
using MediatR;

namespace DocumentStore.Application.QueryHandlers
{
    public class GetDocumentsQueryHandler : IRequestHandler<GetDocumentsQuery, List<DocumentInfoDto>>
    {
        private readonly IDomainRepository domainRepository;
        private readonly ISortHelper<Document> sortHelper;
        public GetDocumentsQueryHandler(IDomainRepository domainRepository, ISortHelper<Document> sortHelper)
        {
            this.domainRepository = domainRepository;
            this.sortHelper = sortHelper;
        }

        public async Task<List<DocumentInfoDto>> Handle(GetDocumentsQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var documents = await domainRepository.GetDocuments(sortHelper.CreateSortQuery(request.OrderBy));

            return documents.Select(x => new DocumentInfoDto(x.Name, x.FileSize, x.Location)).ToList();
        }
    }
}
