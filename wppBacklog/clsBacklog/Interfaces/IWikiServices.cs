using clsBacklog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clsBacklog.Interfaces
{
    public interface IWikiServices
    {

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
        PaginationModel<WikiModel> GetWikis(string projectId, string parentId, string keyword, string sort, int currentPage, int itemsPerPage);

        /// <summary>
        /// Get wiki.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        WikiModel? GetWik(string id);

        /// <summary>
        /// Upsert wiki.
        /// </summary>
        /// <param name="wiki"></param>
        /// <returns></returns>
        Task<WikiModel?> UpsertWikiAsync(WikiModel wiki);

        /// <summary>
        /// Delete wiki.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<WikiModel?> DeleteWikiAsync(string id);

    }
}
