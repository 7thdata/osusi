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
        public TaskModel(string projectId, string id, string name, int taskNum, string taskType)
        {
            ProjectId = projectId;
            Id = id;
            Name = name;
            TaskNum = taskNum;
            TaskType = taskType;
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
