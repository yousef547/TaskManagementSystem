using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Common.Interfaces;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Repositories;

namespace TaskManagement.Application.Features.Tasks.Commands.DeleteTask;

public class DeleteTaskCommandHandler
    : IRequestHandler<DeleteTaskCommand, bool>
{
    private readonly IAsyncRepository<ProjectTask> _contextTask;

    public DeleteTaskCommandHandler(
        IAsyncRepository<ProjectTask> contextTask)
    {
        _contextTask = contextTask;
    }

    public async Task<bool> Handle(
        DeleteTaskCommand request,
        CancellationToken cancellationToken)
    {
        var task = await _contextTask
            .GetByIdAsync( request.Id);

        if (task is null)
            return false;

        await _contextTask.DeleteAsync(task, cancellationToken);


        return true;
    }
}