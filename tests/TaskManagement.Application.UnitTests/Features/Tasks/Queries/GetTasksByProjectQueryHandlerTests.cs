using FluentAssertions;
using Moq;
using System.Linq.Expressions;
using TaskManagement.Application.Features.Tasks.Queries.GetTasksByProject;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;
using TaskManagement.Domain.Repositories;

namespace TaskManagement.Application.UnitTests
    .Features.Tasks.Queries;

public class GetTasksByProjectQueryHandlerTests
{
    private readonly Mock<IAsyncRepository<ProjectTask>>
        _repositoryMock;

    private readonly GetTasksByProjectQueryHandler
        _handler;

    public GetTasksByProjectQueryHandlerTests()
    {
        _repositoryMock =
            new Mock<IAsyncRepository<ProjectTask>>();

        _handler =
            new GetTasksByProjectQueryHandler(
                _repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnTasks_WhenTasksExist()
    {
         
        var projectId = Guid.NewGuid();

        var tasks = new List<ProjectTask>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Task 1",
                ProjectId = projectId
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Task 2",
                ProjectId = projectId
            }
        };

        var query =
            new GetTasksByProjectQuery(projectId);

        _repositoryMock
            .Setup(x => x.GetAllAsync(
                It.IsAny<Expression<Func<ProjectTask, bool>>>()))
            .ReturnsAsync(tasks);

         
        var result =
            await _handler.Handle(
                query,
                CancellationToken.None);

        result.Should().NotBeNull();

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoTasksExist()
    {
         
        var projectId = Guid.NewGuid();

        var query =
            new GetTasksByProjectQuery(projectId);

        _repositoryMock
            .Setup(x => x.GetAllAsync(
                It.IsAny<Expression<Func<ProjectTask, bool>>>()))
            .ReturnsAsync(new List<ProjectTask>());

         
        var result =
            await _handler.Handle(
                query,
                CancellationToken.None);

         
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_ShouldReturnCorrectTasksCount()
    {
         
        var projectId = Guid.NewGuid();

        var tasks = new List<ProjectTask>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Task 1",
                ProjectId = projectId
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Task 2",
                ProjectId = projectId
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Task 3",
                ProjectId = projectId
            }
        };

        var query =
            new GetTasksByProjectQuery(projectId);

        _repositoryMock
            .Setup(x => x.GetAllAsync(
                It.IsAny<Expression<Func<ProjectTask, bool>>>()))
            .ReturnsAsync(tasks);

         
        var result =
            await _handler.Handle(
                query,
                CancellationToken.None);

         
        result.Should().HaveCount(3);
    }

    [Fact]
    public async Task Handle_ShouldReturnTasksForCorrectProject()
    {
         
        var projectId = Guid.NewGuid();

        var tasks = new List<ProjectTask>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Task 1",
                ProjectId = projectId
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Task 2",
                ProjectId = projectId
            }
        };

        var query =
            new GetTasksByProjectQuery(projectId);

        _repositoryMock
            .Setup(x => x.GetAllAsync(
                It.IsAny<Expression<Func<ProjectTask, bool>>>()))
            .ReturnsAsync(tasks);

         
        var result =
            await _handler.Handle(
                query,
                CancellationToken.None);

         
        result.All(x => x.ProjectId == projectId)
            .Should()
            .BeTrue();
    }

    [Fact]
    public async Task Handle_ShouldCallRepositoryOnce()
    {
         
        var projectId = Guid.NewGuid();

        var query =
            new GetTasksByProjectQuery(projectId);

        _repositoryMock
            .Setup(x => x.GetAllAsync(
                It.IsAny<Expression<Func<ProjectTask, bool>>>()))
            .ReturnsAsync(new List<ProjectTask>());

         
        await _handler.Handle(
            query,
            CancellationToken.None);

         
        _repositoryMock.Verify(
            x => x.GetAllAsync(
                It.IsAny<Expression<Func<ProjectTask, bool>>>()),
            Times.Once);
    }

 

    [Fact]
    public async Task Handle_ShouldReturnCorrectTaskData()
    {
         
        var projectId = Guid.NewGuid();

        var tasks = new List<ProjectTask>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Implement Redis",
                Description = "Add distributed caching",
                ProjectId = projectId,
                Status = ProjectTaskStatus.InProgress,
                Priority = ProjectTaskPriority.High
            }
        };

        var query =
            new GetTasksByProjectQuery(projectId);

        _repositoryMock
            .Setup(x => x.GetAllAsync(
                It.IsAny<Expression<Func<ProjectTask, bool>>>()))
            .ReturnsAsync(tasks);

         
        var result =
            await _handler.Handle(
                query,
                CancellationToken.None);

         
        var task = result.First();

        task.Title.Should().Be("Implement Redis");

        task.Description
            .Should()
            .Be("Add distributed caching");

        task.Status
            .Should()
            .Be(ProjectTaskStatus.InProgress);

        task.Priority
            .Should()
            .Be(ProjectTaskPriority.High);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenRepositoryFails()
    {
         
        var projectId = Guid.NewGuid();

        var query =
            new GetTasksByProjectQuery(projectId);

        _repositoryMock
            .Setup(x => x.GetAllAsync(
                It.IsAny<Expression<Func<ProjectTask, bool>>>()))
            .ThrowsAsync(
                new Exception("Database error"));

         
        Func<Task> act = async () =>
            await _handler.Handle(
                query,
                CancellationToken.None);

         
        await act.Should()
            .ThrowAsync<Exception>()
            .WithMessage("Database error");
    }


}