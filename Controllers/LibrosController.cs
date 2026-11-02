using BibliotecaMVC_BD.Datos;
using Microsoft.AspNetCore.Mvc;
using BibliotecaMVC_BD.Models;
using Microsoft.Data.SqlClient;

namespace BibliotecaMVC_BD.Controllers
{
    public class LibrosController : Controller
    {
        private readonly LibroDatos _libroDatos;

        public LibrosController(IConfiguration configuracion)
        {
            _libroDatos = new LibroDatos(configuracion);
        }

        [HttpGet]
        public IActionResult Index(string? filtro)
        {
            ViewData["Filtro"] = filtro;
            ViewData["TotalLibros"] = _libroDatos.ContarLibros();

            var libros = _libroDatos.ObtenerTodos(filtro);
            return View(libros);
        }

        [HttpGet]
        public IActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Registrar(Libro libro)
        {
            if (libro.FechaPublicacion.HasValue &&
                libro.FechaPublicacion.Value.Date > DateTime.Today)
            {
                ModelState.AddModelError(
                    nameof(libro.FechaPublicacion),
                    "La fecha de publicación no puede ser futura.");
            }

            if (!ModelState.IsValid)
            {
                return View(libro);
            }

            try
            {
                _libroDatos.Registrar(libro);

                TempData["MensajeExito"] = "El libro se registró correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (SqlException ex) when (ex.Number == 2601 || ex.Number == 2627)
            {
                ModelState.AddModelError(
                    nameof(libro.Codigo),
                    "Ya existe un libro con ese código.");

                return View(libro);
            }
        }

        [HttpGet]
        public IActionResult Detalle(int id)
        {
            var libro = _libroDatos.ObtenerPorId(id);

            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }

        [HttpGet]
        public IActionResult Eliminar(int id)
        {
            var libro = _libroDatos.ObtenerPorId(id);

            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmarEliminacion(int id)
        {
            bool eliminado = _libroDatos.Eliminar(id);

            if (!eliminado)
            {
                return NotFound();
            }

            TempData["MensajeExito"] = "El libro se eliminó correctamente.";
            return RedirectToAction(nameof(Index));
        }

    }// fin de la clase LibrosController
}