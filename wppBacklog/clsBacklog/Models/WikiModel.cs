using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clsBacklog.Models
{
    [Table("Wikis")]
    public class WikiModel : SqlDbBaseModel
    {
        public WikiModel(string projectId, string id, string name, string description)
        {
            ProjectId = projectId;
            Id = id;
            Name = name;
            Description = description;
        }

        [Required]
        public string ProjectId { get; set; }

        [Key,MaxLength(64)]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public string? ParentWikiId { get; set; }
    }

    [Table("WikiOlds")]
    public class WikiOldModel : SqlDbBaseModel
    {
        public WikiOldModel(string originalWikiId, string id, string name, string description)
        {
            OriginalWikiId = originalWikiId;
            Id = id;
            Name = name;
            Description = description;
        }

        [Key, MaxLength(64)]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public string? ParentWikiId { get; set; }
        public string OriginalWikiId { get; set; }
    }
}
