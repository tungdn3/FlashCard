using MediatR;
using System.Text.Json.Serialization;

namespace FlashCard.Core.Features.Decks;

public class UpdateDeckRequest : IRequest
{
    [JsonIgnore]
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;
}
