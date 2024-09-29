using FlashCard.Core.Dtos;
using FlashCard.Core.Features.Cards;
using FlashCard.Core.Features.Decks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FlashCard.Api.Controllers
{
    [ApiController]
    [Route("api/v1/decks")]
    public class DecksController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DecksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(Name = "GetDecks")]
        public async Task<PageResultDto<GetDecksResponseItem>> GetDecks([FromQuery] GetDecksRequest request)
        {
            PageResultDto<GetDecksResponseItem> result = await _mediator.Send(request);
            return result;
        }

        [HttpPost(Name = "CreateDeck")]
        public async Task<int> CreateDeck([FromBody] CreateDeckRequest request)
        {
            int id = await _mediator.Send(request);
            return id;
        }

        [HttpPut("{id}", Name = "UpdateDeck")]
        public async Task UpdateDeck(int id, [FromBody] UpdateDeckRequest request)
        {
            request.Id = id;
            await _mediator.Send(request);
        }

        [HttpDelete("{id}", Name = "DeleteDeck")]
        public async Task DeleteDeck(int id)
        {
            await _mediator.Send(new DeleteDeckRequest
            {
                Id = id,
            });
        }

        [HttpGet("{id}/cards", Name = "GetCards")]
        public async Task<IEnumerable<GetCardsResponseItem>> GetCards(int id)
        {
            var request = new GetCardsRequest
            {
                DeckId = id,
            };

            IEnumerable<GetCardsResponseItem> result = await _mediator.Send(request);
            return result;
        }

        [HttpPost("{id}/cards", Name = "CreateCard")]
        public async Task<int> CreateCard(int id, [FromBody] CreateCardRequest request)
        {
            request.DeckId = id;
            int cardId = await _mediator.Send(request);
            return cardId;
        }
    }
}
