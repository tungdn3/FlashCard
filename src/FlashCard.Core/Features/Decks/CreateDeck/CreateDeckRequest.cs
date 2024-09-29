using MediatR;

namespace FlashCard.Core.Features.Decks;

public class CreateDeckRequest : IRequest<int>
{
    public string Name { get; set; } = string.Empty;
}
