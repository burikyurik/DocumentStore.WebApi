using System.Collections.Generic;
using DocumentStore.Application.Dtos;
using MediatR;

namespace DocumentStore.Application.Query
{
    public class GetDocumentsQuery : IRequest<List<DocumentInfoDto>>
    {
        public string OrderBy { get; }

        public GetDocumentsQuery(string orderBy)
        {
            this.OrderBy = orderBy;
        }
    }
}
