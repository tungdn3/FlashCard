using FlashCard.Core.Interfaces.Repositories;
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

        return services;
    }
}
