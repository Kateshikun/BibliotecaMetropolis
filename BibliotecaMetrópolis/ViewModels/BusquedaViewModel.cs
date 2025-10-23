using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BibliotecaMetrópolis.ViewModels
{
    public class BusquedaViewModel
    {
        [Display(Name = "Autor")]
        [StringLength(100, ErrorMessage = "El nombre del autor no puede exceder 100 caracteres")]
        public string? Autor { get; set; }

        [Display(Name = "Palabra Clave")]
        [StringLength(100, ErrorMessage = "La palabra clave no puede exceder 100 caracteres")]
        public string? PalabraClave { get; set; }

        [Display(Name = "Editorial")]
        public int? IdEditorial { get; set; }

        [Display(Name = "Tipo de Recurso")]
        public int? IdTipoRecurso { get; set; }

        [Display(Name = "Año de Publicación")]
        [Range(1900, 2100, ErrorMessage = "El año debe ser entre 1900 y 2100")]
        public int? AnioPublicacion { get; set; }

        // Propiedades para llenar dropdowns
        public IEnumerable<SelectListItem>? TiposRecurso { get; set; }
        public IEnumerable<SelectListItem>? Editoriales { get; set; }
        public IEnumerable<SelectListItem>? Anios { get; set; }
    }
}