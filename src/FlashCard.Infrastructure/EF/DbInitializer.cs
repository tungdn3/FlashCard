using FlashCard.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            new Deck { Name = "Deck 1" },
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
