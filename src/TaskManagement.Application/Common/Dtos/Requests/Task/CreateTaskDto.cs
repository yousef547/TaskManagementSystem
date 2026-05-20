using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.Common.Dtos.Requests.Task
{
    public class CreateTaskDto
    {
        public string Title { get; init; } = null!;

        public string Description { get; init; } = null!;

        public ProjectTaskStatus Status { get; init; }

        public DateTime DueDate { get; init; }

        public ProjectTaskPriority Priority { get; init; }

        public Guid ProjectId { get; init; }
    }
}
