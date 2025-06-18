using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace _3CXTimeControl.Models.DB;

public partial class ControlLlamadasContext : DbContext
{
    public ControlLlamadasContext()
    {
    }

    public ControlLlamadasContext(DbContextOptions<ControlLlamadasContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AuditoriaUsuario> AuditoriaUsuarios { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Llamada> Llamadas { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<NotasCliente> NotasClientes { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseMySQL("server=217.160.248.210;port=3306;database=control_llamadas;user=apiuser;password=CRUZ-Telecom@2025");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuditoriaUsuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("auditoria_usuarios");

            entity.HasIndex(e => e.UsuarioId, "usuario_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Accion)
                .HasMaxLength(255)
                .HasColumnName("accion");
            entity.Property(e => e.Descripcion)
                .HasColumnType("text")
                .HasColumnName("descripcion");
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("fecha");
            entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");

            entity.HasOne(d => d.Usuario).WithMany(p => p.AuditoriaUsuarios)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("auditoria_usuarios_ibfk_1");
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("clientes");

            entity.HasIndex(e => e.NumeroTelefono, "numero_telefono").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FechaUltimaActualizacion)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("fecha_ultima_actualizacion");
            entity.Property(e => e.MinutosDisponibles).HasColumnName("minutos_disponibles");
            entity.Property(e => e.MinutosTotales).HasColumnName("minutos_totales");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
            entity.Property(e => e.NumeroTelefono)
                .HasMaxLength(20)
                .HasColumnName("numero_telefono");
        });

        modelBuilder.Entity<Llamada>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("llamadas");

            entity.HasIndex(e => e.ClienteId, "cliente_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ClienteId).HasColumnName("cliente_id");
            entity.Property(e => e.DuracionMinutos)
                .HasDefaultValueSql("'0'")
                .HasColumnName("duracion_minutos");
            entity.Property(e => e.FechaFin)
                .HasColumnType("datetime")
                .HasColumnName("fecha_fin");
            entity.Property(e => e.FechaInicio)
                .HasColumnType("datetime")
                .HasColumnName("fecha_inicio");
            entity.Property(e => e.MinutosConsumidos)
                .HasDefaultValueSql("'0'")
                .HasColumnName("minutos_consumidos");
            entity.Property(e => e.NumeroTelefono)
                .HasMaxLength(20)
                .HasColumnName("numero_telefono");

            entity.HasOne(d => d.Cliente).WithMany(p => p.Llamada)
                .HasForeignKey(d => d.ClienteId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("llamadas_ibfk_1");
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("logs");

            entity.HasIndex(e => e.ClienteId, "cliente_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Accion)
                .HasMaxLength(255)
                .HasColumnName("accion");
            entity.Property(e => e.ClienteId).HasColumnName("cliente_id");
            entity.Property(e => e.Descripcion)
                .HasColumnType("text")
                .HasColumnName("descripcion");
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("fecha");

            entity.HasOne(d => d.Cliente).WithMany(p => p.Logs)
                .HasForeignKey(d => d.ClienteId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("logs_ibfk_1");
        });

        modelBuilder.Entity<NotasCliente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("notas_cliente");

            entity.HasIndex(e => e.ClienteId, "cliente_id");

            entity.HasIndex(e => e.UsuarioId, "usuario_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ClienteId).HasColumnName("cliente_id");
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("fecha");
            entity.Property(e => e.Nota)
                .HasColumnType("text")
                .HasColumnName("nota");
            entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");

            entity.HasOne(d => d.Cliente).WithMany(p => p.NotasClientes)
                .HasForeignKey(d => d.ClienteId)
                .HasConstraintName("notas_cliente_ibfk_1");

            entity.HasOne(d => d.Usuario).WithMany(p => p.NotasClientes)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("notas_cliente_ibfk_2");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("usuarios");

            entity.HasIndex(e => e.Email, "email").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreadoEn)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("creado_en");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .HasColumnName("password_hash");
            entity.Property(e => e.Rol)
                .HasMaxLength(20)
                .HasDefaultValueSql("'operadora'")
                .HasColumnName("rol");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
