using Microsoft.EntityFrameworkCore;
using PosFarmacia.Application.Ports;
using PosFarmacia.Domain.Entities;
using PosFarmacia.Domain.Enums;

namespace PosFarmacia.Infrastructure.Persistence;

/// ponytail: seed minimo por codigo (mas simple que HasData con GUIDs fijos) para poder demostrar el flujo end-to-end.
public static class SeedData
{
    public static async Task EnsureSeedDataAsync(PosFarmaciaDbContext contexto, IPasswordHasher passwordHasher, CancellationToken ct = default)
    {
        if (await contexto.Roles.AnyAsync(ct))
        {
            return;
        }

        var roles = Enum.GetValues<RolNombre>()
            .Select(nombre => new Rol(nombre, nombre.ToString()))
            .ToList();
        await contexto.Roles.AddRangeAsync(roles, ct);

        var local = new Local("Sede Principal", "Av. Principal 123");
        await contexto.Locales.AddAsync(local, ct);

        var caja = new Caja("Caja 1", local.Id);
        await contexto.Cajas.AddAsync(caja, ct);

        var admin = new Usuario("admin", passwordHasher.Hash("Admin123!"), local.Id, PermisoEspecial.AnularVentas | PermisoEspecial.EmitirNotaCredito | PermisoEspecial.VerAuditoria);
        admin.AsignarRol(roles.First(r => r.Nombre == RolNombre.Administrador).Id);
        await contexto.Usuarios.AddAsync(admin, ct);

        var cajero = new Usuario("cajero", passwordHasher.Hash("Cajero123!"), local.Id);
        cajero.AsignarRol(roles.First(r => r.Nombre == RolNombre.Cajero).Id);
        await contexto.Usuarios.AddAsync(cajero, ct);

        var quimico = new Usuario("quimico", passwordHasher.Hash("Quimico123!"), local.Id, PermisoEspecial.ValidarRecetas);
        quimico.AsignarRol(roles.First(r => r.Nombre == RolNombre.QuimicoFarmaceutico).Id);
        await contexto.Usuarios.AddAsync(quimico, ct);

        var formasPago = new[]
        {
            new FormaPago("Efectivo", TipoFormaPago.Efectivo),
            new FormaPago("Tarjeta de debito", TipoFormaPago.TarjetaDebito),
            new FormaPago("Tarjeta de credito", TipoFormaPago.TarjetaCredito),
            new FormaPago("Transferencia", TipoFormaPago.Transferencia),
            new FormaPago("Billetera digital", TipoFormaPago.BilleteraDigital),
            new FormaPago("Copago de seguro", TipoFormaPago.CopagoSeguro),
            new FormaPago("Credito de farmacia", TipoFormaPago.CreditoFarmacia)
        };
        await contexto.FormasPago.AddRangeAsync(formasPago, ct);

        var categoria = new Categoria("Analgesicos");
        var laboratorio = new Laboratorio("Laboratorio Generico");
        var presentacion = new Presentacion("Tableta x 10", "Caja");
        await contexto.Categorias.AddAsync(categoria, ct);
        await contexto.Laboratorios.AddAsync(laboratorio, ct);
        await contexto.Presentaciones.AddAsync(presentacion, ct);

        var producto = new Producto(
            new Domain.ValueObjects.CodigoProducto("P0001"),
            "Paracetamol 500mg",
            "Analgesico y antipiretico de venta libre",
            TipoProducto.Otc,
            categoria.Id,
            laboratorio.Id,
            presentacion.Id,
            new Domain.ValueObjects.Dinero(12.50m),
            esControlado: false,
            requiereReceta: false,
            tipoRecetaRequerida: null,
            codigoBarras: new Domain.ValueObjects.CodigoBarras("7750001000019"));
        await contexto.Productos.AddAsync(producto, ct);

        var lote = new Lote(
            new Domain.ValueObjects.CodigoLote("L0001"),
            producto.Id,
            new Domain.ValueObjects.FechaVencimiento(DateOnly.FromDateTime(DateTime.UtcNow).AddYears(1)),
            new Domain.ValueObjects.Cantidad(100),
            local.Id);
        await contexto.Lotes.AddAsync(lote, ct);

        await contexto.SaveChangesAsync(ct);
    }
}
