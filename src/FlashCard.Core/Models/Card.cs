namespace FlashCard.Core.Models;

public class Card
{
    public int Id { get; set; }

    public string Word { get; set; } = string.Empty;

    public string Meaning { get; set; } = string.Empty;

    public bool IsDeleted { get; set; }

    public int DeckId { get; set; }

    public Deck Deck { get; set; }
}
