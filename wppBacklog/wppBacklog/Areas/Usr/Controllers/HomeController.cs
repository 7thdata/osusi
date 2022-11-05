using clsBacklog.Interfaces;
using clsBacklog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using wppBacklog.Areas.Usr.Models;

namespace wppBacklog.Areas.Usr.Controllers
{
    [Area("Usr"), Authorize()]
    public class HomeController : Controller
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly IProjectServices _projectServices;
        private readonly IOrganizationServices _organizationServices;
        private readonly SignInManager<UserModel> _signInManager;

        public HomeController(UserManager<UserModel> userManager,
            IProjectServices projectServices,
            IOrganizationServices organizationServices,
            SignInManager<UserModel> signInManager)
        {
            _userManager = userManager;
            _projectServices = projectServices;
            _organizationServices = organizationServices;
            _signInManager = signInManager;
        }

        [Route("/{culture}/usr")]
        public async Task<IActionResult> Index(string culture)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            // Make sure you are ready to be here.
            if (string.IsNullOrEmpty(currentUser.OrganizationId))
            {
                return RedirectToAction("Index", "Organizations", new { @culture = culture });
            }

            var organization = _organizationServices.GetOrganization(currentUser.OrganizationId);

            if (organization == null)
            {
                return NotFound();
            }

            if (string.IsNullOrEmpty(currentUser.LastProjectId))
            {
                return RedirectToAction("Index", "Projects", new { @culture = culture });
            }

            // Project
            var project = _projectServices.GetProject(currentUser.OrganizationId, currentUser.LastProjectId);

            if (project == null)
            {
                // Then go see project or create one.
                return RedirectToAction("Index", "Projects", new { @culture = culture });
            }

            // Make sure you are in this.
            var member = _projectServices.GetProjectMembersView(organization.Id, project.Id, currentUser.Id, "", 1, 1);
            if (member.TotalItems == 0)
            {
                return NotFound();
            }


            var view = new UsrHomeIndexViewModel(project,organization)
            {
                Title = project.Name,
                Culture = culture
            };

            return View(view);
        }

        [Route("/{culture}/settings")]
        public IActionResult Settings(string culture)
        {
            var view = new UsrHomeSettingsViewModel()
            {
                Title = "Settings",
                Culture = culture
            };

            return View(view);
        }

        [HttpPost, AutoValidateAntiforgeryToken]
        [Route("/{culture}/logout")]
        public async Task<IActionResult> Logout(string culture)
        {
            await _signInManager.SignOutAsync();

            var view = new UsrHomeLogoutViewModel()
            {
                Title = "Logout",
                Culture = culture
            };

            return View(view);
        }
    }
}
