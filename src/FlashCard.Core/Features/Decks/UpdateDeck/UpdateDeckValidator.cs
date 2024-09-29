using FluentValidation;

namespace FlashCard.Core.Features.Decks;

public class UpdateDeckValidator : AbstractValidator<UpdateDeckRequest>
{
    public UpdateDeckValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);
    }
}
