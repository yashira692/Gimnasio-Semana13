using Gimnasio.Entidades.Models;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Gimnasio.Datos.Models;

public partial class GimnasioContext : DbContext
{
    public GimnasioContext(DbContextOptions<GimnasioContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Plan> Plans { get; set; }

    public virtual DbSet<Socio> Socios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Plan>(entity =>
        {
            entity.ToTable("Plan");

            entity.HasIndex(e => e.Nombre, "UQ_Plan_Nombre").IsUnique();

            entity.Property(e => e.Descripcion).HasMaxLength(200);
            entity.Property(e => e.Nombre).HasMaxLength(50);
            entity.Property(e => e.PrecioMensual).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<Socio>(entity =>
        {
            entity.ToTable("Socio");

            entity.HasIndex(e => e.Dni, "UQ_Socio_Dni").IsUnique();

            entity.Property(e => e.Apellidos).HasMaxLength(60);
            entity.Property(e => e.Dni)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Nombres).HasMaxLength(60);

            entity.HasOne(d => d.Plan).WithMany(p => p.Socios)
                .HasForeignKey(d => d.PlanId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Socio_Plan");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
