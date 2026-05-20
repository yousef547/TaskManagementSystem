using FluentValidation;
using TaskManagement.Application.Common.Dtos.Requests.Auth;

namespace TaskManagement.Application.Common.Dtos.Validators.Auth;

public class RegisterRequestDtoValidator
    : AbstractValidator<RegisterDto>
{
    public RegisterRequestDtoValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty()
            .WithMessage("Full name is required")
            .MinimumLength(3)
            .WithMessage(
                "Full name must be at least 3 characters");

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
                "Password must be at least 6 characters")
            .Matches("[A-Z]")
            .WithMessage(
                "Password must contain at least one uppercase letter")
            .Matches("[a-z]")
            .WithMessage(
                "Password must contain at least one lowercase letter")
            .Matches("[0-9]")
            .WithMessage(
                "Password must contain at least one number");

        RuleFor(x => x.Role)
            .NotEmpty()
            .WithMessage("Role is required")
            .Must(role =>
                role == "Admin" ||
                role == "Manager" ||
                role == "User")
            .WithMessage(
                "Role must be Admin, Manager, or User");
    }
}