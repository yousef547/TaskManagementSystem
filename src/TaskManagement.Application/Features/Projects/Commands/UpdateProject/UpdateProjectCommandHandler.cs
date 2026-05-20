using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Common.Interfaces;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Repositories;

namespace TaskManagement.Application.Features.Projects.Commands.UpdateProject;

public class UpdateProjectCommandHandler
    : IRequestHandler<UpdateProjectCommand, bool>
{
    private readonly IAsyncRepository<Project> _contextProject;

    public UpdateProjectCommandHandler(
        IAsyncRepository<Project> contextProject)
    {
        _contextProject = contextProject;
    }

    public async Task<bool> Handle(
        UpdateProjectCommand request,
        CancellationToken cancellationToken)
    {
        var project = await _contextProject.GetByIdAsync(request.Id);

        if (project is null)
            return false;

        project.Name = request.Name;
        project.Description = request.Description;
        await _contextProject.UpdateAsync(project,cancellationToken);
        return true;
    }
}