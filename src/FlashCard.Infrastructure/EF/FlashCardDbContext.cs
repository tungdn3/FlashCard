using FlashCard.Core.Models;
using FlashCard.Infrastructure.EF.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace FlashCard.Infrastructure.EF
{
    public class FlashCardDbContext : DbContext
    {
        public DbSet<Deck> Decks { get; set; }

        public DbSet<Card> Cards { get; set; }

        public FlashCardDbContext(DbContextOptions<FlashCardDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new DeckConfiguration().Configure(modelBuilder.Entity<Deck>());
            new CardConfiguration().Configure(modelBuilder.Entity<Card>());
        }
    }
}
