using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering; // Necesario para SelectListItem (los datos de los dropdowns)

namespace BibliotecaMetrópolis.ViewModels
{
    // Esta clase representa TODO lo que necesitamos para CREAR o EDITAR un recurso :0
    public class RecursoViewModel
    {
        // Clave Primaria (NECESARIA SOLO PARA LA EDICIÓN - EDIT POST)
        
        // Es crucial para el EDIT POST saber qué registro actualizar.
        // Lo mapearemos a un campo oculto en la vista.
        public int IdRecurso { get; set; }

        // CAMPOS DIRECTOS DEL RECURSO (Entradas de Usuario)
        
        [Required(ErrorMessage = "El Título es obligatorio y no puede ir vacío.")]
        [StringLength(200, ErrorMessage = "El título no debe superar los 200 caracteres.")]
        [Display(Name = "Título del Material")]
        public string Titulo { get; set; } = null!;

        [Display(Name = "Edición")]
        [StringLength(50)]
        public string? Edicion { get; set; }

        [Display(Name = "Año de Publicación")]
        [Range(1500, 2100, ErrorMessage = "El año debe ser un valor razonable entre {1} y {2}.")]
        public int? AnioPublicacion { get; set; }
        
        [Display(Name = "Cantidad en Inventario")]
        [Range(1, 1000, ErrorMessage = "La cantidad debe ser mayor a cero.")]
        public int? Cantidad { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un Tipo de Recurso.")]
        [Display(Name = "Tipo de Material")]
        public int IdTipoRecurso { get; set; }

        [Required(ErrorMessage = "Debe seleccionar la Casa Editorial.")]
        [Display(Name = "Casa Editorial")]
        public int IdEditorial { get; set; }
        
        [Display(Name = "País de Publicación")]
        public int? IdPais { get; set; }

        
        [Required(ErrorMessage = "Debe seleccionar OBLIGATORIAMENTE un Autor Principal.")]
        [Display(Name = "Autor Principal")]
        // Este ID se usará para marcar EsPrincipal = true en la tabla RecursoAutor.
        public int IdAutorPrincipal { get; set; }

        [Display(Name = "Otros Autores (Selección Múltiple)")]
        // List<int> recibe los IDs seleccionados de un <select multiple>.
        public List<int>? IdsOtrosAutores { get; set; } 

        [Display(Name = "Palabras Clave (Separadas por coma o punto y coma)")]
        [StringLength(500, ErrorMessage = "El texto de búsqueda no debe superar los 500 caracteres.")]
        // Este campo recoge todo el texto plano que luego se procesa en el Controlador POST.
        public string? PalabrasClaveTexto { get; set; } 

        public string? UrlInstitusion { get; set; }
        public string? ContactoInstitucion { get; set; }

        // COLECCIONES DE SOPORTE PARA LAS VISTAS (Dropdowns)

        // Estas colecciones NO se mapean a la BDD; solo sirven para llenar las etiquetas <option> de los <select>.
        public IEnumerable<SelectListItem>? TiposRecurso { get; set; }
        public IEnumerable<SelectListItem>? Editoriales { get; set; }
        public IEnumerable<SelectListItem>? Paises { get; set; }
        public IEnumerable<SelectListItem>? AutoresDisponibles { get; set; }
    }
}