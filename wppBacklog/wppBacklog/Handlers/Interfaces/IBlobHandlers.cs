using Microsoft.AspNetCore.Mvc;

namespace wppBacklog.Handlers.Interfaces
{
    public interface IBlobHandlers
    {

        /// <summary>
        /// Upload images and get urls.
        /// </summary>
        /// <param name="container"></param>
        /// <param name="images"></param>
        /// <returns></returns>
        Task<List<string>> UploadImageAsync(string container, [FromForm] IFormFileCollection images);
    }
}
