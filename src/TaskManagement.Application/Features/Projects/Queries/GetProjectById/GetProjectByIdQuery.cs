using MediatR;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Features.Projects.Queries.GetProjectById;

public record GetProjectByIdQuery(Guid Id)
    : IRequest<Project?>;