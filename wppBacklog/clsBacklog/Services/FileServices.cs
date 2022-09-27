using clsBacklog.Data;
using clsBacklog.Interfaces;
using clsBacklog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clsBacklog.Services
{
    public class FileServices : IFileServices
    {
        private readonly ApplicationDbContext _db;

        public FileServices(ApplicationDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Get files.
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="keyword"></param>
        /// <param name="sort"></param>
        /// <param name="currentPage"></param>
        /// <param name="itemsPerPage"></param>
        /// <returns></returns>
        public PaginationModel<FileModel> GetFiles(string ownerId, string keyword, string sort, int currentPage, int itemsPerPage)
        {
            var files = from o in _db.Files where o.OwnerId == ownerId && o.IsDeleted == false select o;

            if (string.IsNullOrEmpty(keyword))
            {
                files = files.Where(o => o.FileName.Contains(keyword));
            }

            int totalItems = files.Count();
            int totalPages = 0;

            if (totalItems > 0)
            {
                totalPages = (totalItems / itemsPerPage) + 1;
            }

            //
            files = files.Skip(itemsPerPage * (currentPage - 1));
            files = files.Take(itemsPerPage);

            var result = new PaginationModel<FileModel>
            {
                Items = files.ToList(),
                ItemsPerPage = itemsPerPage,
                Sort = sort,
                CurrentPage = currentPage,
                Keyword = keyword,
                TotalItems = totalItems,
                TotalPages = totalPages
            };

            return result;
        }

        /// <summary>
        /// Get file.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public FileModel? GetFile(string id)
        {

            var file = (from f in _db.Files where f.Id == id && f.IsDeleted == false select f).FirstOrDefault();

            return file;
        }

        /// <summary>
        /// Upsert file.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<FileModel?> UploadFileAsync(FileModel file)
        {
            var original = (from o in _db.Files where o.Id == file.Id && o.IsDeleted == false select o).FirstOrDefault();

            if (original == null)
            {
                _db.Files.Add(file);

                await _db.SaveChangesAsync();

                return file;
            }

            original.Modified = DateTime.Now;

            _db.Files.Update(file);

            await _db.SaveChangesAsync();

            return original;
        }

        /// <summary>
        /// Delete file.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<FileModel?> DeleteFileAsync(string id)
        {
            var original = (from o in _db.Files where o.Id == id select o).First();

            if (original == null)
            {
                return null;
            }

            original.IsDeleted = true;
            original.Deleted = DateTime.Now;

            _db.Files.Update(original);

            await _db.SaveChangesAsync();

            return original;
        }
    }
}
