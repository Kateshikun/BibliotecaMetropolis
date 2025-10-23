using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BibliotecaMetrópolis.Models;
using BibliotecaMetrópolis.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BibliotecaMetrópolis.Controllers
{
    public class BusquedaController : Controller
    {
        private readonly BibliotecaMetropolisContext _context;

        public BusquedaController(BibliotecaMetropolisContext context)
        {
            _context = context;
        }

        // GET: Búsqueda principal con múltiples campos
        public IActionResult Index()
        {
            var viewModel = new BusquedaViewModel
            {
                TiposRecurso = new SelectList(_context.TipoRecurso.OrderBy(t => t.nombre), "idTipoR", "nombre"),
                Editoriales = new SelectList(_context.Editorial.OrderBy(e => e.Nombre), "IdEdit", "Nombre"),
                Anios = Enumerable.Range(DateTime.Now.Year - 10, 11).Select(y => new SelectListItem
                {
                    Value = y.ToString(),
                    Text = y.ToString()
                })
            };

            return View(viewModel);
        }

        // POST: Procesar búsqueda
        [HttpPost]
        public async Task<IActionResult> Resultados(BusquedaViewModel busqueda)
        {
            if (!ModelState.IsValid)
            {
                // Recargar datos de apoyo si hay error
                busqueda.TiposRecurso = new SelectList(_context.TipoRecurso.OrderBy(t => t.nombre), "idTipoR", "nombre");
                busqueda.Editoriales = new SelectList(_context.Editorial.OrderBy(e => e.Nombre), "IdEdit", "Nombre");
                return View("Index", busqueda);
            }

            // Construir consulta base
            var query = _context.Recurso
                .Include(r => r.IdTipoRNavigation)
                .Include(r => r.IdEditNavigation)
                .Include(r => r.RecursoAutor)
                    .ThenInclude(ra => ra.IdAutorNavigation)
                .Include(r => r.RecursoPalabraClave)
                    .ThenInclude(rpc => rpc.IdPalabraClaveNavigation)
                .AsQueryable();

            // Aplicar filtros según los campos completados
            if (!string.IsNullOrWhiteSpace(busqueda.Autor))
            {
                query = query.Where(r => r.RecursoAutor.Any(ra =>
                    ra.IdAutorNavigation.nombres.Contains(busqueda.Autor) ||
                    ra.IdAutorNavigation.apellidos.Contains(busqueda.Autor)));
            }

            if (!string.IsNullOrWhiteSpace(busqueda.PalabraClave))
            {
                query = query.Where(r =>
                    r.RecursoPalabraClave.Any(rpc =>
                        rpc.IdPalabraClaveNavigation.Nombre.Contains(busqueda.PalabraClave)) ||
                    r.palabrasbusqueda.Contains(busqueda.PalabraClave));
            }

            if (busqueda.IdEditorial.HasValue && busqueda.IdEditorial > 0)
            {
                query = query.Where(r => r.IdEdit == busqueda.IdEditorial.Value);
            }

            if (busqueda.IdTipoRecurso.HasValue && busqueda.IdTipoRecurso > 0)
            {
                query = query.Where(r => r.IdTipoR == busqueda.IdTipoRecurso.Value);
            }

            if (busqueda.AnioPublicacion.HasValue)
            {
                query = query.Where(r => r.annopublic == busqueda.AnioPublicacion.Value);
            }

            var resultados = await query.ToListAsync();

            var viewModel = new ResultadosBusquedaViewModel
            {
                Resultados = resultados,
                TerminosBusqueda = busqueda,
                TotalResultados = resultados.Count
            };

            return View(viewModel);
        }
    }
}