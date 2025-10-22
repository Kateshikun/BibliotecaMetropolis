using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BibliotecaMetrópolis.Models
{
    [Table("PalabraClave")]
    public partial class PalabraClave
    {
        [Key]
        public int IdPalabraClave { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = null!;

        // --- Propiedad de Navegación M:N ---

        // Esta colección une PalabraClave con Recurso a través de la entidad RecursoPalabraClave.
        // Es una forma eficiente y relacional de manejar el etiquetado.
        [InverseProperty("IdPalabraClaveNavigation")]
        public virtual ICollection<RecursoPalabraClave> RecursoPalabraClaves { get; set; } = new List<RecursoPalabraClave>();
    }
}