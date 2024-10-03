using MediatR;
using System.Text.Json.Serialization;

namespace FlashCard.Core.Features.Cards;

public class UpdateCardRequest : IRequest
{
    [JsonIgnore]
    public int DeckId { get; set; }

    [JsonIgnore]
    public int CardId { get; set; }

    public string Word { get; set; } = string.Empty;

    public string Meaning { get; set; } = string.Empty;

    public string? Example { get; set; }
}
