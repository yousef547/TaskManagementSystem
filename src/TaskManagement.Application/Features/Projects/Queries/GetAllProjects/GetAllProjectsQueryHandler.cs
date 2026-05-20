using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Common.Interfaces;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Repositories;

namespace TaskManagement.Application.Features.Projects.Queries.GetAllProjects;

public class GetAllProjectsQueryHandler
    : IRequestHandler<GetAllProjectsQuery, List<Project>>
{
    private readonly IAsyncRepository<Project> _contextProject;
    private readonly ICacheService _cacheService;
    public GetAllProjectsQueryHandler(
        IAsyncRepository<Project> contextProject, ICacheService cacheService)
    {
        _contextProject = contextProject;
        _cacheService = cacheService;
    }


    public async Task<List<Project>> Handle(
        GetAllProjectsQuery request,
        CancellationToken cancellationToken)
    {
        var cachedProjects =
       await _cacheService.GetAsync<
           List<Project>>(request.CacheKey.ToString());

        if (cachedProjects is not null)
        {
            return cachedProjects;
        }
        var projects = await _contextProject.GetAllAsync();
        await _cacheService.SetAsync(request.CacheKey.ToString(),projects);
        return projects.ToList();
    }
}