using clsBacklog.Models;
using wppBacklog.Models;

namespace wppBacklog.Areas.Usr.Models
{
    public class UsrWikiViewModels
    {
    }

    public class UsrWikiIndexViewModel : ViewBaseModel
    {
        public UsrWikiIndexViewModel(ProjectModel project, OrganizationModel organization, PaginationModel<WikiModel> wikis)
        {
            Project = project;
            Organization = organization;
            Wikis = wikis;
        }
        public OrganizationModel Organization { get; set; }
        public ProjectModel Project { get; set; }
        public PaginationModel<WikiModel> Wikis { get; set; }
    }

    public class UsrWikiEditViewModel : ViewBaseModel
    {
        public UsrWikiEditViewModel(ProjectModel project, OrganizationModel organization, 
            WikiModel wiki)
        {
            Project = project;
            Organization = organization;
            Wiki = wiki;
        }
        public OrganizationModel Organization { get; set; }
        public ProjectModel Project { get; set; }
        public WikiModel Wiki { get; set; }
    }

    public class UsrWikiDetailsViewModel : ViewBaseModel
    {
        public UsrWikiDetailsViewModel(ProjectModel project, OrganizationModel organization,
            WikiModel wiki)
        {
            Project = project;
            Organization = organization;
            Wiki = wiki;
        }
        public OrganizationModel Organization { get; set; }
        public ProjectModel Project { get; set; }
        public WikiModel Wiki { get; set; }
    }
}
