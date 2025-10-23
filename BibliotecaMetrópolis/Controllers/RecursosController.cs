using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BibliotecaMetrópolis.Models;
using BibliotecaMetrópolis.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BibliotecaMetrópolis.Controllers
{
    public class RecursosController : Controller
    {
        private readonly BibliotecaMetropolisContext _context;

        public RecursosController(BibliotecaMetropolisContext context)
        {
            _context = context;
        }

        // GET: Listar recursos por tipo específico con filtro de antigüedad
        public async Task<IActionResult> ListarPorTipo(int? idTipo, int? anosAtras)
        {
            // Valores por defecto
            var anosFiltro = anosAtras ?? 10;
            var tipoSeleccionado = idTipo;

            // Obtener tipos de recurso para el dropdown
            var tiposRecurso = await _context.TipoRecurso
                .OrderBy(t => t.nombre)
                .ToListAsync();

            ViewBag.TiposRecurso = new SelectList(tiposRecurso, "idTipoR", "nombre", tipoSeleccionado);
            ViewBag.AnosAtras = anosFiltro;

            // Construir consulta base
            var query = _context.Recurso
                .Include(r => r.IdTipoRNavigation)
                .Include(r => r.IdEditNavigation)
                .Include(r => r.RecursoAutor)
                    .ThenInclude(ra => ra.IdAutorNavigation)
                .AsQueryable();

            // Aplicar filtros
            if (tipoSeleccionado.HasValue)
            {
                query = query.Where(r => r.IdTipoR == tipoSeleccionado.Value);
            }

            // Filtrar por antigüedad (hasta X años atrás)
            var añoLimite = DateTime.Now.Year - anosFiltro;
            query = query.Where(r => r.annopublic >= añoLimite);

            var recursos = await query
                .OrderByDescending(r => r.annopublic)
                .ThenBy(r => r.titulo)
                .ToListAsync();

            return View(recursos);
        }

        // POST: Aplicar filtros
        [HttpPost]
        public IActionResult ListarPorTipo(int idTipo, int anosAtras)
        {
            return RedirectToAction("ListarPorTipo", new { idTipo, anosAtras });
        }
    }
}