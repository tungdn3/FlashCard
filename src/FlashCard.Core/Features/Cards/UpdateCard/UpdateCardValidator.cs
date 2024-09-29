using FlashCard.Core.Interfaces.Repositories;
using FlashCard.Core.Models;
using FluentValidation;

namespace FlashCard.Core.Features.Cards;

public class UpdateCardValidator : AbstractValidator<UpdateCardRequest>
{
    public UpdateCardValidator(ICardRepository cardRepository)
    {
        RuleFor(x => x.CardId)
            .MustAsync(async (x, cardId, cancellationToken) =>
            {
                Card? card = await cardRepository.GetById(cardId);
                if (card == null)
                {
                    return false;
                }

                // Not allowed to change deck
                if (card.DeckId != x.DeckId)
                {
                    return false;
                }

                return true;
            })
            .WithMessage("Invalid CardId");

        RuleFor(x => x.Word)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Meaning)
            .NotEmpty()
            .MaximumLength(500);
    }
}
