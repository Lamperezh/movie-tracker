using FluentValidation;
using movie_tracker.DTOs.Media;

namespace movie_tracker.Validators.Media;

public class UpdateMediaDtoValidator : AbstractValidator<UpdateMediaDto>
{
    public UpdateMediaDtoValidator()
    {
        RuleFor(x => x.OstLinks)
            .NotNull()
            .WithMessage("OST links cannot be null.");

        RuleForEach(x => x.OstLinks)
            .Must(link =>
                Uri.TryCreate(link, UriKind.Absolute, out var uri) &&
                (uri.Scheme == Uri.UriSchemeHttp ||
                 uri.Scheme == Uri.UriSchemeHttps))
            .WithMessage("Each OST link must be a valid HTTP or HTTPS URL.");
    }
}