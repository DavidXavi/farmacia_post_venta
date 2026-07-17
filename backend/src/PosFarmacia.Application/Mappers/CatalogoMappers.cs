using PosFarmacia.Application.DTOs;
using PosFarmacia.Domain.Entities;

namespace PosFarmacia.Application.Mappers;

public static class CatalogoMappers
{
    public static CategoriaResponse ToResponse(this Categoria c) => new(c.Id, c.Nombre);

    public static LaboratorioResponse ToResponse(this Laboratorio l) => new(l.Id, l.Nombre);

    public static PresentacionResponse ToResponse(this Presentacion p) => new(p.Id, p.Nombre, p.UnidadMedida);

    public static FormaPagoResponse ToResponse(this FormaPago f) => new(f.Id, f.Nombre, f.Tipo.ToString(), f.Activo);

    public static LocalResponse ToResponse(this Local l) => new(l.Id, l.Nombre, l.Direccion, l.Activo);

    public static CajaResponse ToResponse(this Caja c) => new(c.Id, c.Nombre, c.LocalId, c.Activa);

    public static ReglaIncentivoResponse ToResponse(this ReglaIncentivo r) => new(
        r.Id, r.Nombre, r.ProductoId, r.CategoriaId, r.MontoPorUnidad.Monto, r.Vigencia.Inicio!.Value, r.Vigencia.Fin!.Value, r.Activa);
}
