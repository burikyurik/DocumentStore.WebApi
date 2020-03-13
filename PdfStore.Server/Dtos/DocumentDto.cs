namespace DocumentStore.Application.Dtos
{
    /// <summary>
    /// Document Dto with metadata and FileData
    /// </summary>
    public class DocumentDto
    {
        public DocumentDto(string name, string contentType, long length, byte[] fileData)
        {
            Name = name;
            ContentType = contentType;
            Length = length;
            FileData = fileData;
        }
        public string Name { get;}
        public string ContentType { get; }
        public long Length { get; }
        public byte[] FileData { get; }
    }
}