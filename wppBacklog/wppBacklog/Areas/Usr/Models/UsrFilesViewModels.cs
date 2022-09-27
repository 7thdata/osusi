using clsBacklog.Models;
using wppBacklog.Models;

namespace wppBacklog.Areas.Usr.Models
{
    public class UsrFilesViewModels
    {
    }

    public class UsrFilesIndexViewModel : ViewBaseModel
    {
        public UsrFilesIndexViewModel(ProjectModel project, OrganizationModel organization)
        {
            Project = project;
            Organization = organization;
        }
        public OrganizationModel Organization { get; set; }
        public ProjectModel Project { get; set; }
    }
}
