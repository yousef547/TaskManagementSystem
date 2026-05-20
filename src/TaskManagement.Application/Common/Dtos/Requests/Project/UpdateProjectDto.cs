using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Application.Common.Dtos.Requests.Project
{
    public class UpdateProjectDto
    {
        public Guid Id { get; init; }

        public string Name { get; init; }

        public string Description { get; init; }
    }
}
