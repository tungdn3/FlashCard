using FlashCard.Core.Interfaces.Repositories;
using FlashCard.Core.Models;
using FluentValidation;
using MediatR;

namespace FlashCard.Core.Features.Decks;

public class DeleteDeckHandler : IRequestHandler<DeleteDeckRequest>
{
    private readonly IValidator<DeleteDeckRequest> _validator;
    private readonly IDeckRepository _deckRepository;

    public DeleteDeckHandler(IValidator<DeleteDeckRequest> validator, IDeckRepository deckRepository)
    {
        _validator = validator;
        _deckRepository = deckRepository;
    }

    public async Task Handle(DeleteDeckRequest request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        Deck deck = (await _deckRepository.GetById(request.Id))!;
        if (deck.IsDeleted)
        {
            return;
        }

        deck.IsDeleted = true;
        await _deckRepository.Update(deck);
    }
}
