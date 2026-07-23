namespace PosFarmacia.Application.DTOs;

public sealed record RegistrarPromocionRequest(
    string Nombre,
    string Descripcion,
    string TipoBeneficio,
    decimal ValorBeneficio,
    bool RequiereCliente,
    int CantidadMinima,
    DateOnly? FechaInicio,
    DateOnly? FechaFin,
    IReadOnlyCollection<Guid> ProductosParticipantes);

public sealed record EditarPromocionRequest(
    string Nombre,
    string Descripcion,
    string TipoBeneficio,
    decimal ValorBeneficio,
    bool RequiereCliente,
    int CantidadMinima,
    DateOnly? FechaInicio,
    DateOnly? FechaFin,
    IReadOnlyCollection<Guid> ProductosParticipantes);

public sealed record PromocionResponse(
    Guid Id,
    string Nombre,
    string Descripcion,
    string TipoBeneficio,
    decimal ValorBeneficio,
    bool RequiereCliente,
    int CantidadMinima,
    DateOnly? FechaInicio,
    DateOnly? FechaFin,
    bool Activa,
    IReadOnlyCollection<Guid> ProductosParticipantes);
