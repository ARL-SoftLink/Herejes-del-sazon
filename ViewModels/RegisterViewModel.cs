using System.ComponentModel.DataAnnotations;

namespace herejes_del_sazon.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string Nombre { get; set; } = "";

        [Required]
        public string Apellido { get; set; } = "";

        [Required]
        [EmailAddress]
        public string Correo { get; set; } = "";

        [Required]
        public string Contraseña { get; set; } = "";

        public string? Telefono { get; set; }
    }
}