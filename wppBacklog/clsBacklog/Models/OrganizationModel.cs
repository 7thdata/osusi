using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clsBacklog.Models
{
    [Table("Organizations")]
    public class OrganizationModel : SqlDbBaseModel
    {
        public OrganizationModel(string id, string name, string permaName)
        {
            Id = id;
            Name = name;
            PermaName = permaName;
        }

        [Key, MaxLength(64)]
        public string Id { get; set; }

        [MaxLength(8)]
        public string PermaName { get; set; }

        [MaxLength(64), Required]
        public string Name { get; set; }

        [MaxLength(64)]
        public string? BillingName { get; set; }

        [MaxLength(3)]
        public string? BillingAddressCountry { get; set; }
        [MaxLength(12)]
        public string? BillingAddressPostalCode { get; set; }
        public string? BillingAddressRegion { get; set; }
        public string? BillingAddressLocality { get; set; }
        public string? BillingAddressStreet { get; set; }
        public string? BillingAddressUnit { get; set; }

        [MaxLength(64)]
        public string? Subscription { get; set; }
        public DateTime CurrentSubscriptionExpires { get; set; }
    }

    public class OrganizationViewModel
    {
        public OrganizationViewModel(OrganizationModel organization, OrganizationMemberModel memberAs)
        {
            Organization = organization;
            MemberAs = memberAs;
        }
        public OrganizationModel Organization { get; set; }
        public OrganizationMemberModel MemberAs { get; set; }
    }

    [Table("OrganizationMembers")]
    public class OrganizationMemberModel : SqlDbBaseModel
    {
        public OrganizationMemberModel(string id, string userId, string organizationId, string membershipType)
        {
            Id = id;
            UserId = userId;
            OrganizationId = organizationId;
            MembershipType = membershipType;
        }

        [Key, MaxLength(64)]
        public string Id { get; set; }
        [MaxLength(64), Required]
        public string UserId { get; set; }
        [MaxLength(64), Required]
        public string OrganizationId { get; set; }
        [MaxLength(64), Required]
        public string MembershipType { get; set; }
    }

    public class OrganizationMemberViewModel
    {
        public OrganizationMemberViewModel(string id, UserModel? user,
            OrganizationModel? organization, string membershipType)
        {
            Id = id;
            User = user;
            Organization = organization;
            MembershipType = membershipType;
        }
        public string Id { get; set; }
        public UserModel? User { get; set; }
        public OrganizationModel? Organization { get; set; }
        public string MembershipType { get; set; }
        public DateTime Created { get; set; }
    }
}
