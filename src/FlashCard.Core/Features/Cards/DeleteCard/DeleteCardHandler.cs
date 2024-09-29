using FlashCard.Core.Interfaces.Repositories;
using FlashCard.Core.Models;
using FluentValidation;
using MediatR;

namespace FlashCard.Core.Features.Cards.CreateCard;

public class DeleteCardHandler : IRequestHandler<DeleteCardRequest>
{
    private readonly IValidator<DeleteCardRequest> _validator;
    private readonly ICardRepository _cardRepository;

    public DeleteCardHandler(IValidator<DeleteCardRequest> validator, ICardRepository cardRepository)
    {
        _validator = validator;
        _cardRepository = cardRepository;
    }

    public async Task Handle(DeleteCardRequest request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        Card card = (await _cardRepository.GetById(request.CardId))!;

        if (card.IsDeleted)
        {
            return;
        }

        card.IsDeleted = true;
        await _cardRepository.Update(card);
    }
}
