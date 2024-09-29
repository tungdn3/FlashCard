using FlashCard.Core.Features.Decks;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace FlashCard.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddCoreLayer(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<GetDecksRequest>());
        services.AddValidatorsFromAssemblyContaining<CreateDeckValidator>();
        return services;
    }
}
