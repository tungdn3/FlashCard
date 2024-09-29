using FlashCard.Core.Features.Cards;
using FlashCard.Core.Models;

namespace FlashCard.Core.Interfaces.Repositories;

public interface ICardRepository
{
    Task<List<Card>> Get(GetCardsRequest request);

    Task<int> Create(Card card);
}
