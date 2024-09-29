using FlashCard.Core.Interfaces.Repositories;
using FlashCard.Core.Models;
using FluentValidation;
using MediatR;

namespace FlashCard.Core.Features.Decks;

public class UpdateDeckHandler : IRequestHandler<UpdateDeckRequest>
{
    private readonly IValidator<UpdateDeckRequest> _validator;
    private readonly IDeckRepository _deckRepository;

    public UpdateDeckHandler(IValidator<UpdateDeckRequest> validator, IDeckRepository deckRepository)
    {
        _validator = validator;
        _deckRepository = deckRepository;
    }

    public async Task Handle(UpdateDeckRequest request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        Deck deck = (await _deckRepository.GetById(request.Id))!;

        deck.Name = request.Name;

        await _deckRepository.Update(deck);
    }
}
