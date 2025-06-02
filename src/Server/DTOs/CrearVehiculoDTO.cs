namespace Coretallerauto.Server.DTOs;

public class CrearVehiculoDTO

{
    public int ClienteID { get; set; }
    public string Marca { get; set; } = null!;
    public string Modelo { get; set; } = null!;
    public int Anio { get; set; }
    public string Placa { get; set; } = null!;

}
