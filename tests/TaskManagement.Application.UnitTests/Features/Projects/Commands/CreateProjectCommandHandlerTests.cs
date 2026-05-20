using FluentAssertions;
using MediatR;
using Moq;
using TaskManagement.Application.Common.Dtos.Requests.Project;
using TaskManagement.Application.Common.Dtos.Validators.Project;
using TaskManagement.Application.Common.Responses;
using TaskManagement.Application.Features.Projects.Commands.CreateProject;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Repositories;

namespace TaskManagement.Application.UnitTests
    .Features.Projects.Commands;

public class CreateProjectCommandHandlerTests
{
    private readonly Mock<IAsyncRepository<Project>>
        _repositoryMock;

    private readonly CreateProjectCommandHandler
        _handler;

    public CreateProjectCommandHandlerTests()
    {
        _repositoryMock =
            new Mock<IAsyncRepository<Project>>();

        _handler =
            new CreateProjectCommandHandler(
                _repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldCreateProject_AndReturnId()
    {
         
        var command =
            new CreateProjectCommand(
                "E-Commerce",
                "Backend API");

         
        var result =
            await _handler.Handle(
                command,
                CancellationToken.None);

         
        result.Should().NotBeEmpty();

        _repositoryMock.Verify(
            x => x.AddAsync(
                It.IsAny<Project>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public void Validate_ShouldFail_WhenNameIsEmpty()
    {
         
        var validator =
            new CreateProjectDtoValidator();

        var dto =
            new CreateProjectDto
            {
                Name = "",
                Description = "Backend API"
            };

         
        var result =
            validator.Validate(dto);

         
        result.IsValid.Should().BeFalse();

        result.Errors.Should()
            .Contain(x =>
                x.PropertyName == "Name");
    }


    [Fact]
    public async Task Handle_ShouldThrowException_WhenRepositoryFails()
    {
         
        var command =
            new CreateProjectCommand(
                "Project",
                "Description");

        _repositoryMock
            .Setup(x => x.AddAsync(
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