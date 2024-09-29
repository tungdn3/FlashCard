using FlashCard.Core.Interfaces.Repositories;
using FlashCard.Core.Models;
using FluentValidation;
using MediatR;

namespace FlashCard.Core.Features.Decks;

public class CreateDeckHandler : IRequestHandler<CreateDeckRequest, int>
{
    private readonly IValidator<CreateDeckRequest> _validator;
    private readonly IDeckRepository _deckRepository;

    public CreateDeckHandler(IValidator<CreateDeckRequest> validator, IDeckRepository deckRepository)
    {
        _validator = validator;
        _deckRepository = deckRepository;
    }

    public async Task<int> Handle(CreateDeckRequest request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        Deck deck = new Deck
        {
            Name = request.Name,
        };

        int id = await _deckRepository.Create(deck);

        return id;
    }
}
