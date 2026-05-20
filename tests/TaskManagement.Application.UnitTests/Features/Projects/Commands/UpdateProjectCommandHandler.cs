using FluentAssertions;
using Moq;
using TaskManagement.Application.Features.Projects.Commands.UpdateProject;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Repositories;

namespace TaskManagement.Application.UnitTests
    .Features.Projects.Commands;

public class UpdateProjectCommandHandlerTests
{
    private readonly Mock<IAsyncRepository<Project>>
        _repositoryMock;

    private readonly UpdateProjectCommandHandler
        _handler;

    public UpdateProjectCommandHandlerTests()
    {
        _repositoryMock =
            new Mock<IAsyncRepository<Project>>();

        _handler =
            new UpdateProjectCommandHandler(
                _repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnTrue_WhenProjectExists()
    {
         
        var project = new Project
        {
            Id = Guid.NewGuid(),
            Name = "Old Name",
            Description = "Old Description"
        };

        var command =
            new UpdateProjectCommand(
                project.Id,
                "New Name",
                "New Description");

        _repositoryMock
            .Setup(x => x.GetByIdAsync(
                project.Id))
            .ReturnsAsync(project);

         
        var result =
            await _handler.Handle(
                command,
                CancellationToken.None);

         
        result.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_ShouldReturnFalse_WhenProjectDoesNotExist()
    {
        var command =
            new UpdateProjectCommand(
                Guid.NewGuid(),
                "New Name",
                "New Description");

        _repositoryMock.Setup(x => x.GetByIdAsync(
                It.IsAny<Guid>()))
            .ReturnsAsync((Project?)null);

        var result =
            await _handler.Handle(
                command,
                CancellationToken.None);

        result.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_ShouldUpdateProjectName()
    {
         
        var project = new Project
        {
            Id = Guid.NewGuid(),
            Name = "Old Name",
            Description = "Old Description"
        };

        var command =
            new UpdateProjectCommand(
                project.Id,
                "Updated Name",
                "Updated Description");

        _repositoryMock
            .Setup(x => x.GetByIdAsync(
                project.Id))
            .ReturnsAsync(project);

        await _handler.Handle(
            command,
            CancellationToken.None);

        project.Name
            .Should()
            .Be("Updated Name");
    }

 

    [Fact]
    public async Task Handle_ShouldCallUpdateAsync_WhenProjectExists()
    {
         
        var project = new Project
        {
            Id = Guid.NewGuid(),
            Name = "Old Name",
            Description = "Old Description"
        };

        var command =
            new UpdateProjectCommand(
                project.Id,
                "Updated Name",
                "Updated Description");

        _repositoryMock
            .Setup(x => x.GetByIdAsync(
                project.Id))
            .ReturnsAsync(project);

         
        await _handler.Handle(
            command,
            CancellationToken.None);

         
        _repositoryMock.Verify(
            x => x.UpdateAsync(
                project,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldNotCallUpdateAsync_WhenProjectDoesNotExist()
    {
         
        var command =
            new UpdateProjectCommand(
                Guid.NewGuid(),
                "Updated Name",
                "Updated Description");

        _repositoryMock
            .Setup(x => x.GetByIdAsync(
                It.IsAny<Guid>()))
            .ReturnsAsync((Project?)null);

         
        await _handler.Handle(
            command,
            CancellationToken.None);

         
        _repositoryMock.Verify(
            x => x.UpdateAsync(
                It.IsAny<Project>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenRepositoryFails()
    {
         
        var project = new Project
        {
            Id = Guid.NewGuid(),
            Name = "Old Name",
            Description = "Old Description"
        };

        var command =
            new UpdateProjectCommand(
                project.Id,
                "Updated Name",
                "Updated Description");

        _repositoryMock
            .Setup(x => x.GetByIdAsync(
                project.Id))
            .ReturnsAsync(project);

        _repositoryMock
            .Setup(x => x.UpdateAsync(
                It.IsAny<Project>(),
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