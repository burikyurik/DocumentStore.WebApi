using System;
using System.Threading;
using System.Threading.Tasks;
using DocumentStore.Infrastructure.Models;

namespace DocumentStore.Infrastructure
{
    /// <summary>
    /// Domain Entity Repository
    /// </summary>
    public interface IDomainRepository
    {
        Task Add(Document document, CancellationToken cancellationToken);
        Task<Document> GetDocumentByLocation(string location);
        Task<Document[]> GetDocuments(string sortQuery);
        Task Delete(Document document, CancellationToken cancellationToken);
    }
}