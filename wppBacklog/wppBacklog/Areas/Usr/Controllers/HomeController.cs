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

        public HomeController(UserManager<UserModel> userManager,
            IProjectServices projectServices,
            IOrganizationServices organizationServices)
        {
            _userManager = userManager;
            _projectServices = projectServices;
            _organizationServices = organizationServices;
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
    }
}
