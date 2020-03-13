using Microsoft.EntityFrameworkCore;

namespace DocumentStore.Infrastructure.Models
{
    public class DocumentContext : DbContext
    {
        public DocumentContext(DbContextOptions options) : base(options)
        {
            
        }
        public DbSet<Document> Documents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultContainer("DocumentStore");
        }
    }
}