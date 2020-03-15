using System.Threading;
using System.Threading.Tasks;
using DocumentStore.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace DocumentStore.Infrastructure
{
    public class CosmosDbDomainRepository : IDomainRepository
    {
        private readonly DocumentContext context;

        public CosmosDbDomainRepository(DocumentContext context)
        {
            this.context = context;
            context?.Database.EnsureCreated();
        }
        public async Task Add(Document document, CancellationToken cancellationToken)
        {
            await context.AddAsync(document,cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        public Task<Document> GetDocumentByLocation(string location)
        {
            return context.Documents.FirstOrDefaultAsync(x => x.Location == location);
        }

        public Task<Document[]> GetDocuments(string sortQuery)
        {
            return context.Documents.OrderBy(sortQuery).ToArrayAsync();
        }

        public async Task Delete(Document document, CancellationToken cancellationToken)
        {
            context.Documents.Remove(document);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}