using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Coretallerauto.Server.Data;        // para acceder al DbContext
using Microsoft.EntityFrameworkCore;

namespace Coretallerauto.Server.Validation;

/// <summary>Valida formato y unicidad de la cédula ecuatoriana.</summary>
public class CedulaEcuadorAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object? value, ValidationContext context)
    {
        string cedula = value as string ?? string.Empty;

        // 1) 10 dígitos
        if (!Regex.IsMatch(cedula, @"^\d{10}$"))
            return new ValidationResult("La cédula debe tener 10 dígitos.");

        // 2) Dígito verificador (algoritmo simplificado)
        if (!ValidaDigitoVerificador(cedula))
            return new ValidationResult("Cédula inválida (dígito verificador).");

        // 3) Unicidad en BD
        var db = (AppDbContext)context.GetService(typeof(AppDbContext))!;
        bool existe = db.Clientes.AsNoTracking().Any(c => c.Cedula == cedula);
        return existe ? new ValidationResult("La cédula ya existe.") : ValidationResult.Success!;
    }

    private bool ValidaDigitoVerificador(string ced)
    {
        int suma = 0;
        for (int i = 0; i < 9; i++)
        {
            int n = int.Parse(ced[i].ToString());
            suma += (i % 2 == 0) ? (n * 2 > 9 ? n * 2 - 9 : n * 2) : n;
        }
        int verificadorCalc = (10 - (suma % 10)) % 10;
        int verificadorReal = int.Parse(ced[^1].ToString());
        return verificadorCalc == verificadorReal;
    }
}
