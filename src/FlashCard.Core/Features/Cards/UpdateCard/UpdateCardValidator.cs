using FluentValidation;

namespace FlashCard.Core.Features.Cards;

public class UpdateCardValidator : AbstractValidator<UpdateCardRequest>
{
    public UpdateCardValidator()
    {
        RuleFor(x => x.DeckId)
            .NotEmpty();

        RuleFor(x => x.CardId)
            .NotEmpty();

        RuleFor(x => x.Word)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Meaning)
            .NotEmpty()
            .MaximumLength(500);

        RuleFor(x => x.Example)
            .MaximumLength(500);

        RuleFor(x => x.ImageUrl)
            .MaximumLength(2048);
    }
}
