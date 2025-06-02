using Coretallerauto.Server.Validation;
using System.ComponentModel.DataAnnotations;

namespace Coretallerauto.Server.Models;

public class Cliente
{
    [Key]
    public int IDCliente { get; set; }

    [Required, CedulaEcuador]      // atributo que ya definiste
    public string Cedula { get; set; } = null!;

    [Required, MaxLength(80)]
    public string Nombre { get; set; } = null!;

    [MaxLength(80)]
    public string? Apellido { get; set; }

    [Phone]
    public string? Telefono { get; set; }

    [EmailAddress]
    public string? Email { get; set; }

    public ICollection<Vehiculo>? Vehiculos { get; set; }
}
