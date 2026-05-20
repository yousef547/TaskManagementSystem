using MediatR;
using TaskManagement.Application.Common.Interfaces;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Repositories;

namespace TaskManagement.Application.Features.Projects.Commands.CreateProject
{
    public class CreateProjectCommandHandler
    : IRequestHandler<CreateProjectCommand, Guid>
    {
        private readonly IAsyncRepository<Project> _contextProject;
        
        public CreateProjectCommandHandler(
            IAsyncRepository<Project> contextProject)
        {
            _contextProject = contextProject;
        }

        public async Task<Guid> Handle(
            CreateProjectCommand request,
            CancellationToken cancellationToken)
        {
            var project = new Project
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
            };

            await _contextProject.AddAsync(
                project,
                cancellationToken);

            return project.Id;
        }
    }
}
