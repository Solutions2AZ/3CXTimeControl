using System;
using System.Collections.Generic;

namespace _3CXTimeControl.Models.DB;

public partial class Llamada
{
    public int Id { get; set; }

    public int? ClienteId { get; set; }

    public string? NumeroTelefono { get; set; }

    public DateTime FechaInicio { get; set; }

    public DateTime? FechaFin { get; set; }

    public int? DuracionMinutos { get; set; }

    public int? MinutosConsumidos { get; set; }

    public virtual Cliente? Cliente { get; set; }
}
