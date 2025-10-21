using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BlibliotecaWeb.Models;

public partial class Pai
{
    [Key]
    public int IdPais { get; set; }

    [StringLength(80)]
    public string Nombre { get; set; } = null!;

    [StringLength(3)]
    public string? CodigoISO { get; set; }

    [InverseProperty("IdPaisNavigation")]
    public virtual ICollection<Autor> Autors { get; set; } = new List<Autor>();

    [InverseProperty("IdPaisNavigation")]
    public virtual ICollection<Editorial> Editorials { get; set; } = new List<Editorial>();

    [InverseProperty("IdPaisNavigation")]
    public virtual ICollection<Recurso> Recursos { get; set; } = new List<Recurso>();
}
