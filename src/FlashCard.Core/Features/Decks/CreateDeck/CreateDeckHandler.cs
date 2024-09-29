using FlashCard.Core.Interfaces.Repositories;
using FlashCard.Core.Models;
using FluentValidation;
using MediatR;

namespace FlashCard.Core.Features.Decks;

public class CreateDeckHandler : IRequestHandler<CreateDeckRequest, int>
{
    private readonly IValidator<CreateDeckRequest> _validator;
    private readonly IDeckRepository _deckRepository;
    private readonly IIdentityRepository _identityRepository;

    public CreateDeckHandler(IValidator<CreateDeckRequest> validator, IDeckRepository deckRepository, IIdentityRepository identityRepository)
    {
        _validator = validator;
        _deckRepository = deckRepository;
        _identityRepository = identityRepository;
    }

    public async Task<int> Handle(CreateDeckRequest request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        string userId = _identityRepository.GetCurrentUserId();

        Deck deck = new()
        {
            Name = request.Name,
            OwnerId = userId,
        };

        int id = await _deckRepository.Create(deck);

        return id;
    }
}
