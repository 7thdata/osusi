using clsBacklog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clsBacklog.Interfaces
{
    public interface IFileServices
    {
        /// <summary>
        /// Get files.
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="keyword"></param>
        /// <param name="sort"></param>
        /// <param name="currentPage"></param>
        /// <param name="itemsPerPage"></param>
        /// <returns></returns>
        PaginationModel<FileModel> GetFiles(string ownerId, string keyword, string sort, int currentPage, int itemsPerPage);
        
        /// <summary>
        /// Get file.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        FileModel? GetFile(string id);

        /// <summary>
        /// Upload file.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        Task<FileModel?> UploadFileAsync(FileModel file);

        /// <summary>
        /// Delete file.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<FileModel?> DeleteFileAsync(string id);

    }
}
