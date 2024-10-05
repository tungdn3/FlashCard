using FlashCard.Core.Interfaces.AIClients;
using FlashCard.Core.Interfaces.Repositories;
using FlashCard.Infrastructure.AIClients;
using FlashCard.Infrastructure.EF;
using FlashCard.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FlashCard.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<FlashCardDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("DbContextSQLite")));

        services.AddScoped<IDeckRepository, DeckRepository>();
        services.AddScoped<ICardRepository, CardRepository>();
        services.AddScoped<IIdentityRepository, IdentityRepository>();

        services.Configure<AzureChatCompleteOptions>(configuration.GetSection(AzureChatCompleteOptions.AzureChatComplete));
        services.AddSingleton<IGenerativeAIClient, AzureChatCompleteClient>();

        services.Configure<AzureDallEOptions>(configuration.GetSection(AzureDallEOptions.AzureDallE));
        services.AddSingleton<IAIImageClient, AzureDallEClient>();

        return services;
    }
}
