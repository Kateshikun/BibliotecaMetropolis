using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaMetrópolis.Models
{
    // [PrimaryKey] define la clave primaria compuesta antes de OnModelCreating
    [PrimaryKey("IdRecurso", "IdAutor")]
    [Table("RecursoAutor")]
    public partial class RecursoAutor
    {
        [Column("IdRecurso")] // Mapea a la columna IdRec en la tabla Recurso
        public int IdRecurso { get; set; }

        public int IdAutor { get; set; }

        // Campo CLAVE: Este campo es la razón por la que necesitamos una clase de unión explícita.
        public bool EsPrincipal { get; set; }

        // --- Propiedades de Navegación (para unir las dos entidades) ---

        [ForeignKey("IdRecurso")]
        public virtual Recurso IdRecursoNavigation { get; set; } = null!;

        [ForeignKey("IdAutor")]
        public virtual Autor IdAutorNavigation { get; set; } = null!;
    }
}