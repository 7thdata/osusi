using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using wppBacklog.Handlers.Interfaces;
using wppBacklog.Models;

namespace wppBacklog.Handlers
{
    public class BlobHandlers : IBlobHandlers
    {
        private readonly IOptions<AppConfigModel> _config;
        private readonly string _imageUrl = "https://strprdbacklog.blob.core.windows.net/";

        public BlobHandlers(IOptions<AppConfigModel> config)
        {
            _config = config;
        }

        /// <summary>
        /// Upload image and get list of urls.
        /// </summary>
        /// <param name="container"></param>
        /// <param name="images"></param>
        /// <returns></returns>
        public async Task<List<string>> UploadImageAsync(string container, [FromForm] IFormFileCollection images)
        {

            var listOfImages = new List<string>();

            // Upsert image
            var connectionString = _config.Value.AzureBlob.ConnectionString;
            var blobServiceClient = new BlobServiceClient(connectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(container);

            if (await containerClient.ExistsAsync() == false)
            {
                await containerClient.CreateIfNotExistsAsync();
                await containerClient.SetAccessPolicyAsync(PublicAccessType.BlobContainer);
            }

            foreach (var image in images)
            {
                var id = Guid.NewGuid().ToString();

                var fileEx = image.FileName.Substring(image.FileName.Length - 4, 4);
                if (fileEx == "jpeg")
                {
                    fileEx = ".jpg";
                }

                var fileName = id + fileEx;
                var blobClient = containerClient.GetBlobClient(fileName);

                var httpHeaders = new BlobHttpHeaders()
                {
                    ContentType = image.ContentType
                };

                // Upload the file
                await blobClient.UploadAsync(image.OpenReadStream(), httpHeaders);

                var url = _imageUrl + container + "/" + fileName;


                listOfImages.Add(url);
            }

            return listOfImages;
        }
    }
}
