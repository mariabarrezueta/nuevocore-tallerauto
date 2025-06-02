using Microsoft.EntityFrameworkCore;
using Coretallerauto.Server.Data;
using Coretallerauto.Server.Models;

namespace Coretallerauto.Server.Services;

public class AsignacionService
{
    private readonly AppDbContext _db;

    public AsignacionService(AppDbContext db) => _db = db;

    private static readonly Dictionary<CategoriaReparacion, string[]> _keywordsPorCategoria = new()
    {
        [CategoriaReparacion.MecanicaGeneral] = new[] { "motor", "frenos", "suspensión", "dirección", "aceite" },
        [CategoriaReparacion.ElectricidadElectronica] = new[] { "batería", "alternador", "luces", "sensores", "escáner", "airbag" },
        [CategoriaReparacion.EsteticaCarroceria] = new[] { "pintura", "latonería", "vidrios", "accesorios" }
    };

    public async Task<Mecanico?> AsignarMecanicoAsync(OrdenTrabajo orden)
{
    var vehiculo = await _db.Vehiculos.FindAsync(orden.VehiculoID);
    if (vehiculo == null) throw new Exception("Vehículo no encontrado");

    var categoria = orden.TipoReparacion;
    var marcaVehiculo = vehiculo.Marca;
    var palabrasClave = _keywordsPorCategoria[categoria];

    var todos = await _db.Mecanicos.ToListAsync();

    var candidatos = todos
        .Where(m =>
            m.Habilidades.Any(h =>
                palabrasClave.Any(p => h.Contains(p, StringComparison.OrdinalIgnoreCase))))
        .ToList();

    // 💡 Calcular puntaje en variable temporal (NO modificar OrdenesActivas real)
    var candidatosConPuntaje = candidatos
        .Select(m =>
        {
            int puntaje = m.AniosExperiencia;

            if (m.MarcasExpertas.Any(marca => marca.Equals(marcaVehiculo, StringComparison.OrdinalIgnoreCase)))
                puntaje += 3;

            puntaje -= m.OrdenesActivas * 2;

            return (Mecanico: m, Puntaje: puntaje);
        })
        .ToList();

    var mejor = candidatosConPuntaje
        .OrderByDescending(x => x.Puntaje)
        .Select(x => x.Mecanico)
        .FirstOrDefault();

    if (mejor != null)
    {
        orden.MecanicoAsignadoID = mejor.IDMecanico;
        orden.Estado = EstadoOrden.EnProceso;
        await _db.SaveChangesAsync();
    }

    return mejor;
}

}
