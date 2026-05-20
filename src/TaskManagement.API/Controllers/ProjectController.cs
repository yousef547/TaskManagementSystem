using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Common.Dtos.Requests.Project;
using TaskManagement.Application.Common.Responses;
using TaskManagement.Application.Features.Projects.Commands.CreateProject;
using TaskManagement.Application.Features.Projects.Commands.DeleteProject;
using TaskManagement.Application.Features.Projects.Commands.UpdateProject;
using TaskManagement.Application.Features.Projects.Queries.GetAllProjects;
using TaskManagement.Application.Features.Projects.Queries.GetProjectById;
using TaskManagement.Domain.Entities;

namespace TaskManagement.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/projects")]
[Authorize]
public class ProjectController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProjectController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Create(
         CreateProjectDto dto)
    {
        var id = await _mediator.Send(new CreateProjectCommand(dto));

        return Ok(
     ApiResponse<Guid>.SuccessResponse(
         id,
         "Project created successfully"));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(Guid? cacheKey = null)
    {
        cacheKey ??= Guid.NewGuid();
        var projects =
            await _mediator.Send(new GetAllProjectsQuery(cacheKey));

        return Ok(
    ApiResponse<List<Project>>.SuccessResponse(
        projects, cacheKey.ToString()));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var project =
            await _mediator.Send(
                new GetProjectByIdQuery(id));

        if (project is null)
            return NotFound(ApiResponse<string>.FailureResponse("Project not found"));

        return Ok(
    ApiResponse<Project>.SuccessResponse(
        project));
    }

    [HttpPut]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Update(
        UpdateProjectDto dto)
    {
        var result = await _mediator.Send(new UpdateProjectCommand(dto));

        if (!result)
            return NotFound(ApiResponse<string>.FailureResponse("Project not found"));

        return Ok(
    ApiResponse<string>.SuccessResponse(
        null!,
        "Project updated successfully"));
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result =
            await _mediator.Send(
                new DeleteProjectCommand(id));

        if (!result)
        {
            return NotFound(
                ApiResponse<string>.FailureResponse(
                    "Project not found"));
        }

        return Ok(
      ApiResponse<string>.SuccessResponse(
          null!,
          "Project deleted successfully"));
    }
}