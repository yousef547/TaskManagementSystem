using FluentAssertions;
using Moq;
using TaskManagement.Application.Common.Interfaces;
using TaskManagement.Application.Features.Projects.Queries.GetAllProjects;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Repositories;

namespace TaskManagement.Application.UnitTests
    .Features.Projects.Queries;

public class GetAllProjectsQueryHandlerTests
{
    private readonly Mock<IAsyncRepository<Project>>
        _repositoryMock;

    private readonly Mock<ICacheService>
        _cacheServiceMock;

    private readonly GetAllProjectsQueryHandler
        _handler;

    public GetAllProjectsQueryHandlerTests()
    {
        _repositoryMock =
            new Mock<IAsyncRepository<Project>>();

        _cacheServiceMock =
            new Mock<ICacheService>();

        _handler =
            new GetAllProjectsQueryHandler(
                _repositoryMock.Object,
                _cacheServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnProjectsFromCache_WhenCacheExists()
    {
         
        var projects = new List<Project>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Project 1"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Project 2"
            }
        };

        _cacheServiceMock
            .Setup(x => x.GetAsync<List<Project>>(
                It.IsAny<string>()))
            .ReturnsAsync(projects);

        var query =
            new GetAllProjectsQuery(Guid.NewGuid());

         
        var result =
            await _handler.Handle(
                query,
                CancellationToken.None);

         
        result.Should().HaveCount(2);

        _repositoryMock.Verify(
            x => x.GetAllAsync(),
            Times.Never);
    }





    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoProjectsExist()
    {
         
        var projects = new List<Project>();

        _cacheServiceMock
            .Setup(x => x.GetAsync<List<Project>>(
                It.IsAny<string>()))
            .ReturnsAsync((List<Project>?)null);

        _repositoryMock
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(projects);

        var query =
            new GetAllProjectsQuery(Guid.NewGuid());

         
        var result =
            await _handler.Handle(
                query,
                CancellationToken.None);

         
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_ShouldCallRepositoryOnce_WhenCacheIsEmpty()
    {
        _cacheServiceMock
            .Setup(x => x.GetAsync<List<Project>>(
                It.IsAny<string>()))
            .ReturnsAsync((List<Project>?)null);

        _repositoryMock
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(new List<Project>());

        var query =
            new GetAllProjectsQuery(Guid.NewGuid());

        await _handler.Handle(
            query,
            CancellationToken.None);

        _repositoryMock.Verify(
            x => x.GetAllAsync(),
            Times.Once);
    }


    [Fact]
    public async Task Handle_ShouldThrowException_WhenRepositoryFails()
    {
         
        _cacheServiceMock
            .Setup(x => x.GetAsync<List<Project>>(
                It.IsAny<string>()))
            .ReturnsAsync((List<Project>?)null);

        _repositoryMock
            .Setup(x => x.GetAllAsync())
            .ThrowsAsync(
                new Exception("Database error"));

        var query =
            new GetAllProjectsQuery(Guid.NewGuid());

         
        Func<Task> act = async () =>
            await _handler.Handle(
                query,
                CancellationToken.None);

         
        await act.Should()
            .ThrowAsync<Exception>()
            .WithMessage("Database error");
    }


}