using FlashCard.Core.Features.SentenceSuggestions;
using FluentValidation;

namespace FlashCard.Core.Features.Cards;

public class GenerateImageValidator : AbstractValidator<GenerateImageRequest>
{
    public GenerateImageValidator()
    {
        RuleFor(x => x.Word)
            .NotEmpty()
            .MaximumLength(100);
    }
}
