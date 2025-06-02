using System.ComponentModel.DataAnnotations;

namespace Coretallerauto.Server.Models;

public class Vehiculo
{
    [Key]
    public int IDVehiculo { get; set; }

    public int ClienteID { get; set; }
    public Cliente Cliente { get; set; } = null!;

    [Required]  public string Marca  { get; set; } = null!;
    [Required]  public string Modelo { get; set; } = null!;

    [Required]
    public int Anio { get; set; }


    [Required, MaxLength(10)]
    public string Placa { get; set; } = null!;

    public ICollection<OrdenTrabajo>? Ordenes { get; set; }
}
