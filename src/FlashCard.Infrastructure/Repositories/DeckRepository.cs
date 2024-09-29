using FlashCard.Core.Dtos;
using FlashCard.Core.Features.Decks;
using FlashCard.Core.Interfaces.Repositories;
using FlashCard.Core.Models;
using FlashCard.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;

namespace FlashCard.Infrastructure.Repositories;

internal class DeckRepository : IDeckRepository
{
    private readonly FlashCardDbContext _context;

    public DeckRepository(FlashCardDbContext context)
    {
        _context = context;
    }

    public async Task<int> Create(Deck deck)
    {
        _context.Decks.Add(deck);
        await _context.SaveChangesAsync();
        return deck.Id;
    }

    public Task<bool> Exist(int deckId)
    {
        return _context.Decks.AnyAsync(d => d.Id == deckId);
    }

    public async Task<PageResultDto<Deck>> Get(GetDecksRequest request)
    {
        IQueryable<Deck> query = _context.Decks
            .AsNoTracking()
            .Where(x => !x.IsDeleted);

        if (!string.IsNullOrEmpty(request.SearchText))
        {
            query = query.Where(x => x.Name.Contains(request.SearchText));
        }

        int total = await query.CountAsync();
        List<Deck> items = await query
            .OrderBy(x => x.Id)
            .Skip(request.PageSize * (request.PageNumber - 1))
            .Take(request.PageSize)
            .ToListAsync();

        return new PageResultDto<Deck>
        {
            Items = items,
            TotalCount = total,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
        };
    }

    public async Task<Deck?> GetById(int id)
    {
        Deck? deck = await _context.Decks.FindAsync(id);
        return deck;
    }

    public async Task Update(Deck deck)
    {
        _context.Decks.Update(deck);
        await _context.SaveChangesAsync();
    }
}
