using BibliotecaMVC_BD.Datos;
using Microsoft.AspNetCore.Mvc;
using BibliotecaMVC_BD.Models;
using Microsoft.Data.SqlClient;

namespace BibliotecaMVC_BD.Controllers
{
    // Controlador para manejar las operaciones relacionadas con los libros.
    public class LibrosController : Controller // Hereda de Controller para manejar las solicitudes HTTP y devolver vistas.
    {
        private readonly LibroDatos _libroDatos;

        public LibrosController(IConfiguration configuracion) // Constructor que recibe la configuración para inicializar la conexión a la base de datos.
        {
            _libroDatos = new LibroDatos(configuracion);
        }

        [HttpGet] // Acción para mostrar la lista de libros, con un filtro opcional.
        public IActionResult Index(string? filtro) // Recibe un parámetro de filtro opcional para buscar libros por código, título o autor.
        {
            ViewData["Filtro"] = filtro;
            ViewData["TotalLibros"] = _libroDatos.ContarLibros();

            var libros = _libroDatos.ObtenerTodos(filtro);
            return View(libros);
        }

        [HttpGet] // Acción para mostrar el formulario de registro de un nuevo libro.
        public IActionResult Registrar()
        {
            return View();
        }

        [HttpPost] // Acción para manejar el envío del formulario de registro de un nuevo libro. Valida los datos y registra el libro en la base de datos.
        [ValidateAntiForgeryToken] // Protege contra ataques CSRF (Cross-Site Request Forgery).
        public IActionResult Registrar(Libro libro) // Recibe un objeto Libro con los datos del formulario.
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

        [HttpGet] // Acción para mostrar el formulario de edición de un libro existente.
        public IActionResult Detalle(int id) // Recibe el ID del libro a editar y obtiene sus datos de la base de datos.
        {
            var libro = _libroDatos.ObtenerPorId(id);

            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }

        [HttpGet] // Acción para mostrar el formulario de edición de un libro existente.
        public IActionResult Eliminar(int id) // Recibe el ID del libro a eliminar y obtiene sus datos de la base de datos.
        {
            var libro = _libroDatos.ObtenerPorId(id);

            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }

        [HttpPost] // Acción para manejar la confirmación de eliminación de un libro. Valida el ID y elimina el libro de la base de datos.
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