using FlashCard.Core.Dtos;
using MediatR;

namespace FlashCard.Core.Features.Decks;

public class GetDecksRequest : IRequest<PageResultDto<GetDecksResponseItem>>
{
    public string? SearchText { get; set; }

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}
