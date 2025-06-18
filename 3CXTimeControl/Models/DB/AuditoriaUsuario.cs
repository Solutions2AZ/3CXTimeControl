using System;
using System.Collections.Generic;

namespace _3CXTimeControl.Models.DB;

public partial class AuditoriaUsuario
{
    public int Id { get; set; }

    public int UsuarioId { get; set; }

    public string Accion { get; set; } = null!;

    public string? Descripcion { get; set; }

    public DateTime? Fecha { get; set; }

    public virtual Usuario Usuario { get; set; } = null!;
}
