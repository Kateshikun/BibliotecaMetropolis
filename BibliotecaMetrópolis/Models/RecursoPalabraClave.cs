using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaMetrópolis.Models
{
    // Clave compuesta para la relación M:N
    [PrimaryKey("IdRecurso", "IdPalabraClave")]
    [Table("RecursoPalabraClave")]
    public partial class RecursoPalabraClave
    {
        public int IdRecurso { get; set; }
        public int IdPalabraClave { get; set; }

        // Propiedades de navegación para la entidad Recurso
        [ForeignKey("IdRecurso")]
        public virtual Recurso IdRecursoNavigation { get; set; } = null!;

        // Propiedades de navegación para la entidad PalabraClave
        [ForeignKey("IdPalabraClave")]
        public virtual PalabraClave IdPalabraClaveNavigation { get; set; } = null!;
    }
}