using FluentValidation;
using movie_tracker.DTOs;
using movie_tracker.DTOs.Auth;

namespace movie_tracker.Validators.Auth;

public class RegisterDtoValidator : AbstractValidator<AuthRegisterDto>
{
    public RegisterDtoValidator()
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