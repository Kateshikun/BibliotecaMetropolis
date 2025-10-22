using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BibliotecaMetrópolis.Models
{
    [Table("Editorial")]
    public partial class Editorial
    {
        [Key]
        public int IdEdit { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = null!;

        [StringLength(500)]
        public string? descripcion { get; set; } // El '?' indica que la columna permite nulos

        // --- Propiedad de Navegación 1:N ---

        // Relación de uno a muchos: Una editorial puede tener muchos Recursos.
        // Esta colección nos permite, dada una editorial, acceder fácilmente a todos sus recursos.
        // La propiedad opuesta se define en Recurso como 'IdEditNavigation'.
        [InverseProperty("IdEditNavigation")]
        public virtual ICollection<Recurso> Recursos { get; set; } = new List<Recurso>();
    }
}