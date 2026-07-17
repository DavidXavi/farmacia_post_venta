namespace PosFarmacia.Domain.Entities;

public abstract class Entidad
{
    public Guid Id { get; protected set; } = Guid.CreateVersion7();
}
