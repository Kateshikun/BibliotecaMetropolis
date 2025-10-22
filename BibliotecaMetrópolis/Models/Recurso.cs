using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BibliotecaMetrópolis.Models
{
    // Mapea la clase a la tabla "Recurso" en SQL Server
    [Table("Recurso")]
    public partial class Recurso
    {
        // [Key] indica que esta propiedad es la clave primaria de la tabla
        [Key]
        [Column("IdRecurso")]
        public int IdRecurso { get; set; }

        [Required] // Indica que la columna no puede ser nula (NOT NULL)
        [StringLength(200)] // Límite de longitud para la columna en la BDD
        public string titulo { get; set; } = null!;

        public int? annopublic { get; set; } // int? permite valores nulos en la BDD
        [StringLength(50)]
        public string? edicion { get; set; }

        // Campo extraído directamente del diagrama ER.
        [StringLength(500)]
        public string? palabrasbusqueda { get; set; }

        [StringLength(1000)]
        public string? descripcion { get; set; }

        // --- Claves Foráneas (FKs) ---
        public int? IdPais { get; set; }

        [Column("idTipoR")] // Mapea a 'idTipoR' en la BDD, que tiene minúsculas
        public int IdTipoR { get; set; }

        public int IdEdit { get; set; }

        // --- Propiedades de Navegación (Relaciones 1:N) ---

        // 'virtual' es una convención de EF Core para Lazy Loading (carga perezosa),
        // aunque en Core se recomienda usar Eager Loading (.Include()).

        // Relación con Editorial
        [ForeignKey("IdEdit")]
        [InverseProperty("Recursos")] // Indica la colección inversa en la clase Editorial
        public virtual Editorial IdEditNavigation { get; set; } = null!;

        // Relación con País (Puede ser nulo)
        [ForeignKey("IdPais")]
        [InverseProperty("Recursos")]
        public virtual Pais? IdPaisNavigation { get; set; }

        // Relación con TipoRecurso
        [ForeignKey("IdTipoR")]
        [InverseProperty("Recursos")]
        public virtual TipoRecurso IdTipoRNavigation { get; set; } = null!;

        // --- Propiedades de Navegación (Relaciones M:N vía Tablas de Unión Explícitas) ---

        // Relación M:N con Autor, a través de la tabla AutoresRecursos.
        // Esto es necesario para acceder al campo EsPrincipal.
        [InverseProperty("IdRecursoNavigation")]
        public virtual ICollection<RecursoAutor> RecursoAutor { get; set; } = new List<RecursoAutor>();

        // Relación M:N con PalabraClave, a través de la tabla RecursoPalabraClave.
        [InverseProperty("IdRecursoNavigation")]
        public virtual ICollection<RecursoPalabraClave> RecursoPalabraClave { get; set; } = new List<RecursoPalabraClave>();
    }
}