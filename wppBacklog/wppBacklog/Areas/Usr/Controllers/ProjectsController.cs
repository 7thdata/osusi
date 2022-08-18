using clsBacklog.Interfaces;
using clsBacklog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using wppBacklog.Areas.Usr.Models;

namespace wppBacklog.Areas.Usr.Controllers
{
    [Area("Usr"), Authorize()]
    public class ProjectsController : Controller
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly IOrganizationServices _organizationServices;
        private readonly IProjectServices _projectServices;
        private readonly ITaskServices _taskServices;

        public ProjectsController(UserManager<UserModel> userManager,
            IProjectServices projectServices, IOrganizationServices organizationServices,
            ITaskServices taskServices)
        {
            _userManager = userManager;
            _projectServices = projectServices;
            _taskServices = taskServices;
            _organizationServices = organizationServices;
        }

        /// <summary>
        /// Show projects.
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="keyword"></param>
        /// <param name="sort"></param>
        /// <param name="currentPage"></param>
        /// <param name="itemsPerPage"></param>
        /// <returns></returns>
        [Route("/{culture}/projects")]
        public async Task<IActionResult> Index(string culture, string keyword, string sort, int currentPage = 1, int itemsPerPage = 50)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var projects = _projectServices.GetProjectsView(currentUser.Id, keyword, sort, currentPage, itemsPerPage);

            // 
            var organizations = _organizationServices.GetMyOrganizaionsInList(currentUser.Id);

            var view = new UsrProjectIndexViewModel(projects, organizations)
            {
                Culture = culture,
                Title = "Projects"
            };

            return View(view);
        }

        /// <summary>
        /// Create new project.
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="name"></param>
        /// <param name="permaName"></param>
        /// <param name="description"></param>
        /// <param name="displayOrder"></param>
        /// <returns></returns>
        [HttpPost, AutoValidateAntiforgeryToken]
        [Route("/{culture}/project/create")]
        public async Task<IActionResult> CreateProject(string culture, string organizationId, string name,
            string description, int displayOrder)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var projectId = Guid.NewGuid().ToString();

            // make permaname
            var permaName = "";
            var isPermaNameUnique = false;

            while (!isPermaNameUnique)
            {
                permaName = GetPermaName();

                isPermaNameUnique = _projectServices.IsPermaNameUnique(permaName, organizationId);
            }

            var project = await _projectServices.CreateProjectAsync(new ProjectModel(projectId,
                permaName, name, organizationId)
            {
                Description = description,
                DisplayOrder = displayOrder,
                OwnerId = organizationId
            }, currentUser.Id);

            // if project is null then likely an error.
            if (project == null)
            {
                return BadRequest();
            }

            return RedirectToAction("Details", new { @culture = culture, @organizationId = project.OwnerId, @id = project.Id, @rcode = 200 });
        }

        /// <summary>
        /// Generate PermaName.
        /// </summary>
        /// <returns></returns>
        private string GetPermaName()
        {
            string originalChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            string permaName = "";
            Random random = new Random();

            for (int i = 0; i < 8; i++)
            {
                int index = random.Next(originalChars.Length);

                permaName += originalChars[index];
            }

            return permaName;
        }

        /// <summary>
        /// Show detail of the project.
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("/{culture}/organization/{organizationId}/project/{id}")]
        public async Task<IActionResult> Details(string culture, string organizationId, string id, int rcode = 0, int currentPage = 1, int itemsPerPage = 50)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var isActiveProject = false;

            if (currentUser.LastProjectId == id)
            {
                isActiveProject = true;
            }

            // Later: Make sure this is your project.

            var project = _projectServices.GetProject(organizationId, id);

            if (project == null)
            {
                return NotFound();
            }

            // Get tatus
            var listOfStatus = _taskServices.GetStatuses(project.Id);
            var listOfTypes = _taskServices.GetTaskTypes(project.Id);
            var listOfCategories = _taskServices.GetCategories(project.Id);
            var listOfMilestones = _taskServices.GetMilestones(project.Id);
            var listOfVersion = _taskServices.GetVersions(project.Id);

            // Get members, Later: you should make this into partial view.
            var projectMembers = _projectServices.GetProjectMembersView(organizationId, project.Id, "", "", currentPage, itemsPerPage);
            var organizationMembers = _organizationServices.GetMembershipInformationByOrganizationIdFullListView(organizationId, "");
            var organization = _organizationServices.GetOrganization(organizationId);

            if (organization == null)
            {
                return NotFound();
            }

            var view = new UsrProjectDetailsViewModel(project, organization)
            {
                Project = project,
                Culture = culture,
                Title = project.Name,
                RCode = rcode,
                ListOfCategories = listOfCategories,
                ListOfMileStones = listOfMilestones,
                ListOfStatus = listOfStatus,
                ListOfTypes = listOfTypes,
                ListOfVersions = listOfVersion,
                ProjectMembers = projectMembers,
                OrganizationMembers = organizationMembers,
                IsActiveProject = isActiveProject
            };

            return View(view);
        }

        /// <summary>
        /// Set active project id and go back.
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, AutoValidateAntiforgeryToken]
        [Route("/{culture}/organization/{organizationId}/project/{projectId}/active")]
        public async Task<IActionResult> SetActiveProject(string culture, string organizationId, string projectId)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            currentUser.LastProjectId = projectId;
            currentUser.OrganizationId = organizationId;

            await _userManager.UpdateAsync(currentUser);

            return RedirectToAction("Details", new { @id = projectId, @culture = culture, @organizationId = organizationId, @rcode = 201 });
        }

        [HttpPost, AutoValidateAntiforgeryToken]
        [Route("/{culture}/organization/{organizationId}/project/{projectId}/status/upsert")]
        public async Task<IActionResult> UpsertStatus(string culture, string organizationId, string projectId, 
            string id, string name, string color, string textColor, int displayOrder)
        {
            if (string.IsNullOrEmpty(id))
            {
                id = Guid.NewGuid().ToString();
                var createResult = await _taskServices.CreateStatusAsync(new TaskStatusModel(
                    projectId, id, name, displayOrder, color,textColor));

                if (createResult == null)
                {
                    return BadRequest();
                }

                return RedirectToAction("Details", new { @culture = culture, @id = createResult.ProjectId, @organizationId = organizationId, @rcode = 210 });
            }

            var original = _taskServices.GetStatus(id);

            if (original == null)
            {
                return NotFound();
            }

            original.Name = name;
            original.Color = color;
            original.TextColor = textColor;
            original.DisplayOrder = displayOrder;

            var updateResult = await _taskServices.UpdateStatusAsync(original);

            if (updateResult == null)
            {
                return BadRequest();
            }

            return RedirectToAction("Details", new { @culture = culture, @id = updateResult.ProjectId, @organizationId = organizationId, @rcode = 220 });

        }

        [HttpPost, AutoValidateAntiforgeryToken]
        [Route("/{culture}/organization/{organizationId}/project/{projectId}/status/delete")]
        public async Task<IActionResult> DeleteStatus(string culture, string organizationId, string projectId, string id)
        {
            var result = await _taskServices.DeleteStatusAsync(id);

            if (result == null)
            {
                return BadRequest();
            }

            return RedirectToAction("Details", new { @culture = culture, @id = projectId, @organizationId = organizationId, @rcode = 230 });

        }

        [HttpPost, AutoValidateAntiforgeryToken]
        [Route("/{culture}/organization/{organizationId}/project/{projectId}/type/upsert")]
        public async Task<IActionResult> UpsertType(string culture, string organizationId, string projectId, string id, 
            string name, string color, string textColor, int displayOrder)
        {
            if (string.IsNullOrEmpty(id))
            {
                id = Guid.NewGuid().ToString();

                var result = await _taskServices.CreateTaskTypeAsync(new TaskTypeModel(
              projectId, id, name, displayOrder, color, textColor));


                if (result == null)
                {
                    return BadRequest();
                }
                return RedirectToAction("Details", new { @culture = culture, @id = result.ProjectId, @organizationId = organizationId, @rcode = 211 });

            }

            var original = _taskServices.GetTaskType(id);

            if (original == null)
            {
                return NotFound();
            }

            original.Name = name;
            original.TextColor = textColor;
            original.DisplayOrder = displayOrder;
            original.Color = color;

            var updateResult = await _taskServices.UpdateTaskTypeAsync(original);

            if (updateResult == null)
            {
                return BadRequest();
            }

            return RedirectToAction("Details", new { @culture = culture, @id = updateResult.ProjectId, @organizationId = organizationId, @rcode = 221 });
        }

        [HttpPost, AutoValidateAntiforgeryToken]
        [Route("/{culture}/organization/{organizationId}/project/{projectId}/type/delete")]
        public async Task<IActionResult> DeleteType(string culture, string organizationId, string projectId, string id)
        {
            var result = await _taskServices.DeleteTaskTypeAsync(id);

            if (result == null)
            {
                return BadRequest();
            }

            return RedirectToAction("Details", new { @culture = culture, @id = projectId, @organizationId = organizationId, @rcode = 231 });

        }


        [HttpPost, AutoValidateAntiforgeryToken]
        [Route("/{culture}/organization/{organizationId}/project/{projectId}/category/upsert")]
        public async Task<IActionResult> UpsertCategory(string culture, string organizationId, string projectId, string id, string name, int displayOrder)
        {
            if (string.IsNullOrEmpty(id))
            {
                id = Guid.NewGuid().ToString();

                var result = await _taskServices.CreateCategoryAsync(new TaskCategoryModel(
               projectId, id, name, displayOrder));

                if (result == null)
                {
                    return BadRequest();
                }

                return RedirectToAction("Details", new { @culture = culture, @id = result.ProjectId, @organizationId = organizationId, @rcode = 212 });
            }

            var original = _taskServices.GetCategory(id);

            if (original == null)
            {
                return NotFound();
            }

            original.Name = name;
            original.DisplayOrder = displayOrder;

            var updateResult = await _taskServices.UpdateCategoryAsync(original);

            if (updateResult == null)
            {
                return BadRequest();
            }

            return RedirectToAction("Details", new { @culture = culture, @id = updateResult.ProjectId, @organizationId = organizationId, @rcode = 222 });

        }

        [HttpPost, AutoValidateAntiforgeryToken]
        [Route("/{culture}/organization/{organizationId}/project/{projectId}/milestone/upsert")]
        public async Task<IActionResult> UpsertMilestone(string culture, string organizationId, string projectId, string id, string name, int displayOrder)
        {
            if (string.IsNullOrEmpty(id))
            {
                id = Guid.NewGuid().ToString();

                var result = await _taskServices.CreateMilestonesAsync(new TaskMilestoneModel(
              projectId, id, name, displayOrder));

                if (result == null)
                {
                    return BadRequest();
                }

                return RedirectToAction("Details", new { @culture = culture, @id = result.ProjectId, @organizationId = organizationId, @rcode = 213 });

            }

            var original = _taskServices.GetMilestone(id);

            if (original == null)
            {
                return NotFound();
            }

            original.Name = name;
            original.DisplayOrder = displayOrder;

            var updateResult = await _taskServices.UpdateMilestonesAsync(original);

            if (updateResult == null)
            {
                return BadRequest();
            }

            return RedirectToAction("Details", new { @culture = culture, @id = updateResult.ProjectId, @organizationId = organizationId, @rcode = 223 });

        }

        [HttpPost, AutoValidateAntiforgeryToken]
        [Route("/{culture}/organization/{organizationId}/project/{projectId}/version/upsert")]
        public async Task<IActionResult> UpsertVersion(string culture, string organizationId, string projectId, string id, string name, int displayOrder)
        {
            if (string.IsNullOrEmpty(id))
            {
                id = Guid.NewGuid().ToString();

                var result = await _taskServices.CreateVersionAsync(new TaskVersionModel(
               projectId, id, name, displayOrder));

                if (result == null)
                {
                    return BadRequest();
                }

                return RedirectToAction("Details", new { @culture = culture, @id = result.ProjectId, @organizationId = organizationId, @rcode = 214 });

            }

            var original = _taskServices.GetVersion(id);

            if (original == null)
            {
                return NotFound();
            }

            original.Name = name;
            original.DisplayOrder = displayOrder;

            var updateResult = await _taskServices.UpdateVersionAsync(original);

            if (updateResult == null)
            {
                return BadRequest();
            }

            return RedirectToAction("Details", new { @culture = culture, @id = updateResult.ProjectId, @organizationId = organizationId, @rcode = 224 });

        }

    }
}
