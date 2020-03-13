using System.Threading;
using System.Threading.Tasks;

namespace DocumentStore.Infrastructure
{
    /// <summary>
    /// File Storage Repository
    /// </summary>
    public interface IFileRepository
    {
        Task<string> UploadFile(string fileName, byte[] fileData, string fileMimeType);
        Task<byte[]> GetFile(string location);
        Task DeleteFile(string fileName, CancellationToken cancellationToken);
    }
}
