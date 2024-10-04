using FlashCard.Core.Exceptions;
using FlashCard.Core.Interfaces.Repositories;
using FlashCard.Core.Models;
using FluentValidation;
using MediatR;

namespace FlashCard.Core.Features.Decks;

public class DeleteDeckHandler : IRequestHandler<DeleteDeckRequest>
{
    private readonly IValidator<DeleteDeckRequest> _validator;
    private readonly IDeckRepository _deckRepository;
    private readonly IIdentityRepository _identityRepository;

    public DeleteDeckHandler(IValidator<DeleteDeckRequest> validator, IDeckRepository deckRepository, IIdentityRepository identityRepository)
    {
        _validator = validator;
        _deckRepository = deckRepository;
        _identityRepository = identityRepository;
    }

    public async Task Handle(DeleteDeckRequest request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        string userId = _identityRepository.GetCurrentUserId();

        Deck? deck = await _deckRepository.GetById(request.Id)
            ?? throw new NotFoundException($"The given deck ID '{request.Id}' not found.");

        if (!deck.OwnerId.Equals(userId, StringComparison.OrdinalIgnoreCase))
        {
            throw new ForbiddenException("You are not allowed to access this deck.");
        }

        if (deck.IsDeleted)
        {
            return;
        }

        deck.IsDeleted = true;
        await _deckRepository.Update(deck);
    }
}
