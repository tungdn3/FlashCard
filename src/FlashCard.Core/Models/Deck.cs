namespace FlashCard.Core.Models;

public class Deck
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public bool IsDeleted { get; set; }

    public string OwnerId { get; set; } = string.Empty;

    public ICollection<Card> Cards { get; set; } = new List<Card>();
}
