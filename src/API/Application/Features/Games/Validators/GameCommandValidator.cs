using System.Globalization;
using FluentValidation;
using GameService.Application.Constant;
using GameService.Application.Features.Games.Command;
using GameService.Application.Features.Games.DTO;

namespace GameService.Application.Features.Games.Validators;

public sealed class GameCommandValidator : AbstractValidator<CommandCreateGame>
{
    public GameCommandValidator()
    {
        RuleFor(x => x.Price)
            .NotNull().WithMessage(x => string.Format(ValidationMessage.Field.FieldRequired, nameof(x.Price)))
            .GreaterThanOrEqualTo(0);
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage(x => string.Format(ValidationMessage.Field.FieldNotEmpty, nameof(x.Title)));
        RuleFor(x => x.Description)
            .MaximumLength(100).WithMessage(x => string.Format(ValidationMessage.Field.FieldMaxLength, nameof(x.Description), 100));
        RuleFor(x => x.ReleaseDate)
            .Must(date => string.IsNullOrWhiteSpace(date) || BeAValidDate(date))
            .WithMessage("Release date must be in format dd/MM/yyyy if provided.");
    }
    
    private bool BeAValidDate(string date)
    {
        return DateTime.TryParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
    }
}