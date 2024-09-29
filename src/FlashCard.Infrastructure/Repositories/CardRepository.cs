using FlashCard.Core.Features.Cards;
using FlashCard.Core.Interfaces.Repositories;
using FlashCard.Core.Models;
using FlashCard.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;

namespace FlashCard.Infrastructure.Repositories;

public class CardRepository : ICardRepository
{
    private readonly FlashCardDbContext _context;

    public CardRepository(FlashCardDbContext context)
    {
        _context = context;
    }

    public async Task<int> Create(Card card)
    {
        _context.Cards.Add(card);
        await _context.SaveChangesAsync();
        return card.Id;
    }

    public Task<List<Card>> Get(GetCardsRequest request)
    {
        return _context.Cards.Where(x => x.DeckId == request.DeckId).ToListAsync();
    }
}
