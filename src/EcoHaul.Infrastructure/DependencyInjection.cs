using EcoHaul.Application.Persistence;
using EcoHaul.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EcoHaul.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<EcoHaulDbContext>(options =>
            options.UseSqlServer(connectionString));
        services.AddScoped<IEcoHaulDbContext>(provider => provider.GetRequiredService<EcoHaulDbContext>());
        return services;
    }
}
