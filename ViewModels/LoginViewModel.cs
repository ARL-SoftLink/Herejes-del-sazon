using System.ComponentModel.DataAnnotations;

namespace herejes_del_sazon.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Ingrese su correo.")]
        [EmailAddress]
        public string Correo { get; set; } = "";

        [Required(ErrorMessage = "Ingrese su contraseña.")]
        public string Contraseña { get; set; } = "";
    }
}