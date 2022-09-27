using clsBacklog.Interfaces;
using clsBacklog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using wppBacklog.Areas.Usr.Models;

namespace wppBacklog.Areas.Usr.Controllers
{
    [Area("Usr"), Authorize()]
    public class FilesController : Controller
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly IProjectServices _projectServices;
        private readonly IOrganizationServices _organizationServices;
        public FilesController(UserManager<UserModel> userManager, IProjectServices projectServices, IOrganizationServices organizationServices)
        {
            _userManager = userManager;
            _projectServices = projectServices;
            _organizationServices = organizationServices;
        }

        [Route("/{culture}/organization/{organizationId}/project/{projectId}/files")]
        public async Task<IActionResult> Index(string culture, string organizationId, string projectId)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            // Project
            var project = _projectServices.GetProject(organizationId, projectId);

            if (project == null)
            {
                // 
                return NotFound();
            }

            var organization = _organizationServices.GetOrganization(organizationId);

            if (organization == null)
            {
                return NotFound();
            }

            // Make sure you are in this.
            var member = _projectServices.GetProjectMembersView(organizationId, projectId, currentUser.Id, "", 1, 1);
            if (member.TotalItems == 0)
            {
                return NotFound();
            }

            var view = new UsrFilesIndexViewModel(project, organization)
            {
                Title = "Files",
                Culture = culture
            };

            return View(view);
        }
    }
}
