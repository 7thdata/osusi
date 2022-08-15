using clsBacklog.Data;
using clsBacklog.Interfaces;
using clsBacklog.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clsBacklog.Services
{
    public class OrganizationServices : IOrganizationServices
    {
        private readonly IdentityContext _db;

        public OrganizationServices(IdentityContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Create organization.
        /// </summary>
        /// <param name="organization"></param>
        /// <returns></returns>
        public async Task<OrganizationModel?> CreateOrganizationAsync(OrganizationModel organization)
        {
            // Create organization

            // Make sure there is no duplicate.
            var original = (from g in _db.Organizations where g.Id == organization.Id select g).FirstOrDefault();

            if (original != null)
            {
                return null;
            }

            // Timestamp is overwritten here.
            organization.Created = DateTime.Now;

            _db.Organizations.Add(organization);
            await _db.SaveChangesAsync();

            return organization;
        }

        /// <summary>
        /// Update specific organization.
        /// </summary>
        /// <param name="organization"></param>
        /// <returns></returns>
        public async Task<OrganizationModel?> UpdateOrganizationAsync(OrganizationModel organization)
        {
            // Update organization

            // Get the organziation to update.
            var original = (from g in _db.Organizations where g.Id == organization.Id select g).FirstOrDefault();

            if (original == null)
            {
                return original;
            }

            original.Name = organization.Name;
            original.Subscription = organization.Subscription;
            original.CurrentSubscriptionExpires = organization.CurrentSubscriptionExpires;

            original.BillingAddressCountry = organization.BillingAddressCountry;
            original.BillingAddressPostalCode = organization.BillingAddressPostalCode;
            original.BillingAddressRegion = organization.BillingAddressRegion;
            original.BillingAddressLocality = organization.BillingAddressLocality;
            original.BillingAddressStreet = organization.BillingAddressStreet;
            original.BillingAddressUnit = organization.BillingAddressUnit;

            _db.Organizations.Update(original);

            await _db.SaveChangesAsync();

            return original;
        }

        /// <summary>
        /// Get specific organization.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public OrganizationModel? GetOrganization(string id)
        {
            // Get specific organization.
            var organization = (from g in _db.Organizations where g.Id == id && g.IsDeleted == false select g).FirstOrDefault();

            return organization;
        }

        /// <summary>
        /// Get organizations.
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="sort"></param>
        /// <param name="currentPage"></param>
        /// <param name="itemsPerPage"></param>
        /// <returns></returns>
        public PaginationModel<OrganizationModel> GetOrganizations(string? keyword,
            string? sort, int currentPage, int itemsPerPage)
        {
            // Search for specific organizations

            var organizations = from g in _db.Organizations where g.IsDeleted == false select g;

            if (!string.IsNullOrEmpty(keyword))
            {
                // Search logic here.
                organizations = organizations.Where(g => g.Id.Contains(keyword) || g.Name.Contains(keyword));
            }

            if (!string.IsNullOrEmpty(sort))
            {
                // Sort logic here.
            }
            else
            {
                // Default sort here.
                organizations = organizations.OrderBy(g => g.Name);
            }

            // Size
            int totalItems = organizations.Count();
            int totalPages = 0;

            if (totalItems > 0)
            {
                totalPages = (totalItems / itemsPerPage) + 1;
            }

            // Skip, and take.
            organizations = organizations.Skip((currentPage - 1) * itemsPerPage);
            organizations = organizations.Take(itemsPerPage);

            var result = new PaginationModel<OrganizationModel>()
            {
                Items = organizations.ToList(),
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
        /// Delete organization.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<OrganizationModel?> DeleteOrganizationAsync(string id)
        {

            // Delete organization, set flag to delete.

            // Get the organization first.
            var organization = (from g in _db.Organizations where g.Id == id select g).FirstOrDefault();

            if (organization == null)
            {
                return null;
            }

            organization.IsDeleted = true;
            organization.Deleted = DateTime.Now;

            _db.Organizations.Update(organization);
            await _db.SaveChangesAsync();

            // If you want to delete physically, uncomment the code below.
            // _db.Organizations.Remove(organization);
            // await _db.SaveChangesAsync();

            return organization;
        }

        /// <summary>
        /// Get membership of the organization.
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="userId"></param>
        /// <param name="sort"></param>
        /// <param name="currentPage"></param>
        /// <param name="itemsPerPage"></param>
        /// <returns></returns>
        public PaginationModel<OrganizationMemberModel> GetMembershipInformationByOrganizationId(string organizationId, string? userId, string sort, int currentPage, int itemsPerPage)
        {
            // Get members of the organization.

            var memberships = from m in _db.OrganizationMembers where m.OrganizationId == organizationId && m.IsDeleted == false select m;

            if (!string.IsNullOrEmpty(userId))
            {
                // If user id is specified.
                memberships = memberships.Where(m => m.UserId == userId);
            }

            if (!string.IsNullOrEmpty(sort))
            {
                // Sort logic here.
            }
            else
            {
                // Default sort here.
                memberships = memberships.OrderBy(m => m.Created);
            }

            // Size
            int totalItems = memberships.Count();
            int totalPages = 0;

            if (totalItems > 0)
            {
                totalPages = (totalItems / itemsPerPage) + 1;
            }

            // Skip, and take.
            memberships = memberships.Skip((currentPage - 1) * itemsPerPage);
            memberships = memberships.Take(itemsPerPage);

            var result = new PaginationModel<OrganizationMemberModel>
            {
                Items = memberships.ToList(),
                Sort = sort,
                Keyword = userId,
                CurrentPage = currentPage,
                ItemsPerPage = itemsPerPage,
                TotalItems = totalItems,
                TotalPages = totalPages
            };

            return result;
        }

        /// <summary>
        /// Get membership of the organization (with user info and org info).
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="userId"></param>
        /// <param name="sort"></param>
        /// <param name="currentPage"></param>
        /// <param name="itemsPerPage"></param>
        /// <returns></returns>
        public PaginationModel<OrganizationMemberViewModel> GetMembershipInformationByOrganizationIdView(string organizationId, string? userId, string sort, int currentPage, int itemsPerPage)
        {
            // Get members of the organization.

            var memberships = from m in _db.OrganizationMembers
                              where m.OrganizationId == organizationId && m.IsDeleted == false
                              select new OrganizationMemberViewModel(m.Id,
                              (from u in _db.Users where u.Id == m.UserId select u).FirstOrDefault(),
                              (from o in _db.Organizations where o.Id == m.OrganizationId select o).FirstOrDefault(),
                              m.MembershipType
                              )
                              {
                                  Created = m.Created
                              };

            if (!string.IsNullOrEmpty(userId))
            {
                // If user id is specified.

            }

            if (!string.IsNullOrEmpty(sort))
            {
                // Sort logic here.
            }
            else
            {
                // Default sort here.
                memberships = memberships.OrderBy(m => m.Created);
            }

            // Size
            int totalItems = memberships.Count();
            int totalPages = 0;

            if (totalItems > 0)
            {
                totalPages = (totalItems / itemsPerPage) + 1;
            }

            // Skip, and take.
            memberships = memberships.Skip((currentPage - 1) * itemsPerPage);
            memberships = memberships.Take(itemsPerPage);

            var result = new PaginationModel<OrganizationMemberViewModel>
            {
                Items = memberships.ToList(),
                Sort = sort,
                Keyword = userId,
                CurrentPage = currentPage,
                ItemsPerPage = itemsPerPage,
                TotalItems = totalItems,
                TotalPages = totalPages
            };

            return result;
        }

        /// <summary>
        /// Get all members of organization with information.
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="userId"></param>
        /// <param name="sort"></param>
        /// <param name="currentPage"></param>
        /// <param name="itemsPerPage"></param>
        /// <returns></returns>
        public IList<OrganizationMemberViewModel> GetMembershipInformationByOrganizationIdFullListView(string organizationId, string sort)
        {
            // Get members of the organization.

            var memberships = from m in _db.OrganizationMembers
                              where m.OrganizationId == organizationId && m.IsDeleted == false
                              select new OrganizationMemberViewModel(m.Id,
                              (from u in _db.Users where u.Id == m.UserId select u).FirstOrDefault(),
                              (from o in _db.Organizations where o.Id == m.OrganizationId select o).FirstOrDefault(),
                              m.MembershipType
                              )
                              {
                                  Created = m.Created
                              };

            if (!string.IsNullOrEmpty(sort))
            {
                // Sort logic here.
            }
            else
            {
                // Default sort here.
                memberships = memberships.OrderBy(m => m.Created);
            }

            var result = memberships.ToList();

            return result;
        }


        /// <summary>
        /// Get membership of the user.
        /// </summary>
        /// <remarks>
        /// Usually just 1.
        /// </remarks>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IList<OrganizationMemberModel> GetMembershipInformationByUserId(string userId)
        {
            // Get membership of the user. 
            var memberships = from m in _db.OrganizationMembers where m.UserId == userId && m.IsDeleted == false select m;

            // Usually just 1, but returing list just in case.
            return memberships.ToList();
        }

        /// <summary>
        /// Add member to the organization.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="organizationId"></param>
        /// <param name="membershipType"></param>
        /// <returns></returns>
        public async Task<OrganizationMemberModel?> AddMemberToOrganizationAsync(string userId, string organizationId, string membershipType)
        {
            // Make sure he/she isn't in it already.
            var membership = (from m in _db.OrganizationMembers
                              where m.OrganizationId == organizationId
                              && m.UserId == userId && m.IsDeleted == false
                              select m).FirstOrDefault();

            if (membership != null)
            {
                return membership;
            }

            // I want to make sure userId and organizationId exist and valid.
            var user = (from u in _db.Users
                        where u.IsDeleted == false && u.IsSuspended == false &&
                        u.Id == userId
                        select u).FirstOrDefault();

            if (user == null)
            {
                return null;
            }

            var organization = (from o in _db.Organizations
                                where o.IsDeleted == false &&
                               o.Id == organizationId
                                select o).FirstOrDefault();
            if (organization == null)
            {
                return null;
            }

            var newMembership = new OrganizationMemberModel(Guid.NewGuid().ToString(), userId, organizationId, membershipType)
            {
                Created = DateTime.Now
            };

            _db.OrganizationMembers.Add(newMembership);

            await _db.SaveChangesAsync();

            return newMembership;
        }

        /// <summary>
        /// Remove member from the organization.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<OrganizationMemberModel?> RemoveMemberFromOrganizationAsync(string id)
        {
            // Get the membership entity
            var membership = (from m in _db.OrganizationMembers where m.Id == id select m).FirstOrDefault();

            if (membership == null)
            {
                return null;
            }

            membership.IsDeleted = true;
            membership.Deleted = DateTime.Now;

            _db.OrganizationMembers.Update(membership);
            await _db.SaveChangesAsync();

            return membership;
        }

        /// <summary>
        /// Update membership type of the membersihp.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="membershipType"></param>
        /// <returns></returns>
        public async Task<OrganizationMemberModel?> UpdateMembershipTypeAsync(string id, string membershipType)
        {
            // Get the membership entity
            var membership = (from m in _db.OrganizationMembers where m.Id == id select m).FirstOrDefault();

            if (membership == null)
            {
                return null;
            }

            membership.MembershipType = membershipType;

            _db.OrganizationMembers.Update(membership);
            await _db.SaveChangesAsync();

            return membership;
        }

        /// <summary>
        /// Leave organization
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<UserModel?> LeaveOrganizationAsync(string organizationId, string userId)
        {
            // Get user
            var user = (from u in _db.Users where u.Id == userId select u).FirstOrDefault();

            if (user == null)
            {
                return null;
            }

            // Make sure the org id is same
            if (user.OrganizationId == organizationId)
            {
                user.OrganizationId = "";
                
                _db.Users.Update(user);

                await _db.SaveChangesAsync();
            }

            return null;

        }
    }
}
