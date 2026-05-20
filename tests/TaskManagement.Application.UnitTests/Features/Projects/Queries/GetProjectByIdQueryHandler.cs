using FluentAssertions;
using Moq;
using System.Linq.Expressions;
using TaskManagement.Application.Features.Projects.Queries.GetProjectById;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Repositories;

namespace TaskManagement.Application.UnitTests
    .Features.Projects.Queries;

public class GetProjectByIdQueryHandlerTests
{
    private readonly Mock<IAsyncRepository<Project>>
        _repositoryMock;

    private readonly GetProjectByIdQueryHandler
        _handler;

    public GetProjectByIdQueryHandlerTests()
    {
        _repositoryMock =
            new Mock<IAsyncRepository<Project>>();

        _handler =
            new GetProjectByIdQueryHandler(
                _repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnProject_WhenProjectExists()
    {
         
        var project = new Project
        {
            Id = Guid.NewGuid(),
            Name = "E-Commerce",
            Description = "Backend API"
        };

        var query =
            new GetProjectByIdQuery(project.Id);

        _repositoryMock
      .Setup(x => x.GetByIdAsync(
          project.Id,
          It.IsAny<Expression<Func<Project, object>>[]>()))
      .ReturnsAsync(project);

         
        var result =
            await _handler.Handle(
                query,
                CancellationToken.None);

         
        result.Should().NotBeNull();

        result!.Id.Should().Be(project.Id);

        result.Name.Should().Be("E-Commerce");

        result.Description
            .Should()
            .Be("Backend API");
    }

    [Fact]
    public async Task Handle_ShouldReturnNull_WhenProjectDoesNotExist()
    {
         
        var query =
            new GetProjectByIdQuery(Guid.NewGuid());

        _repositoryMock
            .Setup(x => x.GetByIdAsync(
                It.IsAny<Guid>()))
            .ReturnsAsync((Project?)null);

         
        var result =
            await _handler.Handle(
                query,
                CancellationToken.None);

         
        result.Should().BeNull();
    }



    [Fact]
    public async Task Handle_ShouldPassCorrectProjectId()
    {
         
        var projectId = Guid.NewGuid();

        var project = new Project
        {
            Id = projectId,
            Name = "Project"
        };

        var query =
            new GetProjectByIdQuery(projectId);

        Guid capturedId = Guid.Empty;


        _repositoryMock
            .Setup(x => x.GetByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<Expression<Func<Project, object>>[]>()))
            .Callback<Guid, Expression<Func<Project, object>>[]>(
                (id, _) =>
                {
                    capturedId = id;
                })
            .ReturnsAsync(project);




         
        await _handler.Handle(
            query,
            CancellationToken.None);

         
        capturedId.Should().Be(projectId);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenRepositoryFails()
    {
         
        var query =
            new GetProjectByIdQuery(Guid.NewGuid());

        _repositoryMock
            .Setup(x => x.GetByIdAsync(
                 It.IsAny<Guid>(),
     It.IsAny<Expression<Func<Project, object>>[]>()))
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


    [Fact]
    public async Task Handle_ShouldReturnCorrectProjectData()
    {
         
        var project = new Project
        {
            Id = Guid.NewGuid(),
            Name = "Task Management",
            Description = "Clean Architecture API"
        };

        var query =
            new GetProjectByIdQuery(project.Id);

        _repositoryMock
      .Setup(x => x.GetByIdAsync(
          It.IsAny<Guid>(),
          It.IsAny<Expression<Func<Project, object>>[]>()))
      .ReturnsAsync(project);



         
        var result =
            await _handler.Handle(
                query,
                CancellationToken.None);

         
        result.Should().NotBeNull();

        result!.Name
            .Should()
            .Be("Task Management");

        result.Description
            .Should()
            .Be("Clean Architecture API");
    }
}