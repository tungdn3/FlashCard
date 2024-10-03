using FlashCard.Core.Exceptions;
using FlashCard.Core.Interfaces.Repositories;
using FlashCard.Core.Models;
using MediatR;

namespace FlashCard.Core.Features.Cards.GetCards;

public class GetCardsHandler : IRequestHandler<GetCardsRequest, IEnumerable<GetCardsResponseItem>>
{
    private readonly ICardRepository _cardRepository;
    private readonly IIdentityRepository _identityRepository;
    private readonly IDeckRepository _deckRepository;

    public GetCardsHandler(
        ICardRepository cardRepository,
        IIdentityRepository identityRepository,
        IDeckRepository deckRepository)
    {
        _cardRepository = cardRepository;
        _identityRepository = identityRepository;
        _deckRepository = deckRepository;
    }

    public async Task<IEnumerable<GetCardsResponseItem>> Handle(GetCardsRequest request, CancellationToken cancellationToken)
    {
        string userId = _identityRepository.GetCurrentUserId();

        Deck deck = await _deckRepository.GetById(request.DeckId)
            ?? throw new NotFoundException($"The given deck ID '{request.DeckId}' not found.");

        if (!deck.OwnerId.Equals(userId, StringComparison.OrdinalIgnoreCase))
        {
            throw new UnauthorizedException("You are not allowed to access this deck.");
        }

        List<Card> entities = await _cardRepository.Get(request);
        
        List<GetCardsResponseItem> dtos = entities
            .Select(x => new GetCardsResponseItem
            {
                Id = x.Id,
                Meaning = x.Meaning,
                Word = x.Word,
                Example = x.Example,
            })
            .ToList();

        return dtos;
    }
}
