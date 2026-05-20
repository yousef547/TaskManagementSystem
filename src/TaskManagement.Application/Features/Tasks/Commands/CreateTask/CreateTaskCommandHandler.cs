using MediatR;
using TaskManagement.Application.Common.Interfaces;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Repositories;

namespace TaskManagement.Application.Features.Tasks.Commands.CreateTask;

public class CreateTaskCommandHandler
    : IRequestHandler<CreateTaskCommand, Guid>
{
    private readonly IAsyncRepository<ProjectTask> _contextTask;

    public CreateTaskCommandHandler(
        IAsyncRepository<ProjectTask> contextTask)
    {
        _contextTask = contextTask;
    }

    public async Task<Guid> Handle(
        CreateTaskCommand request,
        CancellationToken cancellationToken)
    {
        var task = new ProjectTask
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            Status = request.Status,
            DueDate = request.DueDate,
            Priority = request.Priority,
            ProjectId = request.ProjectId
        };

        await _contextTask.AddAsync(
            task,
            cancellationToken);


        return task.Id;
    }
}