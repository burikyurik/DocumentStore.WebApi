using DocumentStore.Application.Dtos;
using MediatR;

namespace DocumentStore.Application.Query
{
    public class DownloadDocumentQuery : IRequest<DocumentDto>
    {
        public string Location { get;}
        public DownloadDocumentQuery(string location)
        {
            Location = location;
        }
    }
}