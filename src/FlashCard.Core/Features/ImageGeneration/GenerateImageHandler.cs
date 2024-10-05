using FlashCard.Core.Features.SentenceSuggestions;
using FlashCard.Core.Interfaces;
using FlashCard.Core.Interfaces.AIClients;
using FluentValidation;
using MediatR;

namespace FlashCard.Core.Features.Cards.CreateCard;

public class GenerateImageHandler : IRequestHandler<GenerateImageRequest, string>
{
    private readonly IValidator<GenerateImageRequest> _validator;
    private readonly IAIImageClient _aiClient;

    public GenerateImageHandler(IValidator<GenerateImageRequest> validator, IAIImageClient aiClient)
    {
        _validator = validator;
        _aiClient = aiClient;
    }

    public async Task<string> Handle(GenerateImageRequest request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        string imageUrl = await _aiClient.GenerateImage(request);

        return imageUrl;
    }
}
