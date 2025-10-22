using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BibliotecaMetrópolis.Models
{
    [Table("TipoRecurso")]
    public partial class TipoRecurso
    {
        [Key]
        public int idTipoR { get; set; }

        [Required]
        [StringLength(50)]
        public string nombre { get; set; } = null!;

        [StringLength(255)]
        public string? descripcion { get; set; }


        // Un TipoRecurso puede clasificar a muchos Recursos.
        [InverseProperty("IdTipoRNavigation")]
        public virtual ICollection<Recurso> Recursos { get; set; } = new List<Recurso>();
    }
}