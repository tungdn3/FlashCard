using FlashCard.Core.Dtos;
using FlashCard.Core.Features.Decks;
using FlashCard.Core.Models;

namespace FlashCard.Core.Interfaces.Repositories;

public interface IDeckRepository
{
    Task<int> Create(Deck deck);
    Task<bool> Exist(int deckId);
    Task<PageResultDto<Deck>> Get(GetDecksRequest request);
    Task<Deck?> GetById(int id);
    Task Update(Deck deck);
}
