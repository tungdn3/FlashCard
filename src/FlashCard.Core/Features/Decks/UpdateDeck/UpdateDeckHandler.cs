using FlashCard.Core.Exceptions;
using FlashCard.Core.Interfaces.Repositories;
using FlashCard.Core.Models;
using FluentValidation;
using MediatR;

namespace FlashCard.Core.Features.Decks;

public class UpdateDeckHandler : IRequestHandler<UpdateDeckRequest>
{
    private readonly IValidator<UpdateDeckRequest> _validator;
    private readonly IDeckRepository _deckRepository;
    private readonly IIdentityRepository _identityRepository;

    public UpdateDeckHandler(IValidator<UpdateDeckRequest> validator, IDeckRepository deckRepository, IIdentityRepository identityRepository)
    {
        _validator = validator;
        _deckRepository = deckRepository;
        _identityRepository = identityRepository;
    }

    public async Task Handle(UpdateDeckRequest request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        string userId = _identityRepository.GetCurrentUserId();

        Deck? deck = await _deckRepository.GetById(request.Id)
            ?? throw new NotFoundException($"The given deck ID '{request.Id}' not found.");

        if (!deck.OwnerId.Equals(userId, StringComparison.OrdinalIgnoreCase))
        {
            throw new UnauthorizedException("You are not allowed to access this deck.");
        }

        deck.Name = request.Name;

        await _deckRepository.Update(deck);
    }
}
