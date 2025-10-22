using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BibliotecaMetrópolis.Models
{
    // [Table] mapea esta clase a la tabla 'Autor' en la base de datos.
    public partial class Autor
    {
        // [Key] define la clave primaria de la tabla.
        [Key]
        public int IdAutor { get; set; }

        [Required]
        [StringLength(100)]
        public string nombres { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string apellidos { get; set; } = null!;

        // --- Propiedad de Navegación M:N ---

        // ICollection<T> representa la relación de uno a muchos (1:N). 
        // En este caso, representa la relación M:N con Recurso a través de la tabla de unión.
        // El atributo [InverseProperty] le dice a EF Core que esta colección es el lado opuesto 
        // de la propiedad 'IdAutorNavigation' que se define en la clase AutoresRecursos.
        // **Decisión de Diseño:** Usamos la tabla de unión explícita (AutoresRecursos)
        // porque necesitamos almacenar el dato adicional 'EsPrincipal'.
        [InverseProperty("IdAutorNavigation")]
        public virtual ICollection<RecursoAutor> RecursoAutor { get; set; } = new List<RecursoAutor>();
    }
}