using System;
using System.Collections.Generic;

namespace _3CXTimeControl.Models.DB;

public partial class Log
{
    public int Id { get; set; }

    public int? ClienteId { get; set; }

    public string? Accion { get; set; }

    public string? Descripcion { get; set; }

    public DateTime? Fecha { get; set; }

    public virtual Cliente? Cliente { get; set; }
}
