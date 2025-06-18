using System;
using System.Collections.Generic;

namespace _3CXTimeControl.Models.DB;

public partial class NotasCliente
{
    public int Id { get; set; }

    public int? ClienteId { get; set; }

    public int? UsuarioId { get; set; }

    public string? Nota { get; set; }

    public DateTime? Fecha { get; set; }

    public virtual Cliente? Cliente { get; set; }

    public virtual Usuario? Usuario { get; set; }
}
