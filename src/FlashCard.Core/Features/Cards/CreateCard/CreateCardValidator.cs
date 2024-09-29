using FlashCard.Core.Interfaces.Repositories;
using FluentValidation;

namespace FlashCard.Core.Features.Cards;

public class CreateCardValidator : AbstractValidator<CreateCardRequest>
{
    public CreateCardValidator(IDeckRepository deckRepository)
    {
        RuleFor(x => x.DeckId)
            .MustAsync((deckId, cancellationToken) =>
            {
                return deckRepository.Exist(deckId);
            })
            .WithMessage("Invalid DeckId");

        // Todo: limit the number of cards per deck

        RuleFor(x => x.Word)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Meaning)
            .NotEmpty()
            .MaximumLength(500);
    }
}
