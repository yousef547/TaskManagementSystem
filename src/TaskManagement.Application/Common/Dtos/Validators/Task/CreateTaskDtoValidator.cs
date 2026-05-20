using FluentValidation;
using TaskManagement.Application.Common.Dtos.Requests.Task;

namespace TaskManagement.Application.Common.Dtos.Validators.Task;

public class CreateTaskDtoValidator
    : AbstractValidator<CreateTaskDto>
{
    public CreateTaskDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required")
            .MinimumLength(3)
            .WithMessage(
                "Title must be at least 3 characters")
            .MaximumLength(100)
            .WithMessage(
                "Title cannot exceed 100 characters");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required")
            .MinimumLength(10)
            .WithMessage(
                "Description must be at least 10 characters")
            .MaximumLength(500)
            .WithMessage(
                "Description cannot exceed 500 characters");

        RuleFor(x => x.ProjectId)
            .NotEmpty()
            .WithMessage("Project Id is required");

        RuleFor(x => x.DueDate)
            .GreaterThan(DateTime.UtcNow)
            .WithMessage(
                "Due date must be greater than current date");

        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage("Invalid task status");

        RuleFor(x => x.Priority)
            .IsInEnum()
            .WithMessage("Invalid task priority");
    }
}