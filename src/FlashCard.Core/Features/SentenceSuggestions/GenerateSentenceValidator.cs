using FluentValidation;

namespace FlashCard.Core.Features.SentenceSuggestions;

public class GenerateSentenceValidator : AbstractValidator<GenerateSentenceRequest>
{
    public GenerateSentenceValidator()
    {
        RuleFor(x => x.Word)
            .NotEmpty()
            .MaximumLength(100);
    }
}
