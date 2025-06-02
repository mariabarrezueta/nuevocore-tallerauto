using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Coretallerauto.Server.Data;
using Coretallerauto.Server.Models;
using Coretallerauto.Server.Services;
using Coretallerauto.Server.DTOs;

namespace Coretallerauto.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdenesController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly AsignacionService _svc;

    public OrdenesController(AppDbContext db, AsignacionService svc)
    {
        _db = db;
        _svc = svc;
    }

    [HttpPost]
    public async Task<IActionResult> CrearOrden([FromBody] CrearOrdenDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var orden = new OrdenTrabajo
        {
            VehiculoID = dto.VehiculoID,
            TipoReparacion = (CategoriaReparacion)dto.TipoReparacion,
            FechaIngreso = DateTime.UtcNow,
            Estado = EstadoOrden.Pendiente
        };

        _db.Ordenes.Add(orden);
        await _db.SaveChangesAsync();

        // Asigna el mecánico más adecuado
        var mecanicoAsignado = await _svc.AsignarMecanicoAsync(orden);

        if (mecanicoAsignado != null)
        {
            orden.MecanicoAsignadoID = mecanicoAsignado.IDMecanico;
            orden.Estado = EstadoOrden.EnProceso;

            // Solo sumamos 1 al mecánico asignado
            mecanicoAsignado.OrdenesActivas++;
            _db.Ordenes.Update(orden);
            _db.Mecanicos.Update(mecanicoAsignado);

            await _db.SaveChangesAsync();
        }

        return CreatedAtAction(nameof(GetById), new { id = orden.IDOrden }, orden);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var orden = await _db.Ordenes
            .Include(o => o.MecanicoAsignado)
            .Include(o => o.Vehiculo)
            .FirstOrDefaultAsync(o => o.IDOrden == id);

        return orden is null ? NotFound() : Ok(orden);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrdenTrabajo>>> GetOrdenes()
    {
        var ordenes = await _db.Ordenes
            .Include(o => o.Vehiculo)
            .Include(o => o.MecanicoAsignado)
            .ToListAsync();

        return Ok(ordenes);
    }

    [HttpPut("{id}/estado")]
    public async Task<IActionResult> ActualizarEstado(int id, [FromBody] EstadoUpdateDTO dto)
    {
        var orden = await _db.Ordenes.Include(o => o.MecanicoAsignado).FirstOrDefaultAsync(o => o.IDOrden == id);
        if (orden == null) return NotFound();

        if (orden.MecanicoAsignado != null)
        {
            // Sumar o restar la carga dependiendo del cambio
            if (orden.Estado != EstadoOrden.EnProceso && dto.Estado == EstadoOrden.EnProceso)
            {
                // Se inicia la orden: suma 1
                orden.MecanicoAsignado.OrdenesActivas++;
            }
            else if (orden.Estado == EstadoOrden.EnProceso &&
                     (dto.Estado == EstadoOrden.Finalizada || dto.Estado == EstadoOrden.Entregada))
            {
                // Se finaliza o entrega la orden: resta 1
                orden.MecanicoAsignado.OrdenesActivas--;
                if (orden.MecanicoAsignado.OrdenesActivas < 0)
                    orden.MecanicoAsignado.OrdenesActivas = 0;
            }

            _db.Mecanicos.Update(orden.MecanicoAsignado);
        }

        orden.Estado = dto.Estado;
        await _db.SaveChangesAsync();
        return Ok();
    }
}
