using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Common.Dtos.Requests.Task;
using TaskManagement.Application.Common.Responses;
using TaskManagement.Application.Features.Tasks.Commands.CreateTask;
using TaskManagement.Application.Features.Tasks.Commands.DeleteTask;
using TaskManagement.Application.Features.Tasks.Commands.UpdateTaskStatus;
using TaskManagement.Application.Features.Tasks.Queries.GetTasksByProject;
using TaskManagement.Domain.Entities;

namespace TaskManagement.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/tasks")]
[Authorize]
public class TaskController : ControllerBase
{
    private readonly IMediator _mediator;

    public TaskController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Create(
        CreateTaskDto dto)
    {
        var id = await _mediator.Send(new CreateTaskCommand(dto));

        return Ok(
        ApiResponse<Guid>.SuccessResponse(
            id,
            "Task created successfully"));
    }

    [HttpPut("status")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> UpdateStatus(
        UpdateTaskStatusDto dto)
    {
        var result = await _mediator.Send(new UpdateTaskStatusCommand(dto));

        if (!result)
        {
            return NotFound(
                ApiResponse<string>.FailureResponse(
                    "Task not found"));
        }

        return Ok(
            ApiResponse<string>.SuccessResponse(
                null!,
                "Task status updated successfully"));
    }

    [HttpGet("project/{projectId:guid}")]
    public async Task<IActionResult> GetByProject(
        Guid projectId)
    {
        var tasks =
            await _mediator.Send(
                new GetTasksByProjectQuery(projectId));
        return Ok(
            ApiResponse<List<ProjectTask>>.SuccessResponse(
                tasks,
                "Tasks retrieved successfully"));
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result =
            await _mediator.Send(
                new DeleteTaskCommand(id));

        if (!result)
        {
            return NotFound(
                ApiResponse<string>.FailureResponse(
                    "Task not found"));
        }

        return Ok(
            ApiResponse<string>.SuccessResponse(
                null!,
                "Task deleted successfully"));
    }
}