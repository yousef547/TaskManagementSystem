using MediatR;
using TaskManagement.Application.Common.Dtos.Requests.Task;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.Features.Tasks.Commands.UpdateTaskStatus;

public record UpdateTaskStatusCommand
    : IRequest<bool>
{
    public Guid Id { get; init; }
    public ProjectTaskStatus Status { get; init; }
    public UpdateTaskStatusCommand(Guid id, ProjectTaskStatus status)
    {
        Id = id;
        Status = status;
    }
    public UpdateTaskStatusCommand(UpdateTaskStatusDto dto)
    {
        Id = dto.Id;
        Status = dto.Status;
    }
}