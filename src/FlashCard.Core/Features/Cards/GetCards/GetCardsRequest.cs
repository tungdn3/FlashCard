using MediatR;

namespace FlashCard.Core.Features.Cards;

public class GetCardsRequest : IRequest<IEnumerable<GetCardsResponseItem>>
{
    public int DeckId { get; set; }
}
