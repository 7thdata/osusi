using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clsBacklog.Models
{
    [Table("Tasks")]
    public class TaskModel : SqlDbBaseModel
    {
        public TaskModel(string projectId, string id, string name, string taskType, string? createdBy)
        {
            ProjectId = projectId;
            Id = id;
            Name = name;
            TaskType = taskType;
            CreatedBy = createdBy;
        }

        [MaxLength(64), Required]
        public string ProjectId { get; set; }

        [Key, MaxLength(64)]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [MaxLength(64)]
        public string TaskType { get; set; }
        public int TaskNum { get; set; }
        public string? ParentProjectId { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        
        public int Priority { get; set; }
        public string? AssignedPerson { get; set; }
        public string? TaskCategory { get; set; }
        public string? TaskMilestone { get; set; }
        public string? TaskApplicableVersion { get; set; }
        public DateTime? StartFrom { get; set; }
        public DateTime? EndAt { get; set; }
        public int? ExpectedTime { get; set; }
        public int? ActualTime { get; set; }
        public string? TimeUnit { get; set; }
        public string? Mentions { get; set; }
        public string? Files { get; set; }
        public string? CreatedBy { get; set; }
        public string? TaskCompletionReason { get; set; }

    }

    public class TaskViewModel 
    {
        public TaskViewModel(ProjectModel project,string id, int taskNum, string name,
            TaskTypeModel taskType)
        {
            Project = project;
            Id = id;
            TaskNum = taskNum;
            Name = name;
            TaskType = taskType;
        }

        public ProjectModel Project { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public TaskTypeModel TaskType { get; set; }
        public int TaskNum { get; set; }
        public ProjectModel? ParentProjectId { get; set; }
        public string? Description { get; set; }
        public TaskStatusModel? TaskStatus { get; set; }
        public string? TaskStatusId { get; set; }
        public int Priority { get; set; }
        public ProjectMemberViewModel? AssignedPerson { get; set; }
        public TaskCategoryModel? TaskCategory { get; set; }
        public TaskMilestoneModel? TaskMilestone { get; set; }
        public TaskVersionModel? TaskApplicableVersion { get; set; }
        public DateTime? StartFrom { get; set; }
        public DateTime? EndAt { get; set; }
        public int? ExpectedTime { get; set; }
        public int? ActualTime { get; set; }
        public string? TimeUnit { get; set; }
        public string? Mentions { get; set; }
        public string? Files { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

    }

    [Table("TaskTypes")]
    public class TaskTypeModel : SqlDbBaseModel
    {
        public TaskTypeModel(string projectId, string id, string name, int displayOrder, string color, string textColor)
        {
            ProjectId = projectId;
            Id = id;
            Name = name;
            DisplayOrder = displayOrder;
            Color = color;
            TextColor = textColor;
        }
        [MaxLength(64), Required]
        public string ProjectId { get; set; }
        [Key, MaxLength(64)]
        public string Id { get; set; }
        [Required]
        public string Color { get; set; }
        [Required]
        public string TextColor { get; set; }

        [Required]
        public string Name { get; set; }
        public int DisplayOrder { get; set; }
    }

    [Table("TaskStatus")]
    public class TaskStatusModel : SqlDbBaseModel
    {
        public TaskStatusModel(string projectId, string id, string name, int displayOrder, string color, string textColor)
        {
            ProjectId = projectId;
            Id = id;
            Name = name;
            DisplayOrder = displayOrder;
            Color = color;
            TextColor = textColor;
        }
        [MaxLength(64), Required]
        public string ProjectId { get; set; }
        [Key, MaxLength(64)]
        public string Id { get; set; }
        [Required]
        public string Color { get; set; }
        [Required]
        public string TextColor { get; set; }

        [Required]
        public string Name { get; set; }
        public int DisplayOrder { get; set; }
    }

    [Table("TaskVersions")]
    public class TaskVersionModel : SqlDbBaseModel
    {
        public TaskVersionModel(string projectId, string id, string name, int displayOrder)
        {
            ProjectId = projectId;
            Id = id;
            Name = name;
            DisplayOrder = displayOrder;
        }
        [MaxLength(64), Required]
        public string ProjectId { get; set; }
        [Key, MaxLength(64)]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int DisplayOrder { get; set; }
    }

    [Table("TaskCategory")]
    public class TaskCategoryModel : SqlDbBaseModel
    {
        public TaskCategoryModel(string projectId, string id, string name, int displayOrder)
        {
            ProjectId = projectId;
            Id = id;
            Name = name;
            DisplayOrder = displayOrder;
        }
        [MaxLength(64), Required]
        public string ProjectId { get; set; }
        [Key, MaxLength(64)]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int DisplayOrder { get; set; }
    }

    [Table("TaskMilestones")]
    public class TaskMilestoneModel : SqlDbBaseModel
    {
        public TaskMilestoneModel(string projectId, string id, string name, int displayOrder)
        {
            ProjectId = projectId;
            Id = id;
            Name = name;
            DisplayOrder = displayOrder;
        }
        [MaxLength(64), Required]
        public string ProjectId { get; set; }
        [Key, MaxLength(64)]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int DisplayOrder { get; set; }
    }

    [Table("TaskCompletionReasons")]
    public class TaskCompletionReasonModel : SqlDbBaseModel
    {
        public TaskCompletionReasonModel(string projectId, string id, string name, int displayOrder)
        {
            ProjectId = projectId;
            Id = id;
            Name = name;
            DisplayOrder = displayOrder;
        }
        [MaxLength(64), Required]
        public string ProjectId { get; set; }
        [Key, MaxLength(64)]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int DisplayOrder { get; set; }
    }

    [Table("TaskUpdates")]
    public class TaskUpdateModel : SqlDbBaseModel
    {
        public TaskUpdateModel(string projectId, string taskId, string id, string description, string updateBy)
        {
            ProjectId = projectId;
            TaskId = taskId;
            Id = id;
            Description = description;
            UpdateBy = updateBy;
        }

        [MaxLength(64), Required]
        public string ProjectId { get; set; }
        [MaxLength(64), Required]
        public string TaskId { get; set; }
        [Key, MaxLength(64)]
        public string Id { get; set; }

        public string? Status { get; set; }
        public string? AssinedPerson { get; set; }
        public string? Milestone { get; set; }
        public string Description { get; set; }
        public string? Reason { get; set; }

        public DateTime? StartFrom { get; set; }
        public DateTime? EndAt { get; set; }
        public int? ExpectedTime { get; set; }
        public int? ActualTime { get; set; }

        [MaxLength(64), Required]
        public string UpdateBy { get; set; }
        public string? Mentions { get; set; }
        public string? Files { get; set; }
    }
}
