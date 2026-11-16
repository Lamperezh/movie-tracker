using FluentValidation;
using movie_tracker.DTOs.User;

namespace movie_tracker.Validators.User;

public class UpdateUserValidator : AbstractValidator<UpdateUserDto>
{
    public UpdateUserValidator()
    {
        RuleFor(user => user.UserName)
            .Must(value => !string.IsNullOrWhiteSpace(value))
            .WithMessage("Username is required.")
            .MaximumLength(256)
            .WithMessage("Username cannot exceed 256 characters.");

        RuleFor(user => user.Email)
            .Must(value => !string.IsNullOrWhiteSpace(value))
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Invalid email address.")
            .MaximumLength(256)
            .WithMessage("Email cannot exceed 256 characters.");
    }
}