using System.ComponentModel.DataAnnotations;


namespace herejes_del_sazon.Models.ViewModels
{
    public class ReservationViewModel
    {
        public int IdMesa { get; set; }

        [Required]
        public string NombreCliente { get; set; } = "";


        [Required]
        [EmailAddress]
        public string Correo { get; set; } = "";

        [Required]
        public string Telefono { get; set; } = "";

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public TimeSpan Hora { get; set; }

        [Range(1,20)]
        public int Personas { get; set; }

        public string Observaciones { get; set; } = "";

        public List<TableCardViewModel> Mesas { get; set; } = new();

        
    }
}