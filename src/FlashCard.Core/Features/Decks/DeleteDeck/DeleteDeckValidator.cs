using FluentValidation;

namespace FlashCard.Core.Features.Decks;

public class DeleteDeckValidator : AbstractValidator<DeleteDeckRequest>
{
    public DeleteDeckValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
