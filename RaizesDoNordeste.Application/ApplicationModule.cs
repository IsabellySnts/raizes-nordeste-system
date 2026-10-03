using Microsoft.Extensions.DependencyInjection;
using RaizesDoNordeste.Application.Interfaces;
using RaizesDoNordeste.Application.Services;

namespace RaizesDoNordeste.Application;

public static class ApplicationModule
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IPedidoService, PedidoService>();
        return services;
    }
}
