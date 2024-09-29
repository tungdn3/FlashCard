using MediatR;
using System.Text.Json.Serialization;

namespace FlashCard.Core.Features.Cards;

public class CreateCardRequest : IRequest<int>
{
    [JsonIgnore]
    public int DeckId { get; set; }

    public string Word { get; set; } = string.Empty;

    public string Meaning { get; set; } = string.Empty;
}
