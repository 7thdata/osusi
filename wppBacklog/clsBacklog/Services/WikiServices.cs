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
    public class WikiServices : IWikiServices
    {
        private readonly ApplicationDbContext _db;

        public WikiServices(ApplicationDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Get wikis.
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="parentId"></param>
        /// <param name="keyword"></param>
        /// <param name="sort"></param>
        /// <param name="currentPage"></param>
        /// <param name="itemsPerPage"></param>
        /// <returns></returns>
        public PaginationModel<WikiModel> GetWikis(string projectId, string parentId, string keyword, string sort, int currentPage, int itemsPerPage)
        {
            var wikis = from o in _db.Wikis where o.ProjectId == projectId && o.IsDeleted == false select o;

            if (!string.IsNullOrEmpty(parentId))
            {
                wikis = wikis.Where(w => w.ParentWikiId == parentId);
            }

            if (string.IsNullOrEmpty(keyword))
            {
                wikis = wikis.Where(o => o.Name.Contains(keyword));
            }

            int totalItems = wikis.Count();
            int totalPages = 0;

            if (totalItems > 0)
            {
                totalPages = (totalItems / itemsPerPage) + 1;
            }

            //
            wikis = wikis.Skip(itemsPerPage * (currentPage - 1));
            wikis = wikis.Take(itemsPerPage);

            var result = new PaginationModel<WikiModel>
            {
                Items = wikis.ToList(),
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
        /// Get wiki.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public WikiModel? GetWik(string id)
        {
            var wiki = (from f in _db.Wikis where f.Id == id && f.IsDeleted == false select f).FirstOrDefault();

            return wiki;
        }

        /// <summary>
        /// Upsert wiki.
        /// </summary>
        /// <param name="wiki"></param>
        /// <returns></returns>
        public async Task<WikiModel?> UpsertWikiAsync(WikiModel wiki)
        {
            var original = (from o in _db.Wikis where o.Id == wiki.Id && o.IsDeleted == false select o).FirstOrDefault();

            if (original == null)
            {
                _db.Wikis.Add(wiki);

                await _db.SaveChangesAsync();

                return wiki;
            }

            // Keep old.
            _db.WikiOlds.Add(new WikiOldModel(original.Id, Guid.NewGuid().ToString(), original.Name, original.Description, original.ParentWikiId)
            {
                Created = DateTime.Now,
                Deleted = original.Deleted,
                IsDeleted = original.IsDeleted,
                Modified = original.Modified,
                OwnerId = original.OwnerId,
                ParentWikiId = original.ParentWikiId
            });

            original.Modified = DateTime.Now;
            original.Name = wiki.Name;
            original.ParentWikiId = wiki.ParentWikiId;
            original.Description = wiki.Description;

            _db.Wikis.Update(wiki);

            await _db.SaveChangesAsync();

            return original;
        }

        /// <summary>
        /// Delete wiki.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<WikiModel?> DeleteWikiAsync(string id)
        {
            var original = (from o in _db.Wikis where o.Id == id select o).First();

            if (original == null)
            {
                return null;
            }

            original.IsDeleted = true;
            original.Deleted = DateTime.Now;

            _db.Wikis.Update(original);

            await _db.SaveChangesAsync();

            return original;
        }
    }
}
