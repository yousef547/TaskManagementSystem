using FluentValidation;
using TaskManagement.Application.Common.Dtos.Requests.Task;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.Common.Dtos.Validators.Task;

public class UpdateTaskStatusDtoValidator
    : AbstractValidator<UpdateTaskStatusDto>
{
    public UpdateTaskStatusDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Task Id is required");

        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage("Invalid task status");
    }
}