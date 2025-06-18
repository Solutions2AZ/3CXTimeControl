using System;
using System.Collections.Generic;

namespace _3CXTimeControl.Models.DB;

public partial class Cliente
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string NumeroTelefono { get; set; } = null!;

    public int MinutosTotales { get; set; }

    public int MinutosDisponibles { get; set; }

    public DateTime? FechaUltimaActualizacion { get; set; }

    public virtual ICollection<Llamada> Llamada { get; set; } = new List<Llamada>();

    public virtual ICollection<Log> Logs { get; set; } = new List<Log>();

    public virtual ICollection<NotasCliente> NotasClientes { get; set; } = new List<NotasCliente>();
}
