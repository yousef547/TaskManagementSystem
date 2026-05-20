using FluentAssertions;
using Moq;
using TaskManagement.Application.Features.Tasks.Commands.CreateTask;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;
using TaskManagement.Domain.Repositories;

namespace TaskManagement.Application.UnitTests
    .Features.Tasks.Commands;

public class CreateTaskCommandHandlerTests
{
    private readonly Mock<IAsyncRepository<ProjectTask>>
        _repositoryMock;

    private readonly CreateTaskCommandHandler
        _handler;

    public CreateTaskCommandHandlerTests()
    {
        _repositoryMock =
            new Mock<IAsyncRepository<ProjectTask>>();

        _handler =
            new CreateTaskCommandHandler(
                _repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldCreateTask_AndReturnId()
    {
        
        var command =
            new CreateTaskCommand(
                "Implement Redis",
                "Add distributed caching",
                ProjectTaskStatus.Pending,
                DateTime.UtcNow.AddDays(5),
                ProjectTaskPriority.High,
                Guid.NewGuid());

        
        var result =
            await _handler.Handle(
                command,
                CancellationToken.None);

         
        result.Should().NotBe(Guid.Empty);

        _repositoryMock.Verify(
            x => x.AddAsync(
                It.IsAny<ProjectTask>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldCallRepositoryOnce()
    {
        
        var command =
            new CreateTaskCommand(
                "Task",
                "Description",
                ProjectTaskStatus.Pending,
                DateTime.UtcNow.AddDays(1),
                ProjectTaskPriority.High,
                Guid.NewGuid());

         
        await _handler.Handle(
            command,
            CancellationToken.None);

         
        _repositoryMock.Verify(
            x => x.AddAsync(
                It.IsAny<ProjectTask>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }


    [Fact]
    public async Task Handle_ShouldThrowException_WhenRepositoryFails()
    {
        
        var command =
            new CreateTaskCommand(
                "Task",
                "Description",
                ProjectTaskStatus.Pending,
                DateTime.UtcNow.AddDays(1),
                ProjectTaskPriority.High,
                Guid.NewGuid());

        _repositoryMock
            .Setup(x => x.AddAsync(
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