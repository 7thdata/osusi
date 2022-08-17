using clsBacklog.Models;
using wppBacklog.Models;

namespace wppBacklog.Areas.Usr.Models
{
    public class UsrOrganizationsViewModels
    {
    }
    public class UsrOrganizationIndexViewModel : ViewBaseModel
    {
        public UsrOrganizationIndexViewModel(PaginationModel<OrganizationViewModel> organizations)
        {
            Organizations = organizations;
        }
        public PaginationModel<OrganizationViewModel> Organizations { get; set; }
    }
    public class UsrOrganizationDetailsViewModel : ViewBaseModel
    {
        public UsrOrganizationDetailsViewModel(OrganizationModel organization)
        {
            Organization = organization;
        }

        public OrganizationModel Organization { get; set; }
        public PaginationModel<OrganizationMemberViewModel>? Members { get; set; }
        public bool IsActiveOrganization { get; set; }
    }
 
    public class UsrOrganizationSubscroptionViewModel : ViewBaseModel
    {

    }
}
