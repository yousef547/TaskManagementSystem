using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Domain.Entities
{
    public class ProjectTask : EntityBase
    {

        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public ProjectTaskStatus Status { get; set; } 

        public DateTime DueDate { get; set; }

        public ProjectTaskPriority Priority { get; set; }

        public Guid ProjectId { get; set; }

        public Project Project { get; set; } = null!;
    }
}
