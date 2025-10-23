using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BibliotecaMetrópolis.Models;
using System;
using Azure;

namespace BibliotecaMetrópolis.Controllers
{
    

    public class AutoresController : Controller // Controlador que gestiona las operaciones CRUD para la entidad Autor.
    {     
        private readonly BibliotecaMetropolisContext _context; // Contexto de base de datos inyectado por el sistema y Permite acceder a las tablas definidas en BibliotecaMetropolisContext.

        public AutoresController(BibliotecaMetropolisContext context) // Constructor que recibe el contexto por inyección de dependencias.
        {
            _context = context;
        }
        // Acción que muestra la lista de todos los autores registrados y se ejecuta cuando se accede a /Autores
        public async Task<IActionResult> Index()
        {
            var autores = await _context.Autor.ToListAsync();  // Obtiene todos los registros de la tabla Autor.
            return View(autores); // Retorna la vista Index con la lista de autores.
        }

        // GET: Autores/Details/5
        public async Task<IActionResult> Details(int? id)    // Acción que muestra los detalles de un autor específico y se ejecuta cuando se accede a /Autores/Details/{id}
        {
            if (id == null) return NotFound(); // Si no se proporciona un ID, retorna error 404.

            var autor = await _context.Autor // Busca el autor por su ID.
                .FirstOrDefaultAsync(m => m.IdAutor == id);

            if (autor == null) return NotFound(); // Si no se encuentra, retorna error 404.

            return View(autor); // Retorna la vista con los datos del autor.
        }

        // GET: Autores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Autores/Create
        [HttpPost]  // Acción que recibe los datos modificados del autor
        [ValidateAntiForgeryToken] // Se ejecuta cuando se envía el formulario de edición
        public async Task<IActionResult> Create([Bind("nombres,apellidos")] Autor autor)
        {

            if (ModelState.IsValid) // Verifica que el ID recibido coincida con el del modelo.
            {
                _context.Add(autor); // Actualiza el autor en la base de datos.
                await _context.SaveChangesAsync();  // Guarda los cambios de forma asincrónica en la base de datos.
                return RedirectToAction(nameof(Index)); // Redirige al usuario a la vista Index (lista de autores).
            }
            return View(autor); // Si el modelo no es válido, vuelve a mostrar el formulario con los datos ingresados.
        }

        // GET: Autores/Edit/5
        public async Task<IActionResult> Edit(int? id) // Acción GET que muestra el formulario para editar un autor existente.
        {
            if (id == null) return NotFound(); // Verifica que se haya recibido un ID válido.

            var autor = await _context.Autor.FindAsync(id); // Busca el autor por su ID en la base de datos.
            if (autor == null) return NotFound();  // Si no se encuentra el autor, retorna error 404.

            return View(autor); // Retorna la vista de edición con los datos del autor.
        }

        // POST: Autores/Edit/5
        [HttpPost] //Acción POST que recibe los datos modificados del autor para actualizarlo.
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdAutor,nombres,apellidos")] Autor autor)
        {
            if (id != autor.IdAutor) return NotFound(); // Verifica que el ID recibido coincida con el del modelo.

            if (ModelState.IsValid) // Verifica que los datos del modelo sean válidos.
            {
                try
                {   
                    _context.Update(autor); // Actualiza el autor en el contexto de base de datos.
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Autor.Any(e => e.IdAutor == id)) return NotFound(); // Verifica si el autor aún existe en la base de datos.
                    else throw; // Si existe pero hay conflicto de concurrencia, lanza la excepción.
                }
                return RedirectToAction(nameof(Index)); // Redirige al usuario a la vista Index.
            }
            return View(autor); // Si el modelo no es válido, vuelve a mostrar el formulario con los datos ingresados.
        }

        // GET: Autores/Delete/5
        public async Task<IActionResult> Delete(int? id) // Acción GET que muestra la confirmación para eliminar un autor.
        {
            if (id == null) return NotFound(); // Verifica que se haya recibido un ID válido.

            var autor = await _context.Autor  // Busca el autor por su ID.
                .FirstOrDefaultAsync(m => m.IdAutor == id);

            if (autor == null) return NotFound(); // Si no se encuentra el autor, retorna error 404.

            return View(autor);  // Retorna la vista de confirmación de eliminación.
        }

        // POST: Autores/Delete/5
        [HttpPost, ActionName("Delete")] // Indica que esta acción responde al formulario Delete.
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var autor = await _context.Autor.FindAsync(id); // Busca el autor por su ID
            if (autor != null) // Si se encuentra, lo elimina del contexto.
            {
                _context.Autor.Remove(autor);
                await _context.SaveChangesAsync(); // Guarda los cambios en la base de datos.
            }
            return RedirectToAction(nameof(Index)); // Redirige al usuario a la vista Index.
        }
    }
}
