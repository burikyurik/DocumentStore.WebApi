using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;

namespace DocumentStore.Infrastructure
{
    public class AzureBlobStorageFileRepository : IFileRepository
    {
        private const string ContainerName = "pdfcontainer";
        private readonly string connectionString;
        public AzureBlobStorageFileRepository(string storageConnectionString)
        {
            connectionString = storageConnectionString;
        }


        public async Task<string> UploadFile(string fileName, byte[] fileData, string fileMimeType)
        {
            if (fileData == null)
                throw new ArgumentNullException(nameof(fileData));

            var container = await GetCloudBlobContainer();

            CloudBlockBlob cloudBlockBlob = container.GetBlockBlobReference(fileName);

            cloudBlockBlob.Properties.ContentType = fileMimeType;
            await cloudBlockBlob.UploadFromByteArrayAsync(fileData, 0, fileData.Length);
            return cloudBlockBlob.Uri.AbsoluteUri;
        }
        
        public async Task<byte[]> GetFile(string fileName)
        {
            var container = await GetCloudBlobContainer();
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);

            using (var stream = new MemoryStream())
            {
                await blockBlob.DownloadToStreamAsync(stream);
                return stream.ToArray();
            }
        }

        public async Task DeleteFile(string fileName, CancellationToken cancellationToken)
        {
            var container = await GetCloudBlobContainer();
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);

            await blockBlob.DeleteAsync(cancellationToken);
        }

        private async Task<CloudBlobContainer> GetCloudBlobContainer()
        {
            CloudBlobClient blobClient;
            if (CloudStorageAccount.TryParse(connectionString, out var cloudStorageAccount))
            {
                blobClient = cloudStorageAccount.CreateCloudBlobClient();
            }
            else
            {
                throw new ArgumentException("Invalid ConnectionString.");
            }

            CloudBlobContainer cloudBlobContainer = blobClient.GetContainerReference(ContainerName);
            if (await cloudBlobContainer.CreateIfNotExistsAsync())
            {
                await cloudBlobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
            }
            return cloudBlobContainer;
        }
    }
}