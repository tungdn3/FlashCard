using FluentValidation;

namespace FlashCard.Core.Features.Decks;

public class CreateDeckValidator : AbstractValidator<CreateDeckRequest>
{
    public CreateDeckValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);
    }
}
