using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Coretallerauto.Server.Data;
using Coretallerauto.Server.Models;
using Coretallerauto.Server.DTOs;

namespace Coretallerauto.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VehiculosController : ControllerBase
{
    private readonly AppDbContext _db;

    public VehiculosController(AppDbContext db)
    {
        _db = db;
    }

    [HttpPost]
    public async Task<IActionResult> CrearVehiculo([FromBody] CrearVehiculoDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Validación de placa (por ejemplo)
        if (!System.Text.RegularExpressions.Regex.IsMatch(dto.Placa, @"^[A-Z]{3}[0-9]{4}$"))
        {
            return BadRequest("La placa debe tener el formato ABC1234.");
        }

        var vehiculo = new Vehiculo
        {
            ClienteID = dto.ClienteID,
            Marca     = dto.Marca,
            Modelo    = dto.Modelo,
            Anio      = dto.Anio,
            Placa     = dto.Placa
        };

        _db.Vehiculos.Add(vehiculo);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPorId), new { id = vehiculo.IDVehiculo }, vehiculo);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Vehiculo>> GetPorId(int id)
    {
        var vehiculo = await _db.Vehiculos
            .Include(v => v.Cliente)
            .Include(v => v.Ordenes)
            .FirstOrDefaultAsync(v => v.IDVehiculo == id);

        return vehiculo == null ? NotFound() : Ok(vehiculo);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Vehiculo>>> GetVehiculos()
    {
        var vehiculos = await _db.Vehiculos
            .Include(v => v.Cliente)
            .Include(v => v.Ordenes)
            .ToListAsync();

        return Ok(vehiculos);
    }
}

