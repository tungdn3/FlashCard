using FlashCard.Core.Interfaces.Repositories;
using FluentValidation;

namespace FlashCard.Core.Features.Decks;

public class DeleteDeckValidator : AbstractValidator<DeleteDeckRequest>
{
    public DeleteDeckValidator(IDeckRepository deckRepository)
    {
        RuleFor(x => x.Id)
            .MustAsync((id, cancellationToken) => deckRepository.Exist(id))
            .WithMessage("The given Deck ID not exist.");
    }
}
