using Coretallerauto.Server.Validation;
using System.ComponentModel.DataAnnotations;
namespace Coretallerauto.Server.Models;

public class OrdenTrabajo
{
    [Key]
    public int IDOrden { get; set; }

    public int VehiculoID { get; set; }
    public Vehiculo Vehiculo { get; set; } = null!;

    public CategoriaReparacion TipoReparacion { get; set; }
    public EstadoOrden        Estado         { get; set; } = EstadoOrden.Pendiente;
    public DateTime           FechaIngreso   { get; set; } = DateTime.UtcNow;

    public int? MecanicoAsignadoID { get; set; }
    public Mecanico? MecanicoAsignado { get; set; }

    public ICollection<HistorialServicio>? Historial { get; set; }
}
