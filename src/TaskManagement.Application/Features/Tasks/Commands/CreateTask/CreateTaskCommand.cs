using MediatR;
using TaskManagement.Application.Common.Dtos.Requests.Task;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.Features.Tasks.Commands.CreateTask;

public record CreateTaskCommand
    : IRequest<Guid>
{
    public string Title { get; init; } = null!;

    public string Description { get; init; } = null!;

    public ProjectTaskStatus Status { get; init; }

    public DateTime DueDate { get; init; }

    public ProjectTaskPriority Priority { get; init; }

    public Guid ProjectId { get; init; }

    public CreateTaskCommand()
    {
    }

    public CreateTaskCommand(CreateTaskDto dto)
    {
        Title = dto.Title;
        Description = dto.Description;
        Status = dto.Status;
        DueDate = dto.DueDate;
        Priority = dto.Priority;
        ProjectId = dto.ProjectId;
    }
}