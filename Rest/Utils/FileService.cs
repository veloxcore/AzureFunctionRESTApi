using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Rest.Utils
{
    /// <summary>
    /// The fileservice to save blobs in azure web storage account
    /// </summary>
    public static class FileService
    {
        /// <summary>
        /// To create blobs from httprequest with multipart form data with files
        /// </summary>
        /// <param name="request"> httprequest with conent type multipart/form-data and files in body</param>
        /// <param name="containerName">container name in which file to be store in azure web storage</param>
        /// <param name="cloudStorageAccount">the cloud storage account</param>
        /// <param name="logger">The logger</param>
        /// <returns></returns>
        public async static Task<Dictionary<string, string>> CreateBlobsAsync(HttpRequestMessage request, string containerName, CloudStorageAccount cloudStorageAccount)
        {
            Dictionary<string, string> fileUrls = new Dictionary<string, string>();
            var provider = await request.Content.ReadAsMultipartAsync(new InMemoryMultipartFormDataStreamProvider());
            //access form data  
            NameValueCollection formData = provider.FormData;
            //access files  
            IList<HttpContent> files = provider.Files;
            foreach (var file in files)
            {
                var thisFileName = file.Headers.ContentDisposition.FileName.Trim('\"');
                Stream input = await file.ReadAsStreamAsync();
                string fileContentType = file.Headers.ContentType.ToString();
                if (input != null)
                {
                    string name;
                    name = Guid.NewGuid().ToString("n");
                    var extension = thisFileName.Split('.').Last();
                    string url = await FileService.CreateBlobAsync(name + "." + extension, input, fileContentType, containerName, cloudStorageAccount);
                    fileUrls.Add(thisFileName, url);
                }
            }
            return fileUrls;
        }

        /// <summary>
        /// To Insert files
        /// </summary>
        /// <param name="name">Filename</param>
        /// <param name="data">the file data</param>
        /// <param name="contentType">content type of file</param>
        /// <param name="containerName">container name of file to be stored in azure web storage (small letters only)</param>
        /// <param name="cloudStorageAccount">cloud storage account</param>
        /// <param name="logger">the logger</param>
        /// <returns>url of uploaded file</returns>
        private async static Task<string> CreateBlobAsync(string name, Stream data, string contentType, string containerName, CloudStorageAccount cloudStorageAccount)
        {
            CloudBlobClient client;
            CloudBlobContainer container;
            CloudBlockBlob blob;
            client = cloudStorageAccount.CreateCloudBlobClient();

            ///Azure webstorage Container name
            ///It always should be in small characters
            container = client.GetContainerReference(containerName);

            await container.CreateIfNotExistsAsync();

            blob = container.GetBlockBlobReference(name);
            blob.Properties.ContentType = contentType;
            await blob.UploadFromStreamAsync(data);
            return blob.StorageUri.PrimaryUri.ToString();
        }
    }
}
