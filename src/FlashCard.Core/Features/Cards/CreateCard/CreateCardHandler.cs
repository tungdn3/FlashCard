using FlashCard.Core.Interfaces.Repositories;
using FlashCard.Core.Models;
using FluentValidation;
using MediatR;

namespace FlashCard.Core.Features.Cards.CreateCard;

public class CreateCardHandler : IRequestHandler<CreateCardRequest, int>
{
    private readonly IValidator<CreateCardRequest> _validator;
    private readonly ICardRepository _cardRepository;

    public CreateCardHandler(IValidator<CreateCardRequest> validator, ICardRepository cardRepository)
    {
        _validator = validator;
        _cardRepository = cardRepository;
    }

    public async Task<int> Handle(CreateCardRequest request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        var card = new Card
        {
            DeckId = request.DeckId,
            Word = request.Word,
            Meaning = request.Meaning,
        };

        int id = await _cardRepository.Create(card);
        return id;
    }
}
