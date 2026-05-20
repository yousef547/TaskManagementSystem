using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Application.Common.Dtos.Requests.Project;

namespace TaskManagement.Application.Features.Projects.Commands.CreateProject
{
    public record CreateProjectCommand : IRequest<Guid>
    {
        public string Name { get; init; }

        public string Description { get; init; }
        public CreateProjectCommand(string name,string description)
        {
            Name = name;
            Description = description;
        }
        public CreateProjectCommand(CreateProjectDto dto)
        {
            Name = dto.Name;
            Description = dto.Description;
        }
    }
}
