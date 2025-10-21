using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BlibliotecaWeb.Models;

[Table("Recurso")]
public partial class Recurso
{
    [Key]
    public int IdRecurso { get; set; }

    [StringLength(200)]
    public string Titulo { get; set; } = null!;

    public int IdTipoRecurso { get; set; }

    public int IdEditorial { get; set; }

    public int? AnioPublicacion { get; set; }

    public int? IdPais { get; set; }

    [StringLength(50)]
    public string? Ciudad { get; set; }

    public int? Cantidad { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? Precio { get; set; }

    [ForeignKey("IdEditorial")]
    [InverseProperty("Recursos")]
    public virtual Editorial IdEditorialNavigation { get; set; } = null!;

    [ForeignKey("IdPais")]
    [InverseProperty("Recursos")]
    public virtual Pai? IdPaisNavigation { get; set; }

    [ForeignKey("IdTipoRecurso")]
    [InverseProperty("Recursos")]
    public virtual TipoRecurso IdTipoRecursoNavigation { get; set; } = null!;

    [ForeignKey("IdRecurso")]
    [InverseProperty("IdRecursos")]
    public virtual ICollection<Autor> IdAutors { get; set; } = new List<Autor>();

    [ForeignKey("IdRecurso")]
    [InverseProperty("IdRecursos")]
    public virtual ICollection<PalabraClave> IdPalabraClaves { get; set; } = new List<PalabraClave>();
}
