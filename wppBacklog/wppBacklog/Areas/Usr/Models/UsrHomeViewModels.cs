using clsBacklog.Models;
using wppBacklog.Models;

namespace wppBacklog.Areas.Usr.Models
{
    public class UsrHomeViewModels
    {
    }
    public class UsrHomeIndexViewModel : ViewBaseModel
    {
        public UsrHomeIndexViewModel(ProjectModel project, OrganizationModel organization)
        {
            Project = project;
            Organization = organization;
        }
        public OrganizationModel Organization { get; set; }
        public ProjectModel Project { get; set; }

    }
    public class UsrHomeSettingsViewModel : ViewBaseModel
    {

    }
    public class UsrHomeLogoutViewModel : ViewBaseModel
    {

    }
}
