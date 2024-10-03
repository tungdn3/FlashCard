using FlashCard.Core.Features.SentenceSuggestions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlashCard.Api.Controllers;

[Authorize]
[ApiController]
[Route("v1/sentence-suggestions")]
public class SentenceSuggestionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public SentenceSuggestionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost(Name = "GenerateSentence")]
    public async Task<IEnumerable<string>> GenerateSentence([FromBody] GenerateSentenceRequest request)
    {
        IEnumerable<string> sentences = await _mediator.Send(request);
        return sentences;
    }
}
