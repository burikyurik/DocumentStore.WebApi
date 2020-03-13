using System;

namespace DocumentStore.Infrastructure.Models
{
    /// <summary>
    /// Document entity.
    /// </summary>
    public class Document
    {
        public Document(Guid id, string name, string location, long fileSize, string contentType)
        {
            Id = id;
            Name = name;
            Location = location;
            FileSize = fileSize;
            ContentType = contentType;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Location { get; private set; }
        public long FileSize { get; private set; }
        public string ContentType { get; private set; }
    }
}
