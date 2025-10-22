using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BibliotecaMetrópolis.Models
{
    [Table("TipoRecurso")]
    public partial class TipoRecurso
    {
        // La clave primaria tiene un nombre diferente a la convención (idTipoR vs IdTipoRecurso),
        // pero EF Core lo maneja bien si el mapeo en Recurso.cs es correcto.
        [Key]
        public int idTipoR { get; set; }

        [Required]
        [StringLength(50)]
        public string nombre { get; set; } = null!;

        [StringLength(255)]
        public string? descripcion { get; set; }

        // --- Propiedad de Navegación 1:N ---

        // Un TipoRecurso puede clasificar a muchos Recursos.
        [InverseProperty("IdTipoRNavigation")]
        public virtual ICollection<Recurso> Recursos { get; set; } = new List<Recurso>();
    }
}