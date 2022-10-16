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

        /// <summary>
        /// List of projects.
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="organizationId"></param>
        /// <param name="projectId"></param>
        /// <param name="keyword"></param>
        /// <param name="sort"></param>
        /// <param name="currentPage"></param>
        /// <param name="itemsPerPage"></param>
        /// <returns></returns>
        [Route("/{culture}/organization/{organizationId}/project/{projectId}/tasks")]
        public async Task<IActionResult> Index(string culture, string organizationId, string projectId, string keyword, string sort,
            string filterTaskStatus,
            int currentPage = 1, int itemsPerPage = 50)
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

            // filters
            var searchFields = new UsersSavedSearch(Guid.NewGuid().ToString(), projectId, currentUser.Id);

            // When everything is null then load search criteria
            if (string.IsNullOrEmpty(keyword) && string.IsNullOrEmpty(filterTaskStatus))
            {
                searchFields = _taskServices.GetUsersSearch(projectId, currentUser.Id);

                if (searchFields == null)
                {
                    // First time?
                    searchFields = await _taskServices.SaveUsersSearchAsync(projectId, currentUser.Id, keyword, filterTaskStatus);
                }
            }
            else
            {
                // Then save it
                searchFields = await _taskServices.SaveUsersSearchAsync(projectId, currentUser.Id, keyword, filterTaskStatus);
            }

            // Get tasks.
            var tasks = _taskServices.GetTasksWithView(project.Id, searchFields.Keyword ?? "", sort, searchFields.TaskStatus??"", currentPage, itemsPerPage);


            // Load data.
            var assignableMembers = _projectServices.GetProjectMembersViewInList(project.OwnerId, project.Id);
            var taskCategories = _taskServices.GetCategories(project.Id);
            var taskStatuses = _taskServices.GetStatuses(project.Id);
            var taskTypes = _taskServices.GetTaskTypes(project.Id);
            var taskMilestones = _taskServices.GetMilestones(project.Id);
            var taskVersions = _taskServices.GetVersions(project.Id);

            var view = new UsrTaskIndexViewModel(project, organization, tasks, assignableMembers, taskTypes,
                taskStatuses, taskCategories, taskMilestones, taskVersions, searchFields)
            {
                Title = "Tasks",
                Culture = culture,
                DefaultEndTime = DateTime.Now.AddDays(3),
                DefaultStartTime = DateTime.Now
            };

            return View(view);
        }

        /// <summary>
        /// Upsert project.
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="organizationId"></param>
        /// <param name="projectId"></param>
        /// <param name="id"></param>
        /// <param name="taskType"></param>
        /// <param name="taskName"></param>
        /// <param name="taskDescription"></param>
        /// <param name="taskStatus"></param>
        /// <param name="assignPerson"></param>
        /// <param name="taskPriority"></param>
        /// <param name="taskMilestone"></param>
        /// <param name="taskCategory"></param>
        /// <param name="taskVersion"></param>
        /// <param name="start"></param>
        /// <param name="due"></param>
        /// <param name="planTime"></param>
        /// <returns></returns>
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

            var currentMember = _projectServices.GetProjectMemberViewByUid(organization.Id, project.Id, currentUser.Id);

            // Make sure you are in this.
            if (currentMember == null)
            {
                return NotFound();
            }

            var task = await _taskServices.CreateTaskAsync(new TaskModel(projectId, id, taskName, taskType, currentMember.Id)
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

            // Send notification if assigned.


            var view = new UsrTaskUpsertViewModel(project, organization, task)
            {
                Culture = culture,
                Title = "Task Added"
            };

            return View(view);
        }

        /// <summary>
        /// Detail of the project.
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="organizationId"></param>
        /// <param name="projectId"></param>
        /// <param name="id"></param>
        /// <param name="rcode"></param>
        /// <returns></returns>
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
            var task = _taskServices.GetTaskWithView(projectId, id);

            if (task == null)
            {
                return NotFound();
            }

            var assignableMembers = _projectServices.GetProjectMembersViewInList(project.OwnerId, project.Id);
            var taskCategories = _taskServices.GetCategories(project.Id);
            var taskStatuses = _taskServices.GetStatuses(project.Id);
            var taskTypes = _taskServices.GetTaskTypes(project.Id);
            var taskMilestones = _taskServices.GetMilestones(project.Id);
            var taskVersions = _taskServices.GetVersions(project.Id);
            var taskCompletionReasons = _taskServices.GetTaskCompletionReasons(projectId);

            var logs = _taskServices.GetTaskUpdates(id);

            var view = new UsrTaskDetailsViewModel(project, organization, task, logs, assignableMembers,
                taskTypes, taskStatuses, taskCategories, taskMilestones, taskVersions, taskCompletionReasons)
            {
                Culture = culture,
                Title = task.Name,
                RCode = rcode
            };

            return View(view);
        }

        /// <summary>
        /// Upsert log.
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="organizationId"></param>
        /// <param name="projectId"></param>
        /// <param name="taskId"></param>
        /// <param name="id"></param>
        /// <param name="comment"></param>
        /// <param name="assignedPerson"></param>
        /// <param name="taskStatus"></param>
        /// <param name="taskMilestone"></param>
        /// <param name="completeReason"></param>
        /// <param name="startFrom"></param>
        /// <param name="endAt"></param>
        /// <param name="plan"></param>
        /// <param name="actual"></param>
        /// <returns></returns>
        [HttpPost, AutoValidateAntiforgeryToken]
        [Route("/{culture}/organization/{organizationId}/project/{projectId}/task/{taskId}/log/upsert")]
        public async Task<IActionResult> UpsertLog(string culture, string organizationId, string projectId,
            string taskId, string id, string comment, string assignPerson, string taskStatus,
            string taskMilestone, string completeReason, DateTime startFrom, DateTime endAt, int plan, int actual)
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

            var log = await _taskServices.CreateTaskUpdateAsync(new TaskUpdateModel(projectId, taskId, id, comment, currentUser.Id)
            {
                Status = taskStatus,
                AssinedPerson = assignPerson,
                Milestone = taskMilestone,
                Reason = completeReason,
                StartFrom = startFrom,
                EndAt = endAt,
                ExpectedTime = plan,
                ActualTime = actual
            });

            if (log == null)
            {
                return BadRequest();
            }

            return RedirectToAction("Details", new { @culture = culture, @projectId = projectId, @organizationId = organizationId, @id = taskId, @rcode = 270 });
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
