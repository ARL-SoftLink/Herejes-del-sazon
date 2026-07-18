using System.Collections.Generic;

namespace herejes_del_sazon.Models.ViewModels
{
    public class MenuViewModel
    {
        public List<FoodCardViewModel> Platillos { get; set; } = new();

        public List<string> Categorias { get; set; } = new();
    }
}