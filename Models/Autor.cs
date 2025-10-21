using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BlibliotecaWeb.Models;

[Table("Autor")]
public partial class Autor
{
    [Key]
    public int IdAutor { get; set; }

    [StringLength(100)]
    public string Nombres { get; set; } = null!;

    [StringLength(100)]
    public string Apellidos { get; set; } = null!;

    public int? IdPais { get; set; }

    [StringLength(100)]
    public string? Email { get; set; }

    [ForeignKey("IdPais")]
    [InverseProperty("Autors")]
    public virtual Pai? IdPaisNavigation { get; set; }

    [ForeignKey("IdAutor")]
    [InverseProperty("IdAutors")]
    public virtual ICollection<Recurso> IdRecursos { get; set; } = new List<Recurso>();
}
