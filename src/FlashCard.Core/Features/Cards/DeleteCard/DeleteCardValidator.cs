using FlashCard.Core.Interfaces.Repositories;
using FluentValidation;

namespace FlashCard.Core.Features.Cards;

public class DeleteCardValidator : AbstractValidator<DeleteCardRequest>
{
    public DeleteCardValidator(ICardRepository cardRepository)
    {
        RuleFor(x => x.CardId)
            .NotEmpty();

        RuleFor(x => x.DeckId)
            .NotEmpty();
    }
}
