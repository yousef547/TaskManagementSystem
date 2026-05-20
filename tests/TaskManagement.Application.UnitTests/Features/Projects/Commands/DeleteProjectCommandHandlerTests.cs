using FluentAssertions;
using Moq;
using TaskManagement.Application.Features.Projects.Commands.DeleteProject;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Repositories;

namespace TaskManagement.Application.UnitTests
    .Features.Projects.Commands;

public class DeleteProjectCommandHandlerTests
{
    private readonly Mock<IAsyncRepository<Project>>
        _repositoryMock;

    private readonly DeleteProjectCommandHandler
        _handler;

    public DeleteProjectCommandHandlerTests()
    {
        _repositoryMock =
            new Mock<IAsyncRepository<Project>>();

        _handler =
            new DeleteProjectCommandHandler(
                _repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnTrue_WhenProjectExists()
    {
         
        var project = new Project
        {
            Id = Guid.NewGuid(),
            Name = "E-Commerce",
            Description = "Backend API"
        };

        var command =
            new DeleteProjectCommand(project.Id);

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
            new DeleteProjectCommand(Guid.NewGuid());

        _repositoryMock
            .Setup(x => x.GetByIdAsync(
                It.IsAny<Guid>()))
            .ReturnsAsync((Project?)null);

         
        var result =
            await _handler.Handle(
                command,
                CancellationToken.None);

         
        result.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_ShouldCallDeleteAsync_WhenProjectExists()
    {
         
        var project = new Project
        {
            Id = Guid.NewGuid(),
            Name = "Project",
            Description = "Description"
        };

        var command =
            new DeleteProjectCommand(project.Id);

        _repositoryMock
            .Setup(x => x.GetByIdAsync(
                project.Id))
            .ReturnsAsync(project);

         
        await _handler.Handle(
            command,
            CancellationToken.None);

         
        _repositoryMock.Verify(
            x => x.DeleteAsync(
                project,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }


    [Fact]
    public async Task Handle_ShouldCallGetByIdAsyncOnce()
    {
         
        var project = new Project
        {
            Id = Guid.NewGuid(),
            Name = "Project"
        };

        var command =
            new DeleteProjectCommand(project.Id);

        _repositoryMock
            .Setup(x => x.GetByIdAsync(
                project.Id))
            .ReturnsAsync(project);

         
        await _handler.Handle(
            command,
            CancellationToken.None);

         
        _repositoryMock.Verify(
            x => x.GetByIdAsync(
                project.Id),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenRepositoryFails()
    {
         
        var project = new Project
        {
            Id = Guid.NewGuid(),
            Name = "Project"
        };

        var command =
            new DeleteProjectCommand(project.Id);

        _repositoryMock
            .Setup(x => x.GetByIdAsync(
                project.Id))
            .ReturnsAsync(project);

        _repositoryMock
            .Setup(x => x.DeleteAsync(
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


    [Fact]
    public async Task Handle_ShouldDeleteCorrectProject()
    {
         
        var project = new Project
        {
            Id = Guid.NewGuid(),
            Name = "Correct Project"
        };

        var command =
            new DeleteProjectCommand(project.Id);

        Project? deletedProject = null;

        _repositoryMock
            .Setup(x => x.GetByIdAsync(
                project.Id))
            .ReturnsAsync(project);

        _repositoryMock
            .Setup(x => x.DeleteAsync(
                It.IsAny<Project>(),
                It.IsAny<CancellationToken>()))
            .Callback<Project, CancellationToken>(
                (p, _) =>
                {
                    deletedProject = p;
                });

         
        await _handler.Handle(
            command,
            CancellationToken.None);

         
        deletedProject.Should().NotBeNull();

        deletedProject!.Id
            .Should()
            .Be(project.Id);
    }
}