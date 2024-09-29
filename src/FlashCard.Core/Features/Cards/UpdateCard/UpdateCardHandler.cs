using FlashCard.Core.Interfaces.Repositories;
using FlashCard.Core.Models;
using FluentValidation;
using MediatR;

namespace FlashCard.Core.Features.Cards.CreateCard;

public class UpdateCardHandler : IRequestHandler<UpdateCardRequest>
{
    private readonly IValidator<UpdateCardRequest> _validator;
    private readonly ICardRepository _cardRepository;

    public UpdateCardHandler(IValidator<UpdateCardRequest> validator, ICardRepository cardRepository)
    {
        _validator = validator;
        _cardRepository = cardRepository;
    }

    public async Task Handle(UpdateCardRequest request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        Card card = (await _cardRepository.GetById(request.CardId))!;

        card.Word = request.Word;
        card.Meaning = request.Meaning;

        await _cardRepository.Update(card);
    }
}
