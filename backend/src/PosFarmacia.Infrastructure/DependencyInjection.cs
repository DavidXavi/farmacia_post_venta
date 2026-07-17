using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PosFarmacia.Application.Ports;
using PosFarmacia.Domain.Repositories;
using PosFarmacia.Infrastructure.Auditing;
using PosFarmacia.Infrastructure.Authentication;
using PosFarmacia.Infrastructure.Insurance;
using PosFarmacia.Infrastructure.Persistence;
using PosFarmacia.Infrastructure.Repositories;

namespace PosFarmacia.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<PosFarmaciaDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Default")));

        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SeccionConfiguracion));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<ISeguroClient, SeguroClient>();
        services.AddScoped<IAuditService, AuditService>();

        services.AddScoped<ILocalRepository, LocalRepository>();
        services.AddScoped<IRolRepository, RolRepository>();
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        services.AddScoped<ICajaRepository, CajaRepository>();
        services.AddScoped<ISesionCajaRepository, SesionCajaRepository>();
        services.AddScoped<IClienteRepository, ClienteRepository>();
        services.AddScoped<ICategoriaRepository, CategoriaRepository>();
        services.AddScoped<ILaboratorioRepository, LaboratorioRepository>();
        services.AddScoped<IPresentacionRepository, PresentacionRepository>();
        services.AddScoped<IFormaPagoRepository, FormaPagoRepository>();
        services.AddScoped<IProductoRepository, ProductoRepository>();
        services.AddScoped<ILoteRepository, LoteRepository>();
        services.AddScoped<IMovimientoStockRepository, MovimientoStockRepository>();
        services.AddScoped<IVentaRepository, VentaRepository>();
        services.AddScoped<IPromocionRepository, PromocionRepository>();
        services.AddScoped<IRecetaRepository, RecetaRepository>();
        services.AddScoped<IValidacionRecetaRepository, ValidacionRecetaRepository>();
        services.AddScoped<IConvenioSeguroRepository, ConvenioSeguroRepository>();
        services.AddScoped<IAfiliacionClienteRepository, AfiliacionClienteRepository>();
        services.AddScoped<ILineaCreditoRepository, LineaCreditoRepository>();
        services.AddScoped<IMovimientoCreditoRepository, MovimientoCreditoRepository>();
        services.AddScoped<INotaCreditoRepository, NotaCreditoRepository>();
        services.AddScoped<IReglaIncentivoRepository, ReglaIncentivoRepository>();
        services.AddScoped<IIncentivoVentaRepository, IncentivoVentaRepository>();
        services.AddScoped<IAuditoriaRepository, AuditoriaRepository>();

        return services;
    }
}
