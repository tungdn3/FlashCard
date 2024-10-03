﻿using FlashCard.Core.Features.SentenceSuggestions;
using FluentValidation;

namespace FlashCard.Core.Features.Cards;

public class GenerateSentenceValidator : AbstractValidator<GenerateSentenceRequest>
{
    public GenerateSentenceValidator()
    {
        RuleFor(x => x.Word)
            .NotEmpty()
            .MaximumLength(100);
    }
}
