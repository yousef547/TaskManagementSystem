using FluentValidation;
using TaskManagement.Application.Common.Dtos.Requests.Auth;

namespace TaskManagement.Application.Common.Dtos.Validators.Auth;

public class LoginRequestDtoValidator
    : AbstractValidator<LogintDto>
{
    public LoginRequestDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .EmailAddress()
            .WithMessage("Invalid email format");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required")
            .MinimumLength(6)
            .WithMessage(
                "Password must be at least 6 characters");
    }
}