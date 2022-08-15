using clsBacklog.Models;
using wppBacklog.Models;

namespace wppBacklog.Areas.Usr.Models
{
    public class UsrFilesViewModels
    {
    }

    public class UsrFilesIndexViewModel : ViewBaseModel
    {
        public UsrFilesIndexViewModel(ProjectModel project)
        {
            Project = project;
        }
        public ProjectModel Project { get; set; }
    }
}
