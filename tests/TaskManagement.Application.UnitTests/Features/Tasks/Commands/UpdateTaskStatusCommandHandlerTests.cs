using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using TaskManagement.Application.Features.Tasks.Commands.UpdateTaskStatus;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;
using TaskManagement.Domain.Repositories;

namespace TaskManagement.Application.UnitTests
    .Features.Tasks.Commands;

public class UpdateTaskStatusCommandHandlerTests
{
    private readonly Mock<IAsyncRepository<ProjectTask>>
        _repositoryMock;

    private readonly UpdateTaskStatusCommandHandler
        _handler;

    public UpdateTaskStatusCommandHandlerTests()
    {
        _repositoryMock =
            new Mock<IAsyncRepository<ProjectTask>>();

        _handler =
            new UpdateTaskStatusCommandHandler(
                _repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnTrue_WhenTaskExists()
    {
        
        var task = new ProjectTask
        {
            Id = Guid.NewGuid(),
            Status = ProjectTaskStatus.Pending
        };

        var command =
            new UpdateTaskStatusCommand(
                task.Id,
                ProjectTaskStatus.Completed);

        _repositoryMock
            .Setup(x => x.GetByIdAsync(
                task.Id,
                It.IsAny<Expression<Func<ProjectTask, object>>[]>()))
            .ReturnsAsync(task);

         
        var result =
            await _handler.Handle(
                command,
                CancellationToken.None);

         
        result.Should().BeTrue();
    }



    [Fact]
    public async Task Handle_ShouldUpdateTaskStatus()
    {
        
        var task = new ProjectTask
        {
            Id = Guid.NewGuid(),
            Status = ProjectTaskStatus.Pending
        };

        var command =
            new UpdateTaskStatusCommand(
                task.Id,
                ProjectTaskStatus.InProgress);

        _repositoryMock
            .Setup(x => x.GetByIdAsync(
                task.Id,
                It.IsAny<Expression<Func<ProjectTask, object>>[]>()))
            .ReturnsAsync(task);

         
        await _handler.Handle(
            command,
            CancellationToken.None);

         
        task.Status
            .Should()
            .Be(ProjectTaskStatus.InProgress);
    }

    [Fact]
    public async Task Handle_ShouldCallUpdateAsync_WhenTaskExists()
    {
        
        var task = new ProjectTask
        {
            Id = Guid.NewGuid(),
            Status = ProjectTaskStatus.Pending
        };

        var command =
            new UpdateTaskStatusCommand(
                task.Id,
                ProjectTaskStatus.Completed);

        _repositoryMock
            .Setup(x => x.GetByIdAsync(
                task.Id,
                It.IsAny<Expression<Func<ProjectTask, object>>[]>()))
            .ReturnsAsync(task);

         
        await _handler.Handle(
            command,
            CancellationToken.None);

         
        _repositoryMock.Verify(
            x => x.UpdateAsync(
                task,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldNotCallUpdateAsync_WhenTaskDoesNotExist()
    {
        
        var command =
            new UpdateTaskStatusCommand(
                Guid.NewGuid(),
                ProjectTaskStatus.Completed);

        _repositoryMock
            .Setup(x => x.GetByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<Expression<Func<ProjectTask, object>>[]>()))
            .ReturnsAsync((ProjectTask?)null);

         
        await _handler.Handle(
            command,
            CancellationToken.None);

         
        _repositoryMock.Verify(
            x => x.UpdateAsync(
                It.IsAny<ProjectTask>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldCallRepositoryOnce()
    {
        
        var task = new ProjectTask
        {
            Id = Guid.NewGuid(),
            Status = ProjectTaskStatus.Pending
        };

        var command =
            new UpdateTaskStatusCommand(
                task.Id,
                ProjectTaskStatus.Completed);

        _repositoryMock
            .Setup(x => x.GetByIdAsync(
                task.Id,
                It.IsAny<Expression<Func<ProjectTask, object>>[]>()))
            .ReturnsAsync(task);

         
        await _handler.Handle(
            command,
            CancellationToken.None);

         
        _repositoryMock.Verify(
            x => x.GetByIdAsync(
                task.Id,
                It.IsAny<Expression<Func<ProjectTask, object>>[]>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldPassCorrectTaskId()
    {
        
        var taskId = Guid.NewGuid();

        var task = new ProjectTask
        {
            Id = taskId,
            Status = ProjectTaskStatus.Pending
        };

        Guid capturedId = Guid.Empty;

        var command =
            new UpdateTaskStatusCommand(
                taskId,
                ProjectTaskStatus.Completed);

        _repositoryMock
            .Setup(x => x.GetByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<Expression<Func<ProjectTask, object>>[]>()))
            .Callback<Guid,
                Expression<Func<ProjectTask, object>>[]>(
                (id, _) =>
                {
                    capturedId = id;
                })
            .ReturnsAsync(task);

         
        await _handler.Handle(
            command,
            CancellationToken.None);

         
        capturedId.Should().Be(taskId);
    }



    [Fact]
    public async Task Handle_ShouldThrowException_WhenRepositoryFails()
    {
        
        var task = new ProjectTask
        {
            Id = Guid.NewGuid(),
            Status = ProjectTaskStatus.Pending
        };

        var command =
            new UpdateTaskStatusCommand(
                task.Id,
                ProjectTaskStatus.Completed);

        _repositoryMock
            .Setup(x => x.GetByIdAsync(
                task.Id,
                It.IsAny<Expression<Func<ProjectTask, object>>[]>()))
            .ReturnsAsync(task);

        _repositoryMock
            .Setup(x => x.UpdateAsync(
                It.IsAny<ProjectTask>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(
                new Exception("Database error"));

         
        Func<Task> act = async () =>
            await _handler.Handle(
                command,
                CancellationToken.None);

         
        await act.Should()
            .ThrowAsync<Exception>()
            .WithMessage("Database error");
    }


}