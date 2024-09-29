using FlashCard.Core.Models;

namespace FlashCard.Infrastructure.EF;

public static class DbInitializer
{
    public static void Initialize(FlashCardDbContext context)
    {
        if (context.Decks.Any())
        {
            return;   // DB has been seeded
        }

        var decks = new Deck[]
        {
            new Deck { 
                Name = "Deck 1" ,
                OwnerId = "4af8b4a5-1e2a-439a-9b92-e3bf116d65a3", // alice/Pass123$
            },
        };

        context.Decks.AddRange(decks);
        context.SaveChanges();

        var cards = new Card[]
        {
            new Card
            {
                DeckId = decks[0].Id,
                Word = "avocado",
                Meaning = "a tropical fruit with hard, dark green skin and a soft, light green inside part that does not taste sweet and contains a large seed"
            },
            new Card
            {
                DeckId = decks[0].Id,
                Word = "apple",
                Meaning = "a round fruit with shiny red or green skin that is fairly hard and white inside"
            },
            new Card
            {
                DeckId = decks[0].Id,
                Word = "mango",
                Meaning = "a tropical fruit with smooth yellow or red skin, that is soft and orange inside with a large seed"
            },
        };

        context.Cards.AddRange(cards);
        context.SaveChanges();
    }
}
