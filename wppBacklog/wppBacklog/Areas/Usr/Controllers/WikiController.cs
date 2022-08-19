using clsBacklog.Interfaces;
using clsBacklog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using wppBacklog.Areas.Usr.Models;

namespace wppBacklog.Areas.Usr.Controllers
{
    [Area("Usr"), Authorize()]
    public class WikiController : Controller
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly IProjectServices _projectServices;
        private readonly IOrganizationServices _organizationServices;

        public WikiController(UserManager<UserModel> userManager, IProjectServices projectServices, IOrganizationServices organizationServices)
        {
            _userManager = userManager;
            _projectServices = projectServices;
            _organizationServices = organizationServices;
        }

        [Route("/{culture}/organization/{organizationId}/project/{projectId}/wiki")]
        public async Task<IActionResult> Index(string culture, string organizationId, string projectId)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            // Make sure you are ready to be here.
            var organization = _organizationServices.GetOrganization(organizationId);

            if (organization == null)
            {
                return NotFound();
            }

            // Project
            var project = _projectServices.GetProject(organizationId, projectId);

            if (project == null)
            {
                // 
                return NotFound();
            }

            // Make sure you are in this.
            var member = _projectServices.GetProjectMembersView(organizationId, projectId, currentUser.Id, "", 1, 1);
            if (member.TotalItems == 0)
            {
                return NotFound();
            }

            var view = new UsrWikiIndexViewModel(project, organization)
            {
                Title = "Wiki",
                Culture = culture
            };

            return View(view);
        }
    }
}
