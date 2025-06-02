namespace Coretallerauto.Server.Models;
public enum CategoriaReparacion
{
    MecanicaGeneral = 1,
    ElectricidadElectronica,
    EsteticaCarroceria
}

public enum EstadoOrden
{
    Pendiente = 0,
    EnProceso = 1,
    Finalizada = 2,
    Entregada = 3
}