using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaMetrópolis.Models;

[Table("Editorial")]
public partial class Editorial
{
    [Key]
    public int IdEditorial { get; set; }

    [StringLength(100)]
    public string Nombre { get; set; } = null!;

    public int? IdPais { get; set; }

    [StringLength(50)]
    public string? Ciudad { get; set; }

    [ForeignKey("IdPais")]
    [InverseProperty("Editorials")]
    public virtual Pai? IdPaisNavigation { get; set; }

    [InverseProperty("IdEditorialNavigation")]
    public virtual ICollection<Recurso> Recursos { get; set; } = new List<Recurso>();
}
