using Azure.AI.OpenAI;
using FlashCard.Core.Features.SentenceSuggestions;
using FlashCard.Core.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OpenAI.Chat;
using System.ClientModel;

namespace FlashCard.Infrastructure.AIClients;

internal class AzureChatCompleteOptions
{
    public const string AzureChatComplete = "AzureChatComplete";
    public string Endpoint { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
    public string ModelName { get; set; } = string.Empty;
    public string SystemMessage { get; set; } = "You are an online English tutor application. Your users will send you a word. Help them to make two sentences using the given word. A machine will read your answer. Therefore, please return it in JSON format. For example, [\"I love apple\", \"I took a bite of the apple\"]";
}

internal class AzureChatCompleteClient : IGenerativeAIClient
{
    private readonly AzureChatCompleteOptions _options;
    private readonly ChatClient _chatClient;

    public AzureChatCompleteClient(IOptions<AzureChatCompleteOptions> options)
    {
        _options = options.Value;
        var azureClient = new AzureOpenAIClient(new Uri(_options.Endpoint), new ApiKeyCredential(_options.Key));
        _chatClient = azureClient.GetChatClient(_options.ModelName);
    }

    public async Task<List<string>> GenerateSentences(GenerateSentenceRequest request)
    {
        ChatCompletion completion = await _chatClient.CompleteChatAsync(
        [
            new SystemChatMessage(_options.SystemMessage),
            new UserChatMessage(request.Word)
        ]);

        string? answer = completion.Content[0].Text;
        if (string.IsNullOrEmpty(answer))
        {
            return new List<string>();
        }

        List<string> sentences = JsonConvert.DeserializeObject<List<string>>(answer) ?? new();
        return sentences;
    }
}
