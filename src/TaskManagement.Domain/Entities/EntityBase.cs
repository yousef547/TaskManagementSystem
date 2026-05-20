using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Domain.Entities
{
    public class EntityBase
    {
        public Guid Id { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }

    }
}
