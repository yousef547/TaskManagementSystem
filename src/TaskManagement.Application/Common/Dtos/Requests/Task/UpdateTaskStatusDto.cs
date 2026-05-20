using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.Common.Dtos.Requests.Task
{
    public class UpdateTaskStatusDto
    {
        public Guid Id { get; init; }

        public ProjectTaskStatus Status { get; init; }
    }
}
