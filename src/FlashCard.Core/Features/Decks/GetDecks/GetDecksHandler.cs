using FlashCard.Core.Dtos;
using FlashCard.Core.Interfaces.Repositories;
using FlashCard.Core.Models;
using MediatR;

namespace FlashCard.Core.Features.Decks;

public class GetDecksHandler : IRequestHandler<GetDecksRequest, PageResultDto<GetDecksResponseItem>>
{
    private readonly IDeckRepository _deckRepository;
    private readonly IIdentityRepository _identityRepository;

    public GetDecksHandler(IDeckRepository deckRepository, IIdentityRepository identityRepository)
    {
        _deckRepository = deckRepository;
        _identityRepository = identityRepository;
    }

    public async Task<PageResultDto<GetDecksResponseItem>> Handle(GetDecksRequest request, CancellationToken cancellationToken)
    {
        string userId = _identityRepository.GetCurrentUserId();

        PageResultDto<Deck> entityResults = await _deckRepository.Get(request, userId);

        var result = new PageResultDto<GetDecksResponseItem>
        {
            Items = entityResults.Items.Select(x => new GetDecksResponseItem
            {
                Id = x.Id,
                Name = x.Name,
            }),
            PageNumber = entityResults.PageNumber,
            PageSize = entityResults.PageSize,
            TotalCount = entityResults.TotalCount,
        };

        return result;
    }
}
