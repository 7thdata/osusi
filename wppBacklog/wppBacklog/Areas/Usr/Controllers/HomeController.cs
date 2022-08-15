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
        public HomeController(UserManager<UserModel> userManager,
            IProjectServices projectServices)
        {
            _userManager = userManager;
            _projectServices = projectServices;
        }

        [Route("/{culture}/usr")]
        public async Task<IActionResult> Index(string culture)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            // Make sure you are ready to be here.
            if (string.IsNullOrEmpty(currentUser.OrganizationId))
            {
                return RedirectToAction("Details", "Organization", new { @culture = culture });
            }

            if (string.IsNullOrEmpty(currentUser.LastProjectId))
            {
                return RedirectToAction("Index", "Projects", new { @culture = culture });
            }

            // Project
            var project = _projectServices.GetProject(currentUser.LastProjectId);

            if (project == null)
            {
                // 
                return NotFound();
            }

            var view = new UsrHomeIndexViewModel(project)
            {
                Title = "Top of " + project.Name,
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
