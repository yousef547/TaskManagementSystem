using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Domain.Entities
{
    public class Project: EntityBase
    {

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;


        public ICollection<ProjectTask> Tasks { get; set; }
            = [];
    }
}
