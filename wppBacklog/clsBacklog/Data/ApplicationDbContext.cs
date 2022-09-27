using clsBacklog.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace clsBacklog.Data;

public class ApplicationDbContext : IdentityDbContext<UserModel>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // Org.
    public DbSet<OrganizationModel> Organizations { get; set; }
    public DbSet<OrganizationMemberModel> OrganizationMembers { get; set; }

    // Subscription
    public DbSet<SubscriptionModel> Subscriptions { get; set; }
    public DbSet<SubscriptionLogModel> SubscriptionLogs { get; set; }

    // Project
    public DbSet<ProjectModel> Projects { get; set; }
    public DbSet<MyFavoriteProjectModel> FavoriteProjects { get; set; }
    public DbSet<ProjectMemberModel> ProjectMembers { get; set; }
    public DbSet<ProjectNewsModel> ProjectNews { get; set; }

    // Tasks
    public DbSet<TaskModel> Tasks { get; set; }
    public DbSet<TaskUpdateModel> TaskUpdates { get; set; }
    public DbSet<TaskTypeModel> TaskTypes { get; set; }
    public DbSet<TaskStatusModel> TaskStatus { get; set; }
    public DbSet<TaskCategoryModel> TaskCategories { get; set; }
    public DbSet<TaskMilestoneModel> TaskMilestones { get; set; }
    public DbSet<TaskVersionModel> TaskVersions { get; set; }
    public DbSet<TaskCompletionReasonModel> TaskCompletionReasons { get; set; }

    // Wiki
    public DbSet<WikiModel> Wikis { get; set; }
    public DbSet<WikiOldModel> WikiOlds { get; set; }

    // File
    public DbSet<FileModel> Files { get; set; }
    public DbSet<FileChangeLogModel> FileChangeLogs { get; set; }
}
