using Microsoft.Extensions.DependencyInjection;
using PosFarmacia.Domain.Services;

namespace PosFarmacia.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationUseCases(this IServiceCollection services)
    {
        var namespacioUseCases = $"{typeof(DependencyInjection).Namespace}.UseCases";
        var tiposUseCases = typeof(DependencyInjection).Assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.Namespace == namespacioUseCases);

        foreach (var tipo in tiposUseCases)
            services.AddScoped(tipo);

        services.AddSingleton<EvaluadorPromociones>();
        services.AddSingleton<CalculadorTotalVenta>();
        services.AddSingleton<ValidadorReceta>();
        services.AddSingleton<CalculadorCopago>();
        services.AddSingleton<ValidadorLineaCredito>();
        services.AddSingleton<AsignadorLotesFEFO>();
        services.AddSingleton<ServicioAnulacionVenta>();
        services.AddSingleton<CalculadorIncentivos>();
        services.AddSingleton<ValidadorDevolucion>();
        services.AddSingleton<AsignadorReversionesDevolucion>();

        return services;
    }
}
