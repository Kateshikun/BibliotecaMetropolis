using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BlibliotecaWeb.Models;

[Table("PalabraClave")]
public partial class PalabraClave
{
    [Key]
    public int IdPalabraClave { get; set; }

    [StringLength(100)]
    public string Nombre { get; set; } = null!;

    [ForeignKey("IdPalabraClave")]
    [InverseProperty("IdPalabraClaves")]
    public virtual ICollection<Recurso> IdRecursos { get; set; } = new List<Recurso>();
}
