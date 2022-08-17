using clsBacklog.Interfaces;
using clsBacklog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using wppBacklog.Areas.Usr.Models;

namespace wppBacklog.Areas.Usr.Controllers
{
    [Area("Usr"), Authorize()]
    public class TasksController : Controller
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly ITaskServices _taskServices;
        private readonly IOrganizationServices _organizationServices;
        private readonly IProjectServices _projectServices;

        public TasksController(UserManager<UserModel> userManager,
            IOrganizationServices organizationServices,
            ITaskServices taskServices, IProjectServices projectServices)
        {
            _userManager = userManager;
            _taskServices = taskServices;
            _projectServices = projectServices;
            _organizationServices = organizationServices;
        }

        [Route("/{culture}/tasks")]
        public async Task<IActionResult> Index(string culture, string keyword, string sort, int currentPage = 1, int itemsPerPage = 50)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            // Make sure you are ready to be here.
            if (string.IsNullOrEmpty(currentUser.OrganizationId))
            {
                return RedirectToAction("Details", "Organization", new { @culture = culture });
            }

            var organization = _organizationServices.GetOrganization(currentUser.OrganizationId);

            if(organization == null)
            {
                return NotFound();
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

            // Show tasks.
            var tasks = _taskServices.GetTasks(currentUser.LastProjectId, keyword, sort, currentPage, itemsPerPage);

            var view = new UsrTaskIndexViewModel(project, organization, tasks)
            {
                Title = "Tasks",
                Culture = culture
            };

            return View(view);
        }

        [Route("/{culture}/board")]
        public async Task<IActionResult> Board(string culture)
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

            var view = new UsrTasksBoardViewModel(project)
            {
                Title = "Board",
                Culture = culture
            };

            return View(view);
        }

        [Route("/{culture}/gunt")]
        public async Task<IActionResult> Gunt(string culture)
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

            var view = new UsrTasksGuntViewModel(project)
            {
                Title = "Gunt Chart",
                Culture = culture
            };

            return View(view);
        }
    }
}
