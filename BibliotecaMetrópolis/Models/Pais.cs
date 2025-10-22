using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BibliotecaMetrópolis.Models
{
    [Table("Pais")]
    public partial class Pais
    {
        [Key]
        public int IdPais { get; set; }

        [Required]
        [StringLength(80)]
        public string nombre { get; set; } = null!;

        // --- Propiedad de Navegación 1:N ---

        // Un país puede tener muchos recursos asociados (ej. País de origen de la edición).
        [InverseProperty("IdPaisNavigation")]
        public virtual ICollection<Recurso> Recursos { get; set; } = new List<Recurso>();
    }
}