using Microsoft.EntityFrameworkCore;
// Se usan DataAnnotations (ej. [Table], [Key]) y EF Core.

namespace BibliotecaMetrópolis.Models
{
    // 'partial class' permite extender la clase en otros archivos si fuera necesario, 
    // pero aquí es el punto central para interactuar con la BDD.
    public partial class BibliotecaMetropolisContext : DbContext
    {
        // Constructor requerido por ASP.NET Core para la Inyección de Dependencias (DI).
        // El framework usa este constructor para pasar las opciones de conexión (ej. la cadena de conexión).
        public BibliotecaMetropolisContext(DbContextOptions<BibliotecaMetropolisContext> options)
            : base(options)
        {
        }

        // Definición de las Tablas (DbSet)
        // Cada DbSet mapea una clase (Modelo) a una tabla en la BDD.

        public virtual DbSet<Autor> Autor { get; set; } = null!;
        public virtual DbSet<Editorial> Editorial { get; set; } = null!;
        public virtual DbSet<Pais> Pais { get; set; } = null!;
        public virtual DbSet<PalabraClave> PalabraClave { get; set; } = null!;
        public virtual DbSet<Recurso> Recurso { get; set; } = null!;
        public virtual DbSet<TipoRecurso> TipoRecurso { get; set; } = null!;

        // Tablas de unión (M:N) modeladas explícitamente. Son requeridas
        // porque llevan datos adicionales (EsPrincipal) o para control avanzado.
        public virtual DbSet<RecursoAutor> AutoresRecursos { get; set; } = null!;
        public virtual DbSet<PalabraClave> RecursoPalabraClave { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // --- Configuración de Claves Primarias Compuestas ---
            // Aunque se usan atributos [PrimaryKey] en las clases, es buena práctica
            // confirmarlas aquí para asegurar que EF Core las mapee correctamente.

            // 1. Clave compuesta para AutoresRecursos (IdRec, IdAutor)
            // Esto le dice a EF que la combinación de estos dos campos es única y es la PK.
            modelBuilder.Entity<RecursoAutor>()
                .HasKey(ar => new { ar.IdRecurso, ar.IdAutor });

            // 2. Clave compuesta para RecursoPalabraClave (IdRecurso, IdPalabraClave)
            modelBuilder.Entity<RecursoPalabraClave>()
        .HasKey(rpc => new { rpc.IdRecurso, rpc.IdPalabraClave });

            // Llama a la implementación base para que EF Core finalice el mapeo de todas las FKs restantes.
            base.OnModelCreating(modelBuilder);
        }
    }
}