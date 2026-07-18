namespace herejes_del_sazon.Models.ViewModels
{
    public class TableCardViewModel
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = "";

        public int Capacidad { get; set; }

        public bool Disponible { get; set; }

        public string Estado =>
            Disponible ? "Disponible" : "Ocupada";

        public string TipoMesa { get; set; } = "";
    }
}