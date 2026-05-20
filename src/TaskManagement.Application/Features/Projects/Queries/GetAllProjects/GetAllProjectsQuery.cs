using MediatR;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Features.Projects.Queries.GetAllProjects;

public record GetAllProjectsQuery(Guid? CacheKey)
    : IRequest<List<Project>>;