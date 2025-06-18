using System;
using System.Collections.Generic;

namespace _3CXTimeControl.Models.DB;

public partial class Usuario
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public DateTime? CreadoEn { get; set; }

    public string? Rol { get; set; }

    public virtual ICollection<AuditoriaUsuario> AuditoriaUsuarios { get; set; } = new List<AuditoriaUsuario>();

    public virtual ICollection<NotasCliente> NotasClientes { get; set; } = new List<NotasCliente>();
}
