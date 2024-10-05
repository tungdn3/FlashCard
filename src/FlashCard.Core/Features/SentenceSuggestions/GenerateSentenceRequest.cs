using MediatR;

namespace FlashCard.Core.Features.SentenceSuggestions;

public class GenerateSentenceRequest : IRequest<IEnumerable<string>>
{
    public string Word { get; set; } = string.Empty;
}
