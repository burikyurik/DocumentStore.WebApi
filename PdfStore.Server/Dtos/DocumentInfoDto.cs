namespace DocumentStore.Application.Dtos
{
    /// <summary>
    /// Uploaded document metadata Dto
    /// </summary>
    public class DocumentInfoDto
    {
        public DocumentInfoDto( string name, long fileSize, string location)
        {
            Name = name;
            FileSize = fileSize;
            Location = location;
        }

        public string Name { get; }
        public string Location { get; }
        public long FileSize { get; }
    }
}