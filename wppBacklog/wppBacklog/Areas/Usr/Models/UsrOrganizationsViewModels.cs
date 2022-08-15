using clsBacklog.Models;
using wppBacklog.Models;

namespace wppBacklog.Areas.Usr.Models
{
    public class UsrOrganizationsViewModels
    {
    }

    public class UsrOrganizationDetailsViewModel : ViewBaseModel
    {
        public OrganizationModel? Organization { get; set; }
        public PaginationModel<OrganizationMemberViewModel>? Members { get; set; }
    }
 
    public class UsrOrganizationSubscroptionViewModel : ViewBaseModel
    {

    }
}
