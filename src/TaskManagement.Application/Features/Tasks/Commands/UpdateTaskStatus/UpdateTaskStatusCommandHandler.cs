using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Common.Interfaces;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Repositories;

namespace TaskManagement.Application.Features.Tasks.Commands.UpdateTaskStatus;

public class UpdateTaskStatusCommandHandler
    : IRequestHandler<UpdateTaskStatusCommand, bool>
{
    private readonly IAsyncRepository<ProjectTask> _contextTask;

    public UpdateTaskStatusCommandHandler(
        IAsyncRepository<ProjectTask> contextTask)
    {
        _contextTask = contextTask;
    }


    public async Task<bool> Handle(
        UpdateTaskStatusCommand request,
        CancellationToken cancellationToken)
    {
        var task = await _contextTask.GetByIdAsync(request.Id);

        if (task is null)
            return false;

        task.Status = request.Status;
        await _contextTask.UpdateAsync(task,cancellationToken);
        return true;
    }
}