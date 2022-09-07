using clsBacklog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clsBacklog.Interfaces
{
    public interface IProjectServices
    {
        /// <summary>
        /// Get projects.
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="sort"></param>
        /// <param name="currentPage"></param>
        /// <param name="itemsPerPage"></param>
        /// <returns></returns>
        PaginationModel<ProjectModel> GetProjects(string keyword, string sort, int currentPage, int itemsPerPage);


        /// <summary>
        /// Get project view for the user.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="keyword"></param>
        /// <param name="sort"></param>
        /// <param name="currentPage"></param>
        /// <param name="itemsPerPage"></param>
        /// <returns></returns>
        PaginationModel<ProjectViewModel> GetProjectsView(string userId, string keyword, string sort, int currentPage, int itemsPerPage);

        /// <summary>
        /// Get project.
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        ProjectModel? GetProject(string organizationId, string id);

        /// <summary>
        /// Create project.
        /// </summary>
        /// <param name="project"></param>
        /// <param name="userId">The user who is creating this project.</param>
        /// <returns></returns>
        Task<ProjectModel?> CreateProjectAsync(ProjectModel project, string userId);

        /// <summary>
        /// Check to see if perma name is unique.
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="permaName"></param>
        /// <returns></returns>
        bool IsPermaNameUnique(string accountId, string permaName);
        /// <summary>
        /// Update project.
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        Task<ProjectModel?> UpdateProjectAsync(ProjectModel project);

        /// <summary>
        /// Delete project.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProjectModel?> DeleteProjectAsync(string id);

        /// <summary>
        /// Get project members.
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="userId"></param>
        /// <param name="sort"></param>
        /// <param name="currentPage"></param>
        /// <param name="itemsPerPage"></param>
        /// <returns></returns>
        PaginationModel<ProjectMemberModel> GetProjectMembers(string projectId, string userId, string sort, int currentPage, int itemsPerPage);

        /// <summary>
        /// Get project member (with member information inside)
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="userId"></param>
        /// <param name="sort"></param>
        /// <param name="currentPage"></param>
        /// <param name="itemsPerPage"></param>
        /// <returns></returns>
        PaginationModel<ProjectMemberViewModel> GetProjectMembersView(string organizationId, string projectId, string userId, string sort, int currentPage, int itemsPerPage);

        /// <summary>
        /// Get project member.
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="projectId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        ProjectMemberViewModel? GetProjectMemberViewByUid(string organizationId, string projectId, string userId);

        /// <summary>
        /// Get project members in list.
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        IList<ProjectMemberViewModel> GetProjectMembersViewInList(string organizationId, string projectId);

        /// <summary>
        /// Add member to the project.
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="userId"></param>
        /// <param name="membershipType"></param>
        /// <returns></returns>
        Task<ProjectMemberModel?> AddMemberToProjectAsync(string projectId, string userId, string membershipType);

        /// <summary>
        /// Remove member from the project.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProjectMemberModel?> RemoveMemberFromProjectAsync(string id);

        /// <summary>
        /// Update project member type.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="membershipType"></param>
        /// <returns></returns>
        Task<ProjectMemberModel?> UpdateProjectMemberTypeAsync(string id, string membershipType);

    }
}
