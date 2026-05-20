using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Common.Interfaces;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Repositories;

namespace TaskManagement.Application.Features.Projects.Queries.GetProjectById;

public class GetProjectByIdQueryHandler
    : IRequestHandler<GetProjectByIdQuery, Project?>
{
    private readonly IAsyncRepository<Project> _contextProject;

    public GetProjectByIdQueryHandler(
        IAsyncRepository<Project> contextProject)
    {
        _contextProject = contextProject;
    }


    public async Task<Project?> Handle(
        GetProjectByIdQuery request,
        CancellationToken cancellationToken)
    {
        var project = await _contextProject.GetByIdAsync( request.Id, c => c.Tasks);
        return project;
            
    }
}