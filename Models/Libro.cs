using System;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaMVC_BD.Models
{
    public class Libro
    {
        public int IdLibro { get; set; }

        [Required(ErrorMessage = "El código es obligatorio.")]
        [StringLength(10, ErrorMessage = "El código no puede tener más de 10 caracteres.")]
        [Display(Name = "Código")]
        public string Codigo { get; set; } = string.Empty;

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