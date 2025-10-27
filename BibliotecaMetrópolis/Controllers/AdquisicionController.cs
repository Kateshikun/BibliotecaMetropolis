using Microsoft.AspNetCore.Mvc;
using BibliotecaMetrópolis.Models;
using Microsoft.EntityFrameworkCore; // Necesario para usar .Include(), .ThenInclude(), etc.
using BibliotecaMetrópolis.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering; // Necesario para SelectListItem (Dropdowns)

namespace BibliotecaMetrópolis.Controllers
{
    public class AdquisicionController : Controller
    {
        // Esta variable privada guarda la conexión a la BDD (el Contexto).
        private readonly BibliotecaMetropolisContext _context;

        // Constructor: Aquí ASP.NET Core nos "inyecta" el contexto de la base de datos.
        public AdquisicionController(BibliotecaMetropolisContext context)
        {
            _context = context;
        }

        // FUNCIÓN AUXILIAR: Cargar datos para Dropdowns
        // Esta función ayuda a llenar los dropdowns de las vistas Create y Edit.
        private RecursoViewModel CargarDatosApoyo(RecursoViewModel viewModel)
        {
            // Llenamos la lista de Tipos de Recurso. Es clave usar "idTipoR" y "nombre".
            viewModel.TiposRecurso = new SelectList(_context.TipoRecurso.OrderBy(t => t.nombre), "idTipoR", "nombre");

            // Llenamos la lista de Editoriales.
            viewModel.Editoriales = new SelectList(_context.Editorial.OrderBy(e => e.Nombre), "IdEdit", "Nombre");

            // Llenamos la lista de Autores. (Preparamos el nombre completo para que se vea bien en el dropdown)
            viewModel.AutoresDisponibles = new SelectList(_context.Autor
                .Select(a => new {
                    a.IdAutor,
                    NombreCompleto = a.nombres + " " + a.apellidos
                })
                .OrderBy(a => a.NombreCompleto),
                "IdAutor",
                "NombreCompleto");

            return viewModel;
        }

        // ---------------------------------------------------------------------------------------
        // ACCIÓN 1: INDEX (LISTADO)
        // ---------------------------------------------------------------------------------------

        [HttpGet]
        public async Task<IActionResult> Index() // Async y Task<IActionResult> es para que no bloquee la app mientras espera la BDD.
        {
            // Cargo los recursos, pero NECESITO el nombre del Tipo y la Editorial, por eso uso Include().
            var recursos = await _context.Recurso
                .Include(r => r.IdTipoRNavigation)
                .Include(r => r.IdEditNavigation)
                .ToListAsync(); // ToListAsync() ejecuta la consulta.

            return View(recursos); // Paso la lista a la vista para hacer la tabla.
        }

        // ---------------------------------------------------------------------------------------
        // ACCIÓN 2: DETAILS (VER DETALLE)
        // ---------------------------------------------------------------------------------------

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound(); // Si no hay ID, devolvemos un error 404 (No Encontrado)

            // Esto es un parche avanzado para un error de ambigüedad (CS0121). 
            var recurso = await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions
                .Include(_context.Recurso, r => r.IdEditNavigation)
                .Include(r => r.IdTipoRNavigation)
                .Include(r => r.IdPaisNavigation)

                // 1. Carga de Autores (M:N)
                .Include(r => r.RecursoAutor)
                    // ThenInclude nos lleva de la tabla de unión (RecursoAutor) al objeto Autor.
                    .ThenInclude(ra => ra.IdAutorNavigation)

                // 2. Carga de Palabras Clave (M:N)
                .Include(r => r.RecursoPalabraClave)
                    .ThenInclude(rpc => rpc.IdPalabraClaveNavigation)

               // Buscamos el registro por su clave primaria (IdRecurso)
               .FirstOrDefaultAsync(m => m.IdRecurso == id);

            if (recurso == null) return NotFound();

            return View(recurso);
        }

        // ---------------------------------------------------------------------------------------
        // ACCIÓN 3: CREATE (GUARDAR DATOS)
        // ---------------------------------------------------------------------------------------

        public IActionResult Create()
        {
            var viewModel = new RecursoViewModel();   
            viewModel = CargarDatosApoyo(viewModel);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Buena práctica de seguridad. Sirve para evitar ataques CSRF, osea peticiones maliciosas.
        public async Task<IActionResult> Create(RecursoViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(CargarDatosApoyo(viewModel));
            }

            if (viewModel.IdTipoRecurso == 3)
            {
                if (viewModel.IdEditorial != 0)
                {
                    ModelState.AddModelError("IdEditorial",
                        $"No está permitido asociar la tesis con editorial. Para tesis solo se registra información de la Institución educativa.");
                    viewModel.IdEditorial = 0;
                    return View(CargarDatosApoyo(viewModel));
                }

                if (string.IsNullOrWhiteSpace(viewModel.UrlInstitusion))
                {
                    ModelState.AddModelError("UrlInstitusion", "La URL del Website de la Institución es requerida para Tesis.");
                    return View(CargarDatosApoyo(viewModel));
                }

                var nombreInstitucion = viewModel.UrlInstitusion.Trim();

                var descripcionInstitucion = $"URL: {nombreInstitucion} | Contacto: {viewModel.ContactoInstitucion?.Trim() ?? "N/A"}";

                var institucionExistente = await _context.Editorial
                    .FirstOrDefaultAsync(e => e.descripcion != null && e.descripcion.Contains(nombreInstitucion));

                if (institucionExistente != null)
                {
                    viewModel.IdEditorial = institucionExistente.IdEdit;
                }
                else
                {
                    var nuevaInstitucion = new Editorial
                    {
                        Nombre = nombreInstitucion,
                        descripcion = descripcionInstitucion
                    };
                    _context.Editorial.Add(nuevaInstitucion);

                    try
                    {
                        await _context.SaveChangesAsync();
                        viewModel.IdEditorial = nuevaInstitucion.IdEdit;
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, "Error al crear la Institución educativa. Detalle: " + ex.Message);
                        return View(CargarDatosApoyo(viewModel));
                    }
                }
            }

            var nuevoRecurso = new Recurso
            {
                titulo = viewModel.Titulo,
                IdTipoR = viewModel.IdTipoRecurso,
                IdEdit = viewModel.IdEditorial,
                annopublic = viewModel.AnioPublicacion,
                palabrasbusqueda = viewModel.PalabrasClaveTexto,
                RecursoAutor = new List<RecursoAutor>(),
                RecursoPalabraClave = new List<RecursoPalabraClave>()
            };

            _context.Recurso.Add(nuevoRecurso);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error al guardar el Recurso principal. Detalle: " + ex.Message);
                return View(CargarDatosApoyo(viewModel));
            }

            var nuevoRecursoId = nuevoRecurso.IdRecurso;

            var idsAutores = new List<int> { viewModel.IdAutorPrincipal };
            if (viewModel.IdsOtrosAutores != null)
            {
                idsAutores.AddRange(viewModel.IdsOtrosAutores.Where(a => a != viewModel.IdAutorPrincipal).Distinct());
            }

            foreach (var autorId in idsAutores)
            {
                var esPrincipal = autorId == viewModel.IdAutorPrincipal;
                nuevoRecurso.RecursoAutor.Add(new RecursoAutor
                {
                    IdRecurso = nuevoRecursoId,
                    IdAutor = autorId,
                    EsPrincipal = esPrincipal
                });
            }

            var palabrasTexto = viewModel.PalabrasClaveTexto?
                                         .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                                         .Select(p => p.Trim())
                                         .Where(p => !string.IsNullOrWhiteSpace(p))
                                         .ToList() ?? new List<string>();

            foreach (var palabra in palabrasTexto)
            {
                var palabraClave = await _context.PalabraClave
                    .FirstOrDefaultAsync(p => p.Nombre.ToUpper() == palabra.ToUpper());

                if (palabraClave == null)
                {
                    palabraClave = new PalabraClave { Nombre = palabra };
                    _context.PalabraClave.Add(palabraClave);
                }

                nuevoRecurso.RecursoPalabraClave.Add(new RecursoPalabraClave
                {
                    IdRecurso = nuevoRecursoId,
                    IdPalabraClaveNavigation = palabraClave
                });
            }

            try
            {
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error al guardar las relaciones. Detalle: " + ex.Message);
                return View(CargarDatosApoyo(viewModel));
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            // CARGAR EL RECURSO Y TODAS SUS RELACIONES
            // Cargar Autores y Palabras Clave para preseleccionar los dropdowns.
            var recurso = await _context.Recurso
                // Incluimos las relaciones 1:N básicas
                .Include(r => r.IdEditNavigation)
                .Include(r => r.IdTipoRNavigation)

                // Incluimos Autores (M:N) y la entidad final
                .Include(r => r.RecursoAutor)
                    .ThenInclude(ra => ra.IdAutorNavigation)

                // Incluimos Palabras Clave (M:N) y la entidad final
                .Include(r => r.RecursoPalabraClave)
                    .ThenInclude(rpc => rpc.IdPalabraClaveNavigation)

                .FirstOrDefaultAsync(m => m.IdRecurso == id);

            if (recurso == null) return NotFound();


            // Extraer Autores: Determinamos cuál es el Principal y cuáles son Secundarios.
            var autorPrincipal = recurso.RecursoAutor.FirstOrDefault(ra => ra.EsPrincipal);

            var otrosAutoresIds = recurso.RecursoAutor
                .Where(ra => !ra.EsPrincipal)
                // Seleccionamos solo el ID para pasarlo a la List<int> del ViewModel.
                .Select(ra => ra.IdAutor)
                .ToList();

            // Aqui se extraen las palabras Clave: Concatenamos los nombres en un solo string.
            var palabrasClaveTexto = string.Join(", ",
                recurso.RecursoPalabraClave.Select(rpc => rpc.IdPalabraClaveNavigation.Nombre)
            );

            // Mapear al ViewModel
            var viewModel = new RecursoViewModel
            {
                // Necesitamos el ID del Recurso para que el POST sepa qué actualizar.
                IdRecurso = recurso.IdRecurso,

                // Campos Directos
                Titulo = recurso.titulo,
                IdTipoRecurso = recurso.IdTipoR,
                IdEditorial = recurso.IdEdit,
                AnioPublicacion = recurso.annopublic,

                // Relaciones M:N
                IdAutorPrincipal = autorPrincipal?.IdAutor ?? 0, 
                IdsOtrosAutores = otrosAutoresIds,
                PalabrasClaveTexto = palabrasClaveTexto
            };

            // CARGAR LISTAS DE APOYO (Dropdowns)
            viewModel = CargarDatosApoyo(viewModel);

            // Devolvemos el ViewModel precargado a la vista Edit.cshtml.
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(RecursoViewModel viewModel)
        {
            // Validamos que el modelo esté correcto.
            if (!ModelState.IsValid)
            {
                return View(CargarDatosApoyo(viewModel));
            }

            // Se BUSCA EL REGISTRO ORIGINAL CON TODAS SUS COLECCIONES
            var recursoOriginal = await _context.Recurso
                .Include(r => r.RecursoAutor)
                .Include(r => r.RecursoPalabraClave)
                .FirstOrDefaultAsync(m => m.IdRecurso == viewModel.IdRecurso);

            if (recursoOriginal == null)
            {
                return NotFound();
            }

            // ACTUALIZAR CAMPOS DIRECTOS (Tabla Recurso)
            recursoOriginal.titulo = viewModel.Titulo;
            recursoOriginal.IdTipoR = viewModel.IdTipoRecurso;
            recursoOriginal.IdEdit = viewModel.IdEditorial;
            recursoOriginal.annopublic = viewModel.AnioPublicacion;
            recursoOriginal.palabrasbusqueda = viewModel.PalabrasClaveTexto; // Campo plano

            // BORRAR: Eliminamos TODAS las relaciones RecursoAutor actuales.
            _context.AutoresRecursos.RemoveRange(recursoOriginal.RecursoAutor);

            // RE-CREAR: Reutilizamos la lógica del Create.
            var idsAutores = new List<int> { viewModel.IdAutorPrincipal };
            if (viewModel.IdsOtrosAutores != null)
            {
                idsAutores.AddRange(viewModel.IdsOtrosAutores.Where(a => a != viewModel.IdAutorPrincipal).Distinct());
            }

            foreach (var autorId in idsAutores)
            {
                var esPrincipal = autorId == viewModel.IdAutorPrincipal;

                // Creamos la nueva relación con el valor de EsPrincipal.
                recursoOriginal.RecursoAutor.Add(new RecursoAutor
                {
                    IdRecurso = recursoOriginal.IdRecurso,
                    IdAutor = autorId,
                    EsPrincipal = esPrincipal
                });
            }

            // Eliminamos TODAS las relaciones RecursoPalabraClave actuales.
            recursoOriginal.RecursoPalabraClave.Clear();

            // Procesamos el nuevo texto de palabras clave.
            var palabrasTexto = viewModel.PalabrasClaveTexto?
                                        .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                                        .Select(p => p.Trim())
                                        .Where(p => !string.IsNullOrWhiteSpace(p))
                                        .ToList() ?? new List<string>();

            foreach (var palabra in palabrasTexto)
            {
                // Buscamos si existe o la creamos.
                var palabraClave = await _context.PalabraClave
                    .FirstOrDefaultAsync(p => p.Nombre.ToUpper() == palabra.ToUpper());

                if (palabraClave == null)
                {
                    palabraClave = new PalabraClave { Nombre = palabra };
                    _context.PalabraClave.Add(palabraClave);
                }

                // Creamos la nueva relación M:N.
                recursoOriginal.RecursoPalabraClave.Add(new RecursoPalabraClave
                {
                    IdRecurso = recursoOriginal.IdRecurso,
                    IdPalabraClave = palabraClave.IdPalabraClave
                });
            }

            try
            {
                //GUARDAR TODO: Actualización del Recurso, eliminación y re-inserción de las relaciones.
                await _context.SaveChangesAsync();

                // Redirigir a los detalles para ver la edición aplicada.
                return RedirectToAction(nameof(Details), new { id = recursoOriginal.IdRecurso });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error al guardar la edición y sincronizar las relaciones. Detalle: " + ex.Message);
                return View(CargarDatosApoyo(viewModel));
            }
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            // Cargamos el recurso y sus relaciones para mostrar un resumen en la página de confirmación.
            var recurso = await _context.Recurso
                .Include(r => r.IdEditNavigation)
                .Include(r => r.IdTipoRNavigation)

                // Incluimos Autores para mostrar el principal
                .Include(r => r.RecursoAutor)
                    .ThenInclude(ra => ra.IdAutorNavigation)

                // Incluimos Palabras Clave
                .Include(r => r.RecursoPalabraClave)
                    .ThenInclude(rpc => rpc.IdPalabraClaveNavigation)

                .FirstOrDefaultAsync(m => m.IdRecurso == id);

            if (recurso == null) return NotFound();

            return View(recurso);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Buscamos el recurso por ID. 
            var recurso = await _context.Recurso.FindAsync(id);

            if (recurso != null)
            {
                // Le decimos al DbContext que elimine el registro.
                _context.Recurso.Remove(recurso);
            }

            try
            {
                // Guardamos los cambios.
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Esto captura errores si la BDD tiene restricciones que EF Core no pudo resolver.
                ModelState.AddModelError(string.Empty, "Error al eliminar el recurso. Verifique que no esté referenciado en otras tablas.");

                // Carga los detalles para volver a mostrar la vista Delete con el mensaje de error.
                var recursoFallido = await _context.Recurso.FirstOrDefaultAsync(m => m.IdRecurso == id);
                if (recursoFallido == null) return RedirectToAction(nameof(Index));

                return View("Delete", recursoFallido);
            }

            //Redirigimos al listado principal.
            return RedirectToAction(nameof(Index));
        }

    }
}