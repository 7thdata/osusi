using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clsBacklog.Models
{
    public class SqlDbBaseModel
    {

        public string? OwnerId { get; set; }
        public DateTime Created { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime Deleted { get; set; }
        
    }
}
