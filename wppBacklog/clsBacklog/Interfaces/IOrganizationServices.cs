using clsBacklog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clsBacklog.Interfaces
{
    public interface IOrganizationServices
    {
        /// <summary>
        /// Create organization
        /// </summary>
        /// <param name="organization"></param>
        /// <returns></returns>
        Task<OrganizationModel?> CreateOrganizationAsync(OrganizationModel organization);

        /// <summary>
        /// Update organization.
        /// </summary>
        /// <param name="organization"></param>
        /// <returns></returns>
        Task<OrganizationModel?> UpdateOrganizationAsync(OrganizationModel organization);

        /// <summary>
        /// Get organization.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        OrganizationModel? GetOrganization(string id);

        /// <summary>
        /// Get organizations.
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="sort"></param>
        /// <param name="currentPage"></param>
        /// <param name="itemsPerPage"></param>
        /// <returns></returns>
        PaginationModel<OrganizationModel> GetOrganizations(string? keyword,
            string? sort, int currentPage, int itemsPerPage);

        /// <summary>
        /// Delete organizaiton.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<OrganizationModel?> DeleteOrganizationAsync(string id);

        /// <summary>
        /// Get memberships of the organization.
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="userId"></param>
        /// <param name="sort"></param>
        /// <param name="currentPage"></param>
        /// <param name="itemsPerPage"></param>
        /// <returns></returns>
        PaginationModel<OrganizationMemberModel> GetMembershipInformationByOrganizationId(string organizationId, string? userId, string sort, int currentPage, int itemsPerPage);

        /// <summary>
        /// Get membership of the organization (with user info and org info).
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="userId"></param>
        /// <param name="sort"></param>
        /// <param name="currentPage"></param>
        /// <param name="itemsPerPage"></param>
        /// <returns></returns>
        PaginationModel<OrganizationMemberViewModel> GetMembershipInformationByOrganizationIdView(string organizationId, string? userId, string sort, int currentPage, int itemsPerPage);

        /// <summary>
        /// Get all members of organization with information.
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        IList<OrganizationMemberViewModel> GetMembershipInformationByOrganizationIdFullListView(string organizationId, string sort);

        /// <summary>
        /// Get memberships of the user. (usually just 1)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IList<OrganizationMemberModel> GetMembershipInformationByUserId(string userId);
        
        /// <summary>
        /// Add member to the organization.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="organizationId"></param>
        /// <param name="membershipType"></param>
        /// <returns></returns>
        Task<OrganizationMemberModel?> AddMemberToOrganizationAsync(string userId, string organizationId, string membershipType);
        
        /// <summary>
        /// Remove user from the organization.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<OrganizationMemberModel?> RemoveMemberFromOrganizationAsync(string id);
        
        /// <summary>
        /// Update memebership type.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="membershipType"></param>
        /// <returns></returns>
        Task<OrganizationMemberModel?> UpdateMembershipTypeAsync(string id, string membershipType);

        /// <summary>
        /// Leave organization.
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<UserModel?> LeaveOrganizationAsync(string organizationId, string userId);
    }
}
