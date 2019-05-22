using System;
using System.IO;
using System.Threading.Tasks;
using ailogica.Azure.Helpers.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace ailogica.Azure.Helpers
{
    public class BlobHelpers
    {
        private readonly string _storageConnectionString;
        public BlobHelpers(string storageConnectionString)
        {
            _storageConnectionString = storageConnectionString;
        }

        public async Task<HelperResponse> UploadBlob(string containername, string filename, string filepath, bool overwrite = true)
        {
            var storageacc = CloudStorageAccount.Parse(_storageConnectionString);
            var cloudBlobClient = storageacc.CreateCloudBlobClient();
            var container = cloudBlobClient.GetContainerReference(containername);

            if (string.IsNullOrEmpty(filename))
            {
                filename = Path.GetFileName(filepath);
            }

            try
            {

                if (await container.CreateIfNotExistsAsync())
                {
                    await container.SetPermissionsAsync(
                        new BlobContainerPermissions
                        {
                            PublicAccess = BlobContainerPublicAccessType.Blob
                        }
                        );
                }
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(filename);

                await blockBlob.UploadFromFileAsync(filepath);

                return new HelperResponse { Message = "success", Success = true, FileName = filename, FileUrl = blockBlob.Uri.ToString() };
            }
            catch (Exception ex)
            {
                return new HelperResponse { Message = ex.Message, Success = false };
            }

        }

        public async Task<HelperResponse> UploadBlob(string containername, string filename, Stream filestream)
        {
            var storageacc = CloudStorageAccount.Parse(_storageConnectionString);
            var cloudBlobClient = storageacc.CreateCloudBlobClient();
            var container = cloudBlobClient.GetContainerReference(containername);
            
            try
            {

                if (await container.CreateIfNotExistsAsync())
                {
                    await container.SetPermissionsAsync(
                        new BlobContainerPermissions
                        {
                            PublicAccess = BlobContainerPublicAccessType.Blob
                        }
                        );
                }
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(filename);

                await blockBlob.UploadFromStreamAsync(filestream);
                
                return new HelperResponse { Message = "success", Success = true, FileName = filename , FileUrl = blockBlob.Uri.ToString() };
            }
            catch (Exception ex)
            {
                return new HelperResponse { Message = ex.Message, Success = false };
            }

        }

        public async Task<HelperResponse> BlobExist(string containername, string filename, string filepath)
        {
            var storageacc = CloudStorageAccount.Parse(_storageConnectionString);
            var cloudBlobClient = storageacc.CreateCloudBlobClient();
            var container = cloudBlobClient.GetContainerReference(containername);

            if (string.IsNullOrEmpty(filename))
            {
                filename = Path.GetFileName(filepath);
            }

            CloudBlockBlob blockBlob = container.GetBlockBlobReference(filename);
            try
            {
                var isExists = await blockBlob.ExistsAsync();
                if (!isExists)
                {
                    return new HelperResponse { Message = "blob does not exist", Success = false, FileName = filename };
                }

                return new HelperResponse { Message = "success", Success = true, FileName = filename };
            }
            catch (Exception ex)
            {
                return new HelperResponse { Message = ex.Message, Success = false };
            }

        }

    }
}


