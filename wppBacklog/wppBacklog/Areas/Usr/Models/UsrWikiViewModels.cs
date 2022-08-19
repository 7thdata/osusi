using clsBacklog.Models;
using wppBacklog.Models;

namespace wppBacklog.Areas.Usr.Models
{
    public class UsrWikiViewModels
    {
    }

    public class UsrWikiIndexViewModel : ViewBaseModel
    {
        public UsrWikiIndexViewModel(ProjectModel project, OrganizationModel organization)
        {
            Project = project;
            Organization = organization;
        }
        public OrganizationModel Organization { get; set; }
        public ProjectModel Project { get; set; }
    }
}
