using PosFarmacia.Domain.ValueObjects;

namespace PosFarmacia.Domain.Entities;

public sealed class ConvenioSeguro : Entidad
{
    private readonly List<CoberturaConvenio> _coberturas = [];

    private ConvenioSeguro() { }

    public ConvenioSeguro(string nombre)
    {
        Nombre = nombre;
        Activo = true;
    }

    public string Nombre { get; private set; } = string.Empty;

    public bool Activo { get; private set; }

    public IReadOnlyCollection<CoberturaConvenio> Coberturas => _coberturas;

    public void ConfigurarCobertura(Guid productoId, Porcentaje porcentajeCubierto)
    {
        var existente = _coberturas.FirstOrDefault(c => c.ProductoId == productoId);
        if (existente is not null)
        {
            existente.ActualizarPorcentaje(porcentajeCubierto);
            return;
        }

        _coberturas.Add(new CoberturaConvenio(Id, productoId, porcentajeCubierto));
    }

    public CoberturaConvenio? ObtenerCoberturaPara(Guid productoId) =>
        _coberturas.FirstOrDefault(c => c.ProductoId == productoId);

    public void Desactivar() => Activo = false;
}
