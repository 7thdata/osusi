using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace clsBacklog.Models;

// Add profile data for application users by adding properties to the UserModel class
public class UserModel : IdentityUser
{
    public UserModel(string name, string preferedLanguage)
    {
        Name = name;
        PreferedLanguage = preferedLanguage;
    }

    [MaxLength(128)]
    public string Name { get; set; }

    public string? ProfileImage { get; set; }

    [MaxLength(64)]
    public string? OrganizationId { get; set; }
    public DateTime LastLogin { get; set; }
    public DateTime Created { get; set; }
    public bool IsSuspended { get; set; }
    public DateTime Suspended { get; set; }
    [MaxLength(3)]
    public string PreferedLanguage { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime Deleted { get; set; }
    public string? LastProjectId { get; set; }
 
}

