using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using TaskManagement.Application.Features.Tasks.Commands.DeleteTask;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Repositories;

namespace TaskManagement.Application.UnitTests
    .Features.Tasks.Commands;

public class DeleteTaskCommandHandlerTests
{
    private readonly Mock<IAsyncRepository<ProjectTask>>
        _repositoryMock;

    private readonly DeleteTaskCommandHandler
        _handler;

    public DeleteTaskCommandHandlerTests()
    {
        _repositoryMock =
            new Mock<IAsyncRepository<ProjectTask>>();

        _handler =
            new DeleteTaskCommandHandler(
                _repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnTrue_WhenTaskExists()
    {
        
        var task = new ProjectTask
        {
            Id = Guid.NewGuid(),
            Title = "Implement Redis"
        };

        var command =
            new DeleteTaskCommand(task.Id);

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
    public async Task Handle_ShouldReturnFalse_WhenTaskDoesNotExist()
    {
        
        var command =
            new DeleteTaskCommand(Guid.NewGuid());

        _repositoryMock
            .Setup(x => x.GetByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<Expression<Func<ProjectTask, object>>[]>()))
            .ReturnsAsync((ProjectTask?)null);

         
        var result =
            await _handler.Handle(
                command,
                CancellationToken.None);

         
        result.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_ShouldCallDeleteAsync_WhenTaskExists()
    {
        
        var task = new ProjectTask
        {
            Id = Guid.NewGuid(),
            Title = "Task"
        };

        var command =
            new DeleteTaskCommand(task.Id);

        _repositoryMock
            .Setup(x => x.GetByIdAsync(
                task.Id,
                It.IsAny<Expression<Func<ProjectTask, object>>[]>()))
            .ReturnsAsync(task);

         
        await _handler.Handle(
            command,
            CancellationToken.None);

         
        _repositoryMock.Verify(
            x => x.DeleteAsync(
                task,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldNotCallDeleteAsync_WhenTaskDoesNotExist()
    {
        
        var command =
            new DeleteTaskCommand(Guid.NewGuid());

        _repositoryMock
            .Setup(x => x.GetByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<Expression<Func<ProjectTask, object>>[]>()))
            .ReturnsAsync((ProjectTask?)null);

         
        await _handler.Handle(
            command,
            CancellationToken.None);

         
        _repositoryMock.Verify(
            x => x.DeleteAsync(
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
            Title = "Task"
        };

        var command =
            new DeleteTaskCommand(task.Id);

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
    public async Task Handle_ShouldThrowException_WhenRepositoryFails()
    {
        
        var task = new ProjectTask
        {
            Id = Guid.NewGuid(),
            Title = "Task"
        };

        var command =
            new DeleteTaskCommand(task.Id);

        _repositoryMock
            .Setup(x => x.GetByIdAsync(
                task.Id,
                It.IsAny<Expression<Func<ProjectTask, object>>[]>()))
            .ReturnsAsync(task);

        _repositoryMock
            .Setup(x => x.DeleteAsync(
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

    [Fact]
    public async Task Handle_ShouldDeleteCorrectTask()
    {
        
        var task = new ProjectTask
        {
            Id = Guid.NewGuid(),
            Title = "Correct Task"
        };

        ProjectTask? deletedTask = null;

        var command =
            new DeleteTaskCommand(task.Id);

        _repositoryMock
            .Setup(x => x.GetByIdAsync(
                task.Id,
                It.IsAny<Expression<Func<ProjectTask, object>>[]>()))
            .ReturnsAsync(task);

        _repositoryMock
            .Setup(x => x.DeleteAsync(
                It.IsAny<ProjectTask>(),
                It.IsAny<CancellationToken>()))
            .Callback<ProjectTask, CancellationToken>(
                (t, _) =>
                {
                    deletedTask = t;
                });

         
        await _handler.Handle(
            command,
            CancellationToken.None);

         
        deletedTask.Should().NotBeNull();

        deletedTask!.Id
            .Should()
            .Be(task.Id);

        deletedTask.Title
            .Should()
            .Be("Correct Task");
    }

   
}