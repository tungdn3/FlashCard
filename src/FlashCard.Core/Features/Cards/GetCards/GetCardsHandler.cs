using FlashCard.Core.Interfaces.Repositories;
using MediatR;

namespace FlashCard.Core.Features.Cards.GetCards;

public class GetCardsHandler : IRequestHandler<GetCardsRequest, IEnumerable<GetCardsResponseItem>>
{
    private readonly ICardRepository _cardRepository;

    public GetCardsHandler(ICardRepository cardRepository)
    {
        _cardRepository = cardRepository;
    }

    public async Task<IEnumerable<GetCardsResponseItem>> Handle(GetCardsRequest request, CancellationToken cancellationToken)
    {
        List<Models.Card> entities = await _cardRepository.Get(request);
        
        List<GetCardsResponseItem> dtos = entities
            .Select(x => new GetCardsResponseItem
            {
                Id = x.Id,
                Meaning = x.Meaning,
                Word = x.Word,
            })
            .ToList();

        return dtos;
    }
}
