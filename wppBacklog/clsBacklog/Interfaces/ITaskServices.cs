using clsBacklog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clsBacklog.Interfaces
{
    public interface ITaskServices
    {
        /// <summary>
        /// Get tasks.
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="keyword"></param>
        /// <param name="sort"></param>
        /// <param name="currentPage"></param>
        /// <param name="itemsPerPage"></param>
        /// <returns></returns>
        PaginationModel<TaskModel> GetTasks(string projectId, string keyword, string sort, int currentPage, int itemsPerPage);

        /// <summary>
        /// Get task.
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        TaskModel? GetTask(string projectId, string id);

        /// <summary>
        /// Create task.
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        Task<TaskModel?> CreateTaskAsync(TaskModel task);

        /// <summary>
        /// Get the biggest number.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        int BiggestTaskNum(string projectId);

        /// <summary>
        /// Update task.
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        Task<TaskModel?> UpdateTaskAsync(TaskModel task);

        /// <summary>
        /// Delete task.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TaskModel?> DeleteTaskAsync(string id);

        /// <summary>
        /// Get categories.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        IList<TaskCategoryModel> GetCategories(string projectId);

        /// <summary>
        /// Get category.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TaskCategoryModel? GetCategory(string id);

        /// <summary>
        /// Create category.
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        Task<TaskCategoryModel?> CreateCategoryAsync(TaskCategoryModel category);

        /// <summary>
        /// Update category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        Task<TaskCategoryModel?> UpdateCategoryAsync(TaskCategoryModel category);

        /// <summary>
        /// Delete category
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TaskCategoryModel?> DeleteCategoryAsync(string id);

        /// <summary>
        /// Get milestones.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        IList<TaskMilestoneModel> GetMilestones(string projectId);

        /// <summary>
        /// Get milestone.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TaskMilestoneModel? GetMilestone(string id);

        /// <summary>
        /// Create milestone.
        /// </summary>
        /// <param name="milestone"></param>
        /// <returns></returns>
        Task<TaskMilestoneModel?> CreateMilestonesAsync(TaskMilestoneModel milestone);

        /// <summary>
        /// Update milestone.
        /// </summary>
        /// <param name="milestone"></param>
        /// <returns></returns>
        Task<TaskMilestoneModel?> UpdateMilestonesAsync(TaskMilestoneModel milestone);

        /// <summary>
        /// Delete milestone.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TaskMilestoneModel?> DeleteMilestonesAsync(string id);

        /// <summary>
        /// Get versions.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        IList<TaskVersionModel> GetVersions(string projectId);

        /// <summary>
        /// Get version.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TaskVersionModel? GetVersion(string id);

        /// <summary>
        /// Create version.
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        Task<TaskVersionModel?> CreateVersionAsync(TaskVersionModel version);

        /// <summary>
        /// Update version.
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        Task<TaskVersionModel?> UpdateVersionAsync(TaskVersionModel version);

        /// <summary>
        /// Delete version.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TaskVersionModel?> DeleteVersionAsync(string id);

        /// <summary>
        /// Get status.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        IList<TaskStatusModel> GetStatuses(string projectId);

        /// <summary>
        /// Get a status.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TaskStatusModel? GetStatus(string id);

        /// <summary>
        /// Create status.
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        Task<TaskStatusModel?> CreateStatusAsync(TaskStatusModel status);

        /// <summary>
        /// Update status.
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        Task<TaskStatusModel?> UpdateStatusAsync(TaskStatusModel status);

        /// <summary>
        /// Delete status.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TaskStatusModel?> DeleteStatusAsync(string id);

        /// <summary>
        /// Get task type.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        IList<TaskTypeModel> GetTaskTypes(string projectId);

        /// <summary>
        /// Get task type.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TaskTypeModel? GetTaskType(string id);

        /// <summary>
        /// Create task type.
        /// </summary>
        /// <param name="taskType"></param>
        /// <returns></returns>
        Task<TaskTypeModel?> CreateTaskTypeAsync(TaskTypeModel taskType);

        /// <summary>
        /// Update task type.
        /// </summary>
        /// <param name="taskType"></param>
        /// <returns></returns>
        Task<TaskTypeModel?> UpdateTaskTypeAsync(TaskTypeModel taskType);

        /// <summary>
        /// Delete task type.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TaskTypeModel?> DeleteTaskTypeAsync(string id);

        /// <summary>
        /// Get task update logs.
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        IList<TaskUpdateModel> GetTaskUpdates(string taskId);

        /// <summary>
        /// Create task update log.
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        Task<TaskUpdateModel?> CreateTaskUpdateAsync(TaskUpdateModel update);

        /// <summary>
        /// Update task update log.
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        Task<TaskUpdateModel?> UpdateTaskUpdateAsync(TaskUpdateModel update);

        /// <summary>
        /// Delete task update log.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TaskUpdateModel?> DeleteTaskUpdateAsync(string id);
    }
}
