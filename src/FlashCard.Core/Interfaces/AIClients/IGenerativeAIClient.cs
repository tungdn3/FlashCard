using FlashCard.Core.Features.SentenceSuggestions;

namespace FlashCard.Core.Interfaces.AIClients;

public interface IGenerativeAIClient
{
    Task<List<string>> GenerateSentences(GenerateSentenceRequest request);
}
