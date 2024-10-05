using FlashCard.Core.Exceptions;
using FlashCard.Core.Interfaces.Repositories;
using FlashCard.Core.Models;
using FluentValidation;
using MediatR;

namespace FlashCard.Core.Features.Cards.CreateCard;

public class CreateCardHandler : IRequestHandler<CreateCardRequest, int>
{
    private readonly IValidator<CreateCardRequest> _validator;
    private readonly ICardRepository _cardRepository;
    private readonly IIdentityRepository _identityRepository;
    private readonly IDeckRepository _deckRepository;

    public CreateCardHandler(
        IValidator<CreateCardRequest> validator,
        ICardRepository cardRepository,
        IIdentityRepository identityRepository,
        IDeckRepository deckRepository)
    {
        _validator = validator;
        _cardRepository = cardRepository;
        _identityRepository = identityRepository;
        _deckRepository = deckRepository;
    }

    public async Task<int> Handle(CreateCardRequest request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        string userId = _identityRepository.GetCurrentUserId();

        Deck? deck = await _deckRepository.GetById(request.DeckId)
            ?? throw new NotFoundException($"The given deck ID '{request.DeckId}' not found.");

        if (!deck.OwnerId.Equals(userId, StringComparison.OrdinalIgnoreCase))
        {
            throw new ForbiddenException("You are not allowed to access this deck.");
        }

        var card = new Card
        {
            DeckId = request.DeckId,
            Word = request.Word,
            Meaning = request.Meaning,
            Example = request.Example,
            ImageUrl = request.ImageUrl,
        };

        int id = await _cardRepository.Create(card);
        return id;
    }
}
