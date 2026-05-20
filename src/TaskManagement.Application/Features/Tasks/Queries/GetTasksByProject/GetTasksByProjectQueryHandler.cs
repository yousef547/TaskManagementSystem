using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Common.Interfaces;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Repositories;

namespace TaskManagement.Application.Features.Tasks.Queries.GetTasksByProject;

public class GetTasksByProjectQueryHandler
    : IRequestHandler<GetTasksByProjectQuery, List<ProjectTask>>
{
    private readonly IAsyncRepository<ProjectTask> _contextTask;

    public GetTasksByProjectQueryHandler(
        IAsyncRepository<ProjectTask> contextTask)
    {
        _contextTask = contextTask;
    }

    public async Task<List<ProjectTask>> Handle(
        GetTasksByProjectQuery request,
        CancellationToken cancellationToken)
    {
        var projectTask = await _contextTask.GetAllAsync(x => x.ProjectId == request.ProjectId);
        return projectTask.ToList();
            
    }
}