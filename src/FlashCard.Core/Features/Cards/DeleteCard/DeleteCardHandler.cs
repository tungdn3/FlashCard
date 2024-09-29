using FlashCard.Core.Exceptions;
using FlashCard.Core.Interfaces.Repositories;
using FlashCard.Core.Models;
using FluentValidation;
using MediatR;

namespace FlashCard.Core.Features.Cards.CreateCard;

public class DeleteCardHandler : IRequestHandler<DeleteCardRequest>
{
    private readonly IValidator<DeleteCardRequest> _validator;
    private readonly ICardRepository _cardRepository;
    private readonly IIdentityRepository _identityRepository;
    private readonly IDeckRepository _deckRepository;

    public DeleteCardHandler(
        IValidator<DeleteCardRequest> validator,
        ICardRepository cardRepository,
        IIdentityRepository identityRepository,
        IDeckRepository deckRepository)
    {
        _validator = validator;
        _cardRepository = cardRepository;
        _identityRepository = identityRepository;
        _deckRepository = deckRepository;
    }

    public async Task Handle(DeleteCardRequest request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        string userId = _identityRepository.GetCurrentUserId();

        Deck deck = await _deckRepository.GetById(request.DeckId)
            ?? throw new NotFoundException($"The given deck ID '{request.DeckId}' not found.");

        if (!deck.OwnerId.Equals(userId, StringComparison.OrdinalIgnoreCase))
        {
            throw new UnauthorizedException("You are not allowed to access this deck.");
        }

        Card card = await _cardRepository.GetById(request.CardId, request.DeckId)
            ?? throw new NotFoundException($"The given card ID '{request.CardId}' not found in the deck.");

        if (card.IsDeleted)
        {
            return;
        }

        card.IsDeleted = true;
        await _cardRepository.Update(card);
    }
}
