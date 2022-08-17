using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clsBacklog.Models
{
    [Table("Projects")]
    public class ProjectModel : SqlDbBaseModel
    {
        public ProjectModel(string id, string permaName, string name, string ownerId)
        {
            Id = id;
            PermaName = permaName;
            Name = name;
            OwnerId = ownerId;
        }

        [Key, MaxLength(64)]
        public string Id { get; set; }

        [MaxLength(8), Required]
        public string PermaName { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsFavorite { get; set; }
        public string OwnerId { get; set; }
    }

    public class ProjectViewModel
    {
        public ProjectViewModel(ProjectModel project, OrganizationModel organization)
        {
            Project = project;
            Organization = organization;
        }

        public ProjectModel Project { get; set; }
        public OrganizationModel? Organization { get; set; }

    }

    [Table("ProjectFavorites")]
    public class MyFavoriteProjectModel : SqlDbBaseModel
    {
        public MyFavoriteProjectModel(string id, string projectId, string ownerId)
        {
            Id = id;
            ProjectId = projectId;
            OwnerId = ownerId;
        }

        [Key, MaxLength(64)]
        public string Id { get; set; }
        [MaxLength(64), Required]
        public string ProjectId { get; set; }
    }

    [Table("ProjectMembers")]
    public class ProjectMemberModel : SqlDbBaseModel
    {
        public ProjectMemberModel(string id, string projectId, string userId, string membershipType)
        {
            Id = id;
            ProjectId = projectId;
            UserId = userId;
            MembershipType = membershipType;
        }

        [Key, MaxLength(64)]
        public string Id { get; set; }
        [MaxLength(64), Required]
        public string ProjectId { get; set; }
        [MaxLength(64), Required]
        public string UserId { get; set; }
        [MaxLength(8), Required]
        public string MembershipType { get; set; }
    }

    public class ProjectMemberViewModel
    {
        public ProjectMemberViewModel(string id, string projectId, UserModel? user, string membershipType)
        {
            Id = id;
            ProjectId = projectId;
            User = user;
            MembershipType = membershipType;
        }

        public string Id { get; set; }

        public string ProjectId { get; set; }

        public UserModel? User { get; set; }

        public string MembershipType { get; set; }
        public DateTime Created { get; set; }
    }

    [Table("ProjectNews")]
    public class ProjectNewsModel : SqlDbBaseModel
    {
        public ProjectNewsModel(string id, DateTime timestamp, string title, string description)
        {
            Id = id;
            Timestamp = timestamp;
            Title = title;
            Description = description;
        }

        [Key, MaxLength(64)]
        public string Id { get; set; }
        public DateTime Timestamp { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public string? Link { get; set; }
        public bool IsTop { get; set; } = false;
    }
}
