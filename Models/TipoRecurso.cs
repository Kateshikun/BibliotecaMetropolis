using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BlibliotecaWeb.Models;

[Table("TipoRecurso")]
public partial class TipoRecurso
{
    [Key]
    public int IdTipoRecurso { get; set; }

    [StringLength(50)]
    public string Nombre { get; set; } = null!;

    [InverseProperty("IdTipoRecursoNavigation")]
    public virtual ICollection<Recurso> Recursos { get; set; } = new List<Recurso>();
}
