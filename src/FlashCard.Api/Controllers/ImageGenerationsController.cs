using FlashCard.Core.Features.SentenceSuggestions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlashCard.Api.Controllers;

[Authorize]
[ApiController]
[Route("v1/image-generations")]
public class ImageGenerationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ImageGenerationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost(Name = "GenerateImage")]
    public async Task<string> GenerateSentence([FromBody] GenerateImageRequest request)
    {
        string imageUrl = await _mediator.Send(request);
        return imageUrl;
    }
}
