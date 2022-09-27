using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clsBacklog.Models
{
    [Table("Files")]
    public class FileModel : SqlDbBaseModel
    {
        public FileModel(string projectId, string id, string fileName, string location, string mediaTypeName, string folderName)
        {
            ProjectId = projectId;
            Id = id;
            FileName = fileName;
            Location = location;
            MediaTypeName = mediaTypeName;
            FolderName = folderName;
        }

        [Key, MaxLength(64)]
        public string Id { get; set; }
        [MaxLength(64)]
        public string ProjectId { get; set; }
        [Required]
        public string FileName { get; set; }
        [Required]
        public string Location { get; set; }
        public int Size { get; set; }
        public string? SizeUnit { get; set; }
        [Required]
        public string MediaTypeName { get; set; }
        [Required]
        public string FolderName { get; set; }
    }

    [Table("FileChangeLogs")]
    public class FileChangeLogModel : SqlDbBaseModel
    {
        public FileChangeLogModel(string projectId, string id, string fileName, string folderName)
        {
            ProjectId = projectId;
            Id = id;
            FileName = fileName;
            FolderName = folderName;
        }

        [Key, MaxLength(64)]
        public string Id { get; set; }
        [MaxLength(64)]
        public string ProjectId { get; set; }
        [Required]
        public string FileName { get; set; }
        [Required]
        public string FolderName { get; set; }
    }

}
