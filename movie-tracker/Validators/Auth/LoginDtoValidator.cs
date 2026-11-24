using Business.DTOs.Auth;
using FluentValidation;

namespace movie_tracker.Validators.Auth;

public class LoginDtoValidator : AbstractValidator<AuthLoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Invalid email format.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required.");
    }
}