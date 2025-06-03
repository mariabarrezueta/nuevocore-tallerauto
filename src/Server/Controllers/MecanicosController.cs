using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Coretallerauto.Server.Data;
using Coretallerauto.Server.Models;

namespace Coretallerauto.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MecanicosController : ControllerBase
{
    private readonly AppDbContext _db;

    public MecanicosController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Mecanico>>> GetByCategoria([FromQuery] int categoriaId)
    {
        try
        {
            var categoria = (CategoriaReparacion)categoriaId;
            var mecanicos = await _db.Mecanicos.ToListAsync();

            var filtrados = mecanicos
                .Where(m => m.Habilidades.Any(h =>
                    h.Contains(categoria.ToString(), StringComparison.OrdinalIgnoreCase)))
                .OrderByDescending(m => m.CalcularPuntaje(categoria))
                .ToList();

            return Ok(filtrados);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error al filtrar mecánicos: {ex.Message}");
        }
    }

    [HttpGet("todos")]
    public async Task<ActionResult<IEnumerable<Mecanico>>> GetAll()
    {
        return await _db.Mecanicos.ToListAsync();
    }

    [HttpPost]
    public async Task<IActionResult> CrearMecanico([FromBody] Mecanico nuevo)
    {
        // Aseguramos que siempre se inicie con 0 órdenes activas
        nuevo.OrdenesActivas = 0;

        _db.Mecanicos.Add(nuevo);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAll), new { id = nuevo.IDMecanico }, nuevo);
    }

    [HttpGet("activos")]
    public async Task<ActionResult<IEnumerable<Mecanico>>> GetMecanicosActivos()
    {
        try
        {
            var mecanicos = await _db.Mecanicos
                .OrderByDescending(m => m.OrdenesActivas) // ⚡ Ordena de mayor a menor
                .ToListAsync();

            return Ok(mecanicos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error al obtener mecánicos activos: {ex.Message}");
        }
    }


}



