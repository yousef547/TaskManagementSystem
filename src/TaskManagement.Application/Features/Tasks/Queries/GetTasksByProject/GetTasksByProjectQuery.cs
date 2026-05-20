using MediatR;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Features.Tasks.Queries.GetTasksByProject;

public record GetTasksByProjectQuery(Guid ProjectId)
    : IRequest<List<ProjectTask>>;