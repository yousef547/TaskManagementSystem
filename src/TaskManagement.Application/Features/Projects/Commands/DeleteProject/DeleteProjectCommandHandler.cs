using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Common.Interfaces;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Repositories;

namespace TaskManagement.Application.Features.Projects.Commands.DeleteProject;

public class DeleteProjectCommandHandler
    : IRequestHandler<DeleteProjectCommand, bool>
{


    private readonly IAsyncRepository<Project> _contextProject;

    public DeleteProjectCommandHandler(
        IAsyncRepository<Project> contextProject)
    {
        _contextProject = contextProject;
    }
    public async Task<bool> Handle(
        DeleteProjectCommand request,
        CancellationToken cancellationToken)
    {
        var project = await _contextProject
            .GetByIdAsync(request.Id);

        if (project is null)
            return false;

        await _contextProject.DeleteAsync(project, cancellationToken);
        return true;
    }
}