using clsBacklog.Models;
using wppBacklog.Models;

namespace wppBacklog.Areas.Usr.Models
{
    public class UsrHomeViewModels
    {
    }
    public class UsrHomeIndexViewModel : ViewBaseModel
    {
        public UsrHomeIndexViewModel(ProjectModel project)
        {
            Project = project;
        }
        public ProjectModel Project { get; set; }

    }
    public class UsrHomeSettingsViewModel : ViewBaseModel
    {

    }
}
