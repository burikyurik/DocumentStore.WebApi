using DocumentStore.Application.Dtos;
using MediatR;

namespace DocumentStore.Application.Command
{
    public class UploadDocumentCommand : IRequest
    {
        public DocumentDto DocumentDto { get; }

        public UploadDocumentCommand(DocumentDto documentDto)
        {
            DocumentDto = documentDto;
        }
    }
}
