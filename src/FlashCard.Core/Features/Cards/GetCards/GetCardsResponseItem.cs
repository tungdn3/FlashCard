namespace FlashCard.Core.Features.Cards;

public class GetCardsResponseItem
{
    public int Id { get; set; }

    public string Word { get; set; } = string.Empty;

    public string Meaning { get; set; } = string.Empty;

    public string? Example { get; set; }

    public string? ImageUrl { get; set; }
}
