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
        return _context.Cards
            .Where(x => x.DeckId == request.DeckId && !x.IsDeleted)
            .ToListAsync();
    }

    public async Task<Card?> GetById(int cardId)
    {
        Card? card = await _context.Cards.FindAsync(cardId);
        return card;
    }

    public async Task Update(Card card)
    {
        _context.Cards.Update(card);
        await _context.SaveChangesAsync();
    }
}
