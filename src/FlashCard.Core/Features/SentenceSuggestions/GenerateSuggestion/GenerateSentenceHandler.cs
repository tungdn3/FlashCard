using FlashCard.Core.Features.SentenceSuggestions;
using FlashCard.Core.Interfaces;
using FluentValidation;
using MediatR;

namespace FlashCard.Core.Features.Cards.CreateCard;

public class GenerateSentenceHandler : IRequestHandler<GenerateSentenceRequest, IEnumerable<string>>
{
    private readonly IValidator<GenerateSentenceRequest> _validator;
    private readonly IGenerativeAIClient _aiClient;

    public GenerateSentenceHandler(
        IValidator<GenerateSentenceRequest> validator,
        IGenerativeAIClient aiClient)
    {
        _validator = validator;
        _aiClient = aiClient;
    }

    public async Task<IEnumerable<string>> Handle(GenerateSentenceRequest request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        List<string> sentences = await _aiClient.GenerateSentences(request);

        return sentences;
    }
}
