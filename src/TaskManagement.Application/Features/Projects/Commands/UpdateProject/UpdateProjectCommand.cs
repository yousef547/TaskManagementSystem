using MediatR;
using TaskManagement.Application.Common.Dtos.Requests.Project;

namespace TaskManagement.Application.Features.Projects.Commands.UpdateProject;

public record UpdateProjectCommand
    : IRequest<bool>
{
    public Guid Id { get; init; }

    public string Name { get; init; } 

    public string Description { get; init; }


    public UpdateProjectCommand(Guid id,string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }
    public UpdateProjectCommand(UpdateProjectDto Dto)
    {
        Id = Dto.Id;
        Name = Dto.Name;
        Description = Dto.Name;
    }
}