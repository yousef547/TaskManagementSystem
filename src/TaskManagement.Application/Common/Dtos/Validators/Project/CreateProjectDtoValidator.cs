using FluentValidation;
using TaskManagement.Application.Common.Dtos.Requests.Project;

namespace TaskManagement.Application.Common.Dtos.Validators.Project;

public class CreateProjectDtoValidator
    : AbstractValidator<CreateProjectDto>
{
    public CreateProjectDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Project name is required")
            .MinimumLength(3)
            .WithMessage(
                "Project name must be at least 3 characters")
            .MaximumLength(100)
            .WithMessage(
                "Project name cannot exceed 100 characters");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required")
            .MinimumLength(10)
            .WithMessage(
                "Description must be at least 10 characters")
            .MaximumLength(500)
            .WithMessage(
                "Description cannot exceed 500 characters");
    }
}