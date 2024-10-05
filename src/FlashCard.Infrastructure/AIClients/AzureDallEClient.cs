using Azure.AI.OpenAI;
using FlashCard.Core.Features.SentenceSuggestions;
using FlashCard.Core.Interfaces.AIClients;
using Microsoft.Extensions.Options;
using OpenAI.Images;
using System.ClientModel;

namespace FlashCard.Infrastructure.AIClients;

internal class AzureDallEOptions
{
    public const string AzureDallE = "AzureDallE";
    public string Endpoint { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
    public string ModelName { get; set; } = string.Empty;
}

internal class AzureDallEClient : IAIImageClient
{
    private readonly ImageClient _imageClient;

    public AzureDallEClient(IOptions<AzureDallEOptions> options)
    {
        var azureClient = new AzureOpenAIClient(new Uri(options.Value.Endpoint), new ApiKeyCredential(options.Value.Key));
        _imageClient = azureClient.GetImageClient(options.Value.ModelName);
    }

    public async Task<string> GenerateImage(GenerateImageRequest request)
    {
        ClientResult<GeneratedImage> imageGeneration = await _imageClient.GenerateImageAsync(
            $"meaning of the word {request.Word}",
            new ImageGenerationOptions()
            {
                Size = GeneratedImageSize.W1024xH1024,
            }
        );

        return imageGeneration.Value.ImageUri.ToString();
    }
}
