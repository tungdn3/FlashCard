using MediatR;
using System.Text.Json.Serialization;

namespace FlashCard.Core.Features.Cards;

public class DeleteCardRequest : IRequest
{
    [JsonIgnore]
    public int DeckId { get; set; }

    [JsonIgnore]
    public int CardId { get; set; }
}
