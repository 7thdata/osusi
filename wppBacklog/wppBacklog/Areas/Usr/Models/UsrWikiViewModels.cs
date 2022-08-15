using clsBacklog.Models;
using wppBacklog.Models;

namespace wppBacklog.Areas.Usr.Models
{
    public class UsrWikiViewModels
    {
    }

    public class UsrWikiIndexViewModel : ViewBaseModel
    {
        public UsrWikiIndexViewModel(ProjectModel project)
        {
            Project = project;
        }
        public ProjectModel Project { get; set; }
    }
}
