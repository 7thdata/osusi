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

        [Route("/{culture}/organization/{organizationId}/project/{projectId}/tasks")]
        public async Task<IActionResult> Index(string culture, string organizationId, string projectId, string keyword, string sort, int currentPage = 1, int itemsPerPage = 50)
        {
            var currentUser = await _userManager.GetUserAsync(User);

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


            // Show tasks.
            var tasks = _taskServices.GetTasks(project.Id, keyword, sort, currentPage, itemsPerPage);

            var assignableMembers = _projectServices.GetProjectMembersViewInList(project.OwnerId, project.Id);
            var taskCategories = _taskServices.GetCategories(project.Id);
            var taskStatuses = _taskServices.GetStatuses(project.Id);
            var taskTypes = _taskServices.GetTaskTypes(project.Id);
            var taskMilestones = _taskServices.GetMilestones(project.Id);
            var taskVersions = _taskServices.GetVersions(project.Id);

            var view = new UsrTaskIndexViewModel(project, organization, tasks, assignableMembers, taskTypes,
                taskStatuses, taskCategories, taskMilestones, taskVersions)
            {
                Title = "Tasks",
                Culture = culture
            };

            return View(view);
        }

        [HttpPost, AutoValidateAntiforgeryToken]
        [Route("/{culture}/organization/{organizationId}/project/{projectId}/task/upsert")]
        public async Task<IActionResult> Upsert(string culture, string organizationId, string projectId, string id, string taskType,
            string taskName, string taskDescription, string taskStatus, string assignPerson,
            int taskPriority, string taskMilestone, string taskCategory, string taskVersion, DateTime start,
            DateTime due, int planTime)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (string.IsNullOrEmpty(id))
            {
                id = Guid.NewGuid().ToString();
            }

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

            var task = await _taskServices.CreateTaskAsync(new TaskModel(projectId, id, taskName, taskType,currentUser.Id)
            {
                StartFrom = start,
                EndAt = due,
                Status = taskStatus,
                AssignedPerson = assignPerson,
                Description = taskDescription,
                ExpectedTime = planTime,
                OwnerId = organizationId,
                Priority = taskPriority,
                TaskApplicableVersion = taskVersion,
                TaskMilestone = taskMilestone,
                TaskCategory = taskCategory
            });

            if (task == null)
            {
                return BadRequest();
            }

            var view = new UsrTaskUpsertViewModel(project, organization, task)
            {
                Culture = culture,
                Title = "Task Added"
            };

            return View(view);
        }

        [Route("/{culture}/organization/{organizationId}/project/{projectId}/task/{id}")]
        public async Task<IActionResult> Details(string culture, string organizationId, string projectId, string id, int rcode = 0)
        {

            var currentUser = await _userManager.GetUserAsync(User);

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

            // Show tasks.
            var task = _taskServices.GetTask(projectId, id);

            var assignableMembers = _projectServices.GetProjectMembersViewInList(project.OwnerId, project.Id);
            var taskCategories = _taskServices.GetCategories(project.Id);
            var taskStatuses = _taskServices.GetStatuses(project.Id);
            var taskTypes = _taskServices.GetTaskTypes(project.Id);
            var taskMilestones = _taskServices.GetMilestones(project.Id);
            var taskVersions = _taskServices.GetVersions(project.Id);

            var logs = _taskServices.GetTaskUpdates(id);

            var view = new UsrTaskDetailsViewModel(project, organization, task, logs, assignableMembers,
                taskTypes, taskStatuses, taskCategories, taskMilestones, taskVersions)
            {
                Culture = culture,
                Title = task.Name,
                RCode = rcode
            };

            return View(view);
        }

        [Route("/{culture}/organization/{organizationId}/project/{projectId}/board")]
        public async Task<IActionResult> Board(string culture, string organizationId, string projectId)
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

            var view = new UsrTasksBoardViewModel(project, organization)
            {
                Title = "Board",
                Culture = culture
            };

            return View(view);
        }

        [Route("/{culture}/organization/{organizationId}/project/{projectId}/gunt")]
        public async Task<IActionResult> Gunt(string culture, string organizationId, string projectId)
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

            var view = new UsrTasksGuntViewModel(project, organization)
            {
                Title = "Gunt Chart",
                Culture = culture
            };

            return View(view);
        }
    }
}
