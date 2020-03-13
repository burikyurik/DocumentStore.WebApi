using MediatR;

namespace DocumentStore.Application.Command
{
    public class DeleteDocumentCommand : IRequest
    {
        public string FileLocation { get;}
        public DeleteDocumentCommand(string location)
        {
            FileLocation = location;
        }
    }
}