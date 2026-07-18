namespace herejes_del_sazon.Models.ViewModels
{
    public class DishDetailViewModel
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = "";

        public string Categoria { get; set; } = "";

        public string Descripcion { get; set; } = "";

        public decimal Precio { get; set; }

        public string Imagen { get; set; } = "";

        public List<string> Etiquetas { get; set; } = new();

        public List<string> Ingredientes { get; set; } = new();

        public List<string> PerfilesFamiliares { get; set; } = new();

        public bool Disponible { get; set; }
        
    }
}