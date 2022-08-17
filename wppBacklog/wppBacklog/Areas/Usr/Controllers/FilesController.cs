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
        public FilesController(UserManager<UserModel> userManager, IProjectServices projectServices)
        {
            _userManager = userManager;
            _projectServices = projectServices;
        }

        [Route("/{culture}/files")]
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
            var project = _projectServices.GetProject(currentUser.OrganizationId,currentUser.LastProjectId);

            if (project == null)
            {
                // 
                return NotFound();
            }

            var view = new UsrFilesIndexViewModel(project)
            {
                Title = "Files",
                Culture = culture
            };

            return View(view);
        }
    }
}
