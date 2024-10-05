using FlashCard.Core.Features.SentenceSuggestions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCard.Core.Interfaces.AIClients;

public interface IAIImageClient
{
    Task<string> GenerateImage(GenerateImageRequest request);
}
