using Microsoft.EntityFrameworkCore;
using PosFarmacia.Domain.Entities;
using PosFarmacia.Domain.Repositories;
using PosFarmacia.Domain.ValueObjects;
using PosFarmacia.Infrastructure.Persistence;

namespace PosFarmacia.Infrastructure.Repositories;

public sealed class RecetaRepository(PosFarmaciaDbContext contexto) : RepositorioBase<Receta>(contexto), IRecetaRepository
{
    public async Task<Receta?> ObtenerPorNumeroAsync(string numero, CancellationToken ct = default) =>
        await Contexto.Recetas.FirstOrDefaultAsync(r => r.Numero == new NumeroReceta(numero), ct);
}

public sealed class ValidacionRecetaRepository(PosFarmaciaDbContext contexto) : RepositorioBase<ValidacionReceta>(contexto), IValidacionRecetaRepository;
