using Microsoft.EntityFrameworkCore;
using Coretallerauto.Server.Models;
using System.Text.Json;

namespace Coretallerauto.Server.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt) { }

    public DbSet<Cliente>       Clientes  => Set<Cliente>();
    public DbSet<Vehiculo>      Vehiculos { get; set; }
    public DbSet<Mecanico>      Mecanicos { get; set; }
    public DbSet<OrdenTrabajo>  Ordenes   { get; set; }
    public DbSet<HistorialServicio> HistorialServicios => Set<HistorialServicio>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}



