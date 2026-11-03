using BibliotecaMVC_BD.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BibliotecaMVC_BD.Controllers

// Controlador para manejar las vistas de la página principal y la privacidad.
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        // Acción para manejar errores y mostrar la vista de error.

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
