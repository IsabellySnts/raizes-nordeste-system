using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RaizesDoNordeste.Infrastructure;

public static class InfrastructureModule
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddData(configuration);
        return services;    
    }
    private static IServiceCollection AddData(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<RaizesDoNordesteDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("RaizesDoNordesteSystemDb")));
        return services;
    }
}
