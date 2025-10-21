using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BlibliotecaWeb.Models;

public partial class BibliotecaContext : DbContext
{
    public BibliotecaContext()
    {
    }

    public BibliotecaContext(DbContextOptions<BibliotecaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Autor> Autors { get; set; }

    public virtual DbSet<Editorial> Editorials { get; set; }

    public virtual DbSet<Pai> Pais { get; set; }

    public virtual DbSet<PalabraClave> PalabraClaves { get; set; }

    public virtual DbSet<Recurso> Recursos { get; set; }

    public virtual DbSet<TipoRecurso> TipoRecursos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=ANDERSON\\SQLEXPRESS01;Database=BibliotecaMetropolis;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Autor>(entity =>
        {
            entity.HasKey(e => e.IdAutor).HasName("PK__Autor__DD33B031AB4AD39C");

            entity.HasOne(d => d.IdPaisNavigation).WithMany(p => p.Autors).HasConstraintName("FK_Autor_Pais");
        });

        modelBuilder.Entity<Editorial>(entity =>
        {
            entity.HasKey(e => e.IdEditorial).HasName("PK__Editoria__EF83867182BD3FCC");

            entity.HasOne(d => d.IdPaisNavigation).WithMany(p => p.Editorials).HasConstraintName("FK_Editorial_Pais");
        });

        modelBuilder.Entity<Pai>(entity =>
        {
            entity.HasKey(e => e.IdPais).HasName("PK__Pais__FC850A7B8AF324B9");
        });

        modelBuilder.Entity<PalabraClave>(entity =>
        {
            entity.HasKey(e => e.IdPalabraClave).HasName("PK__PalabraC__153BDD35A08260BE");
        });

        modelBuilder.Entity<Recurso>(entity =>
        {
            entity.HasKey(e => e.IdRecurso).HasName("PK__Recurso__B91948E945341B8D");

            entity.HasOne(d => d.IdEditorialNavigation).WithMany(p => p.Recursos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Recurso_Editorial");

            entity.HasOne(d => d.IdPaisNavigation).WithMany(p => p.Recursos).HasConstraintName("FK_Recurso_Pais");

            entity.HasOne(d => d.IdTipoRecursoNavigation).WithMany(p => p.Recursos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Recurso_TipoRecurso");

            entity.HasMany(d => d.IdAutors).WithMany(p => p.IdRecursos)
                .UsingEntity<Dictionary<string, object>>(
                    "RecursoAutor",
                    r => r.HasOne<Autor>().WithMany()
                        .HasForeignKey("IdAutor")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_RecursoAutor_Autor"),
                    l => l.HasOne<Recurso>().WithMany()
                        .HasForeignKey("IdRecurso")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_RecursoAutor_Recurso"),
                    j =>
                    {
                        j.HasKey("IdRecurso", "IdAutor").HasName("PK__RecursoA__B4CA73EAC8B8BA9F");
                        j.ToTable("RecursoAutor");
                    });

            entity.HasMany(d => d.IdPalabraClaves).WithMany(p => p.IdRecursos)
                .UsingEntity<Dictionary<string, object>>(
                    "RecursoPalabraClave",
                    r => r.HasOne<PalabraClave>().WithMany()
                        .HasForeignKey("IdPalabraClave")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_RPK_PalabraClave"),
                    l => l.HasOne<Recurso>().WithMany()
                        .HasForeignKey("IdRecurso")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_RPK_Recurso"),
                    j =>
                    {
                        j.HasKey("IdRecurso", "IdPalabraClave").HasName("PK__RecursoP__F84AF53ADDE8C851");
                        j.ToTable("RecursoPalabraClave");
                    });
        });

        modelBuilder.Entity<TipoRecurso>(entity =>
        {
            entity.HasKey(e => e.IdTipoRecurso).HasName("PK__TipoRecu__B310D5CF7230815E");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
