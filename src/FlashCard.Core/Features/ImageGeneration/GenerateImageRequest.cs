using MediatR;

namespace FlashCard.Core.Features.SentenceSuggestions;

public class GenerateImageRequest : IRequest<string>
{
    public string Word { get; set; } = string.Empty;
}
