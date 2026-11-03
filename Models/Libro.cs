using System;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaMVC_BD.Models
{
    // Clase que representa un libro en la biblioteca, con propiedades para el código, título, autor y fecha de publicación.
    public class Libro
    {
        public int IdLibro { get; set; }

        [Required(ErrorMessage = "El código es obligatorio.")] // Atributo de validación que indica que el código es obligatorio.
        [StringLength(10, ErrorMessage = "El código no puede tener más de 10 caracteres.")] // Atributo de validación que indica que el código no puede tener más de 10 caracteres.
        [Display(Name = "Código")] // Atributo que indica el nombre a mostrar en la vista para esta propiedad.
        public string Codigo { get; set; } = string.Empty; // Inicializa la propiedad con una cadena vacía para evitar valores nulos.

        [Required(ErrorMessage = "El título es obligatorio.")]
        [StringLength(100, ErrorMessage = "El título no puede tener más de 100 caracteres.")]
        [Display(Name = "Título")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El autor es obligatorio.")]
        [StringLength(50, ErrorMessage = "El autor no puede tener más de 50 caracteres.")]
        public string Autor { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de publicación es obligatoria.")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de publicación")]
        public DateTime? FechaPublicacion { get; set; }
    }
}