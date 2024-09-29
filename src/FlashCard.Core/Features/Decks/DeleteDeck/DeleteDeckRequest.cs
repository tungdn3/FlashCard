using MediatR;

namespace FlashCard.Core.Features.Decks;

public class DeleteDeckRequest : IRequest
{
    public int Id { get; set; }
}
