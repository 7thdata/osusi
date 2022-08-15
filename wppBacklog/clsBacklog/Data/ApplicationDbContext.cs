using clsBacklog.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clsBacklog.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
       : base(options)
        {
        }

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

        // Wiki
        public DbSet<WikiModel> Wikis { get; set; }
        public DbSet<WikiOldModel> WikiOlds { get; set; }

        // File
        public DbSet<FileModel> Files { get; set; }
        public DbSet<FileChangeLogModel> FileChangeLogs { get; set; }
    }
}
