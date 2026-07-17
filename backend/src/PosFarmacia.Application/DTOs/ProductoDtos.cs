namespace PosFarmacia.Application.DTOs;

public sealed record RegistrarProductoRequest(
    string CodigoInterno,
    string? CodigoBarras,
    string NombreComercial,
    string Descripcion,
    string TipoProducto,
    Guid CategoriaId,
    Guid LaboratorioId,
    Guid PresentacionId,
    decimal PrecioVenta,
    bool EsControlado,
    bool RequiereReceta,
    string? TipoRecetaRequerida);

public sealed record ActualizarProductoRequest(string NombreComercial, string Descripcion, decimal PrecioVenta);

public sealed record ProductoResponse(
    Guid Id,
    string CodigoInterno,
    string? CodigoBarras,
    string NombreComercial,
    string Descripcion,
    string TipoProducto,
    Guid CategoriaId,
    Guid LaboratorioId,
    Guid PresentacionId,
    decimal PrecioVenta,
    bool EsControlado,
    bool RequiereReceta,
    string? TipoRecetaRequerida,
    string Estado);
