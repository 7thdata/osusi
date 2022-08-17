using clsBacklog.Models;
using wppBacklog.Models;

namespace wppBacklog.Areas.Usr.Models
{
    public class UsrProjectViewModels
    {
    }

    public class UsrProjectIndexViewModel : ViewBaseModel
    {
        public UsrProjectIndexViewModel(PaginationModel<ProjectViewModel> projects,
            IList<OrganizationViewModel> organizations)
        {
            Projects = projects;
            Organizations = organizations;
        }
        public PaginationModel<ProjectViewModel> Projects { get; set; }
        public IList<OrganizationViewModel> Organizations { get; set; }
    }

    public class UsrProjectDetailsViewModel : ViewBaseModel
    {
        public UsrProjectDetailsViewModel(ProjectModel project,
            OrganizationModel organization)
        {
            Project = project;
            Organization = organization;
        }
        public ProjectModel Project { get; set; }
        public OrganizationModel Organization {get;set;}

        public IList<TaskStatusModel>? ListOfStatus { get; set; }
        public IList<TaskTypeModel>? ListOfTypes { get; set; }
        public IList<TaskCategoryModel>? ListOfCategories { get; set; }
        public IList<TaskMilestoneModel>? ListOfMileStones { get; set; }
        public IList<TaskVersionModel>? ListOfVersions { get; set; }
        public IList<OrganizationMemberViewModel>? OrganizationMembers { get; set; }
        public PaginationModel<ProjectMemberViewModel>? ProjectMembers { get; set; }
        public bool IsActiveProject { get; set; }
    }
}
