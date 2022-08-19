using clsBacklog.Data;
using clsBacklog.Interfaces;
using clsBacklog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clsBacklog.Services
{
    public class TaskServices : ITaskServices
    {
        private readonly IdentityContext _identity;
        private readonly ApplicationContext _db;

        public TaskServices(IdentityContext identity,
           ApplicationContext db)
        {
            _identity = identity;
            _db = db;
        }

        /// <summary>
        /// Get tasks.
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="keyword"></param>
        /// <param name="sort"></param>
        /// <param name="currentPage"></param>
        /// <param name="itemsPerPage"></param>
        /// <returns></returns>
        public PaginationModel<TaskModel> GetTasks(string projectId, string keyword, string sort, int currentPage, int itemsPerPage)
        {
            var tasks = from p in _db.Tasks where p.IsDeleted == false && p.ProjectId == projectId select p;

            if (!string.IsNullOrEmpty(keyword))
            {
                // Search logic here.
                tasks = tasks.Where(g => g.Name.Contains(keyword) || g.Id.Contains(keyword));
            }

            if (!string.IsNullOrEmpty(sort))
            {
                // Sort logic here.
            }
            else
            {
                // Default sort here.
                tasks = tasks.OrderBy(g => g.Created);
            }

            // Size
            int totalItems = tasks.Count();
            int totalPages = 0;

            if (totalItems > 0)
            {
                totalPages = (totalItems / itemsPerPage) + 1;
            }

            // Skip, and take.
            tasks = tasks.Skip((currentPage - 1) * itemsPerPage);
            tasks = tasks.Take(itemsPerPage);

            var result = new PaginationModel<TaskModel>()
            {
                Items = tasks.ToList(),
                Sort = sort,
                Keyword = keyword,
                CurrentPage = currentPage,
                ItemsPerPage = itemsPerPage,
                TotalItems = totalItems,
                TotalPages = totalPages
            };

            return result;
        }

        /// <summary>
        /// Get specific task.
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public TaskModel? GetTask(string projectId, string id)
        {
            // Get specific task.
            var task = (from g in _db.Tasks where g.ProjectId == projectId && g.Id == id && g.IsDeleted == false select g).FirstOrDefault();

            return task;
        }

        /// <summary>
        /// Create task.
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public async Task<TaskModel?> CreateTaskAsync(TaskModel task)
        {
            // Create task

            // Make sure there is no duplicate.
            var original = (from g in _db.Tasks where g.ProjectId == task.ProjectId && (g.Id == task.Id || g.TaskNum == task.TaskNum) select g).FirstOrDefault();

            if (original != null)
            {
                return null;
            }

            // Timestamp is overwritten here.
            task.Created = DateTime.Now;

            // Get and set task num.
            var taskNum = (from n in _db.Tasks where n.ProjectId == task.ProjectId select n).Count() + 1;
            task.TaskNum = taskNum;

            _db.Tasks.Add(task);
            await _db.SaveChangesAsync();

            return task;
        }


        /// <summary>
        /// Get the biggest task num + 1. Use this number to create new task.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public int BiggestTaskNum(string projectId)
        {
            var num = (from n in _db.Tasks where n.ProjectId == projectId select n).Count();

            return (num + 1);
        }

        /// <summary>
        /// Update the task.
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public async Task<TaskModel?> UpdateTaskAsync(TaskModel task)
        {
            // Update project

            // Get
            var original = (from g in _db.Tasks where g.Id == task.Id select g).FirstOrDefault();

            if (original == null)
            {
                return original;
            }

            original.ActualTime = task.ActualTime;
            original.AssignedPerson = task.AssignedPerson;
            original.Description = task.Description;
            original.EndAt = task.EndAt;
            original.ExpectedTime = task.ExpectedTime;
            original.Files = task.Files;
            original.Mentions = task.Mentions;
            original.Name = task.Name;
            original.ParentProjectId = task.ParentProjectId;
            original.Priority = task.Priority;
            original.StartFrom = task.StartFrom;
            original.Status = task.Status;
            original.TaskType = task.TaskType;
            original.TaskApplicableVersion = task.TaskApplicableVersion;
            original.TaskCategory = task.TaskCategory;
            original.TaskMilestone = task.TaskMilestone;
            original.TimeUnit = task.TimeUnit;

            _db.Tasks.Update(original);

            await _db.SaveChangesAsync();

            return original;
        }

        /// <summary>
        /// Delete the task.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TaskModel?> DeleteTaskAsync(string id)
        {
            // Delete project, set flag to delete.

            // Get the project first.
            var task = (from g in _db.Tasks where g.Id == id select g).FirstOrDefault();

            if (task == null)
            {
                return null;
            }

            task.IsDeleted = true;
            task.Deleted = DateTime.Now;

            _db.Tasks.Update(task);
            await _db.SaveChangesAsync();

            // If you want to delete physically, uncomment the code below.
            // _db.Tasks.Remove(task);
            // await _db.SaveChangesAsync();

            return task;
        }

        /// <summary>
        /// Get categories.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public IList<TaskCategoryModel> GetCategories(string projectId)
        {
            var categories = from c in _db.TaskCategories
                             where c.ProjectId == projectId &&
                             c.IsDeleted == false
                             orderby c.DisplayOrder
                             select c;

            return categories.ToList();
        }

        /// <summary>
        /// Get category.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TaskCategoryModel? GetCategory(string id)
        {
            var taskCategory = (from t in _db.TaskCategories where t.Id == id select t).FirstOrDefault();

            return taskCategory;
        }

        /// <summary>
        /// Create category.
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public async Task<TaskCategoryModel?> CreateCategoryAsync(TaskCategoryModel category)
        {
            // Create category

            // Make sure there is no duplicate.
            var original = (from g in _db.TaskCategories where g.Id == category.Id select g).FirstOrDefault();

            if (original != null)
            {
                return null;
            }

            // Timestamp is overwritten here.
            category.Created = DateTime.Now;

            _db.TaskCategories.Add(category);
            await _db.SaveChangesAsync();

            return category;
        }

        /// <summary>
        /// Update the category.
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public async Task<TaskCategoryModel?> UpdateCategoryAsync(TaskCategoryModel category)
        {
            // Update category

            // Get the organziation to update.
            var original = (from g in _db.TaskCategories where g.Id == category.Id select g).FirstOrDefault();

            if (original == null)
            {
                return original;
            }

            original.Name = category.Name;
            original.DisplayOrder = category.DisplayOrder;

            _db.TaskCategories.Update(original);

            await _db.SaveChangesAsync();

            return original;
        }

        /// <summary>
        /// Delete the category.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TaskCategoryModel?> DeleteCategoryAsync(string id)
        {
            // Delete category, set flag to delete.

            // Get
            var category = (from g in _db.TaskCategories where g.Id == id select g).FirstOrDefault();

            if (category == null)
            {
                return null;
            }

            category.IsDeleted = true;
            category.Deleted = DateTime.Now;

            _db.TaskCategories.Update(category);
            await _db.SaveChangesAsync();

            // If you want to delete physically, uncomment the code below.
            // _db.TaskCategories.Remove(category);
            // await _db.SaveChangesAsync();

            return category;
        }

        /// <summary>
        /// Get milestones.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public IList<TaskMilestoneModel> GetMilestones(string projectId)
        {
            var mileStones = from c in _db.TaskMilestones
                             where c.ProjectId == projectId &&
                             c.IsDeleted == false
                             orderby c.DisplayOrder
                             select c;
            return mileStones.ToList();
        }

        /// <summary>
        /// Get milestone.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TaskMilestoneModel? GetMilestone(string id)
        {
            var taskMilestone = (from t in _db.TaskMilestones where t.Id == id select t).FirstOrDefault();

            return taskMilestone;
        }


        /// <summary>
        /// Create milestone.
        /// </summary>
        /// <param name="milestone"></param>
        /// <returns></returns>
        public async Task<TaskMilestoneModel?> CreateMilestonesAsync(TaskMilestoneModel milestone)
        {
            // Create milestone

            // Make sure there is no duplicate.
            var original = (from g in _db.TaskMilestones where g.Id == milestone.Id select g).FirstOrDefault();

            if (original != null)
            {
                return null;
            }

            // Timestamp is overwritten here.
            milestone.Created = DateTime.Now;

            _db.TaskMilestones.Add(milestone);
            await _db.SaveChangesAsync();

            return milestone;
        }

        /// <summary>
        /// Update milestone.
        /// </summary>
        /// <param name="milestone"></param>
        /// <returns></returns>
        public async Task<TaskMilestoneModel?> UpdateMilestonesAsync(TaskMilestoneModel milestone)
        {
            // Update milestone

            // Get the organziation to update.
            var original = (from g in _db.TaskMilestones where g.Id == milestone.Id select g).FirstOrDefault();

            if (original == null)
            {
                return original;
            }

            original.Name = milestone.Name;
            original.DisplayOrder = milestone.DisplayOrder;

            _db.TaskMilestones.Update(original);

            await _db.SaveChangesAsync();

            return original;
        }

        /// <summary>
        /// Delete milestone.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TaskMilestoneModel?> DeleteMilestonesAsync(string id)
        {
            // Delete project, set flag to delete.

            // Get
            var milestone = (from g in _db.TaskMilestones where g.Id == id select g).FirstOrDefault();

            if (milestone == null)
            {
                return null;
            }

            milestone.IsDeleted = true;
            milestone.Deleted = DateTime.Now;

            _db.TaskMilestones.Update(milestone);
            await _db.SaveChangesAsync();

            // If you want to delete physically, uncomment the code below.
            // _db.TaskMilestones.Remove(milestone);
            // await _db.SaveChangesAsync();

            return milestone;
        }

        /// <summary>
        /// Get versions.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public IList<TaskVersionModel> GetVersions(string projectId)
        {
            var versions = from c in _db.TaskVersions
                           where c.ProjectId == projectId &&
                         c.IsDeleted == false
                           orderby c.DisplayOrder
                           select c;
            return versions.ToList();
        }

        /// <summary>
        /// Get version.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TaskVersionModel? GetVersion(string id)
        {
            var taskVersion = (from t in _db.TaskVersions where t.Id == id select t).FirstOrDefault();

            return taskVersion;
        }

        /// <summary>
        /// Create version.
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public async Task<TaskVersionModel?> CreateVersionAsync(TaskVersionModel version)
        {
            // Create version

            // Make sure there is no duplicate.
            var original = (from g in _db.TaskVersions where g.Id == version.Id select g).FirstOrDefault();

            if (original != null)
            {
                return null;
            }

            // Timestamp is overwritten here.
            version.Created = DateTime.Now;

            _db.TaskVersions.Add(version);
            await _db.SaveChangesAsync();

            return version;
        }

        /// <summary>
        /// Update version.
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public async Task<TaskVersionModel?> UpdateVersionAsync(TaskVersionModel version)
        {
            // Update version

            // Get the organziation to update.
            var original = (from g in _db.TaskVersions where g.Id == version.Id select g).FirstOrDefault();

            if (original == null)
            {
                return original;
            }

            original.Name = version.Name;
            original.DisplayOrder = version.DisplayOrder;

            _db.TaskVersions.Update(original);

            await _db.SaveChangesAsync();

            return original;
        }

        /// <summary>
        /// Delete version.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TaskVersionModel?> DeleteVersionAsync(string id)
        {
            // Delete version, set flag to delete.

            // Get
            var version = (from g in _db.TaskVersions where g.Id == id select g).FirstOrDefault();

            if (version == null)
            {
                return null;
            }

            version.IsDeleted = true;
            version.Deleted = DateTime.Now;

            _db.TaskVersions.Update(version);
            await _db.SaveChangesAsync();

            // If you want to delete physically, uncomment the code below.
            // _db.TaskVersions.Remove(version);
            // await _db.SaveChangesAsync();

            return version;
        }

        /// <summary>
        /// Get statuses.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public IList<TaskStatusModel> GetStatuses(string projectId)
        {
            var status = from c in _db.TaskStatus
                         where c.ProjectId == projectId &&
                       c.IsDeleted == false
                         orderby c.DisplayOrder
                         select c;
            return status.ToList();
        }

        /// <summary>
        /// Get status.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TaskStatusModel? GetStatus(string id)
        {
            var taskStatus = (from t in _db.TaskStatus where t.Id == id select t).FirstOrDefault();

            return taskStatus;
        }

        /// <summary>
        /// Create status.
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<TaskStatusModel?> CreateStatusAsync(TaskStatusModel status)
        {
            // Create status

            // Make sure there is no duplicate.
            var original = (from g in _db.TaskStatus where g.Id == status.Id select g).FirstOrDefault();

            if (original != null)
            {
                return null;
            }

            // Timestamp is overwritten here.
            status.Created = DateTime.Now;

            _db.TaskStatus.Add(status);
            await _db.SaveChangesAsync();

            return status;
        }

        /// <summary>
        /// Update status.
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<TaskStatusModel?> UpdateStatusAsync(TaskStatusModel status)
        {
            // Update status

            // Get 
            var original = (from g in _db.TaskStatus where g.Id == status.Id select g).FirstOrDefault();

            if (original == null)
            {
                return original;
            }

            original.Name = status.Name;
            original.Color = status.Color;
            original.TextColor = status.TextColor;
            original.DisplayOrder = status.DisplayOrder;

            _db.TaskStatus.Update(original);

            await _db.SaveChangesAsync();

            return original;
        }

        /// <summary>
        /// Delete status
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TaskStatusModel?> DeleteStatusAsync(string id)
        {
            // Delete status, set flag to delete.

            // Get
            var status = (from g in _db.TaskStatus where g.Id == id select g).FirstOrDefault();

            if (status == null)
            {
                return null;
            }

            status.IsDeleted = true;
            status.Deleted = DateTime.Now;

            _db.TaskStatus.Update(status);
            await _db.SaveChangesAsync();

            // If you want to delete physically, uncomment the code below.
            // _db.TaskStatus.Remove(status);
            // await _db.SaveChangesAsync();

            return status;
        }

        /// <summary>
        /// Get task types.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public IList<TaskTypeModel> GetTaskTypes(string projectId)
        {
            var types = from c in _db.TaskTypes
                        where c.ProjectId == projectId &&
                      c.IsDeleted == false
                        orderby c.DisplayOrder
                        select c;
            return types.ToList();
        }

        /// <summary>
        /// Get task type.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TaskTypeModel? GetTaskType(string id)
        {
            var taskType = (from t in _db.TaskTypes where t.Id == id select t).FirstOrDefault();

            return taskType;
        }

        /// <summary>
        /// Create task type.
        /// </summary>
        /// <param name="taskType"></param>
        /// <returns></returns>
        public async Task<TaskTypeModel?> CreateTaskTypeAsync(TaskTypeModel taskType)
        {
            // Create project

            // Make sure there is no duplicate.
            var original = (from g in _db.TaskTypes where g.Id == taskType.Id select g).FirstOrDefault();

            if (original != null)
            {
                return null;
            }

            // Timestamp is overwritten here.
            taskType.Created = DateTime.Now;

            _db.TaskTypes.Add(taskType);
            await _db.SaveChangesAsync();

            return taskType;
        }

        /// <summary>
        /// Update task type.
        /// </summary>
        /// <param name="taskType"></param>
        /// <returns></returns>

        public async Task<TaskTypeModel?> UpdateTaskTypeAsync(TaskTypeModel taskType)
        {
            // Update project

            // Get
            var original = (from g in _db.TaskTypes where g.Id == taskType.Id select g).FirstOrDefault();

            if (original == null)
            {
                return original;
            }

            original.Name = taskType.Name;
            original.Color = taskType.Color;
            original.TextColor = taskType.TextColor;
            original.DisplayOrder = taskType.DisplayOrder;

            _db.TaskTypes.Update(original);

            await _db.SaveChangesAsync();

            return original;
        }

        /// <summary>
        /// Delete task type.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TaskTypeModel?> DeleteTaskTypeAsync(string id)
        {
            // Delete project, set flag to delete.

            // Get
            var taskType = (from g in _db.TaskTypes where g.Id == id select g).FirstOrDefault();

            if (taskType == null)
            {
                return null;
            }

            taskType.IsDeleted = true;
            taskType.Deleted = DateTime.Now;

            _db.TaskTypes.Update(taskType);
            await _db.SaveChangesAsync();

            // If you want to delete physically, uncomment the code below.
            // _db.TaskTypes.Remove(taskType);
            // await _db.SaveChangesAsync();

            return taskType;
        }

        /// <summary>
        /// Get task logs.
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public IList<TaskUpdateModel> GetTaskUpdates(string taskId)
        {
            var logs = from l in _db.TaskUpdates where l.TaskId == taskId && l.IsDeleted == false orderby l.Created select l;

            return logs.ToList();
        }

        /// <summary>
        /// Create task log.
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        public async Task<TaskUpdateModel?> CreateTaskUpdateAsync(TaskUpdateModel update)
        {
            // Make sure there is no duplicate.
            var original = (from g in _db.TaskUpdates where g.Id == update.Id && g.IsDeleted == false select g).FirstOrDefault();

            if (original != null)
            {
                return null;
            }

            update.Created = DateTime.Now;

            _db.TaskUpdates.Add(update);

            await _db.SaveChangesAsync();

            return update;
        }

        /// <summary>
        /// Update task log.
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>

        public async Task<TaskUpdateModel?> UpdateTaskUpdateAsync(TaskUpdateModel update)
        {
            // Make sure there is data
            var original = (from g in _db.TaskUpdates where g.Id == update.Id select g).FirstOrDefault();

            if (original == null)
            {
                return null;
            }

            original.ActualTime = update.ActualTime;
            original.AssinedPerson = update.AssinedPerson;
            original.Description = update.Description;
            original.EndAt = update.EndAt;
            original.ExpectedTime = update.ExpectedTime;
            original.Files = update.Files;
            original.Mentions = update.Mentions;
            original.Milestone = update.Milestone;
            original.Reason = update.Reason;
            original.StartFrom = update.StartFrom;
            original.Status = update.Status;
            original.UpdateBy = update.UpdateBy;
            original.Modified = DateTime.Now;

            _db.TaskUpdates.Update(original);

            await _db.SaveChangesAsync();

            return original;
        }

        /// <summary>
        /// Delete task log.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TaskUpdateModel?> DeleteTaskUpdateAsync(string id)
        {
            // Make sure there is data
            var original = (from g in _db.TaskUpdates where g.Id == id select g).FirstOrDefault();

            if (original == null)
            {
                return null;
            }

            original.IsDeleted = true;
            original.Deleted = DateTime.Now;

            _db.TaskUpdates.Update(original);

            await _db.SaveChangesAsync();

            return original;
        }
    }
}
