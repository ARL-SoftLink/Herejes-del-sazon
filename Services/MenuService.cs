using herejes_del_sazon.Models.ViewModels;
using herejes_del_sazon.Models;
using Microsoft.EntityFrameworkCore;

namespace herejes_del_sazon.Services
{
    public class MenuService
    {
        private readonly MyDBContext _context;

        private FoodCardViewModel MapToFoodCard(Platillo platillo)
        {
           return new FoodCardViewModel
          {
            Id = platillo.IdPlatillo,

            Nombre = platillo.Nombre ?? "Platillo sin nombre",

            Descripcion = platillo.Descripcion ?? "Sin descripción",

            Precio = platillo.Precio ?? 0,

            Imagen = platillo.ImagenUrl ?? "/images/platillos/default.jpg",

            Categoria = platillo.IdCategoriaNavigation?.NombreCategoria ?? "Sin categoría",

            Etiquetas = new List<string>()
           };
        }
        
        public MenuService(MyDBContext context)
        {
            _context = context;
        }
        // ===============================
        // MÉTODOS PÚBLICOS
        // ===============================

        public List<FoodCardViewModel> GetFeaturedDishes()
        {
         var platillosBD = _context.Platillos
            .Include(p => p.IdCategoriaNavigation)
            .Where(p => p.Disponible == true)
           .ToList();

         if (platillosBD.Any())
         {
            return platillosBD
            .Select(MapToFoodCard)
            .ToList();
          }

          return GetMockFeaturedDishes();
        }

        public List<FoodCardViewModel> GetAllDishes()
        {
            var platillosBD = _context.Platillos
              .Include(p => p.IdCategoriaNavigation)
              .Where(p => p.Disponible == true)
              .ToList();

            if (platillosBD.Any())
            {
              return platillosBD
              .Select(MapToFoodCard)
              .ToList();
            }

          return GetMockAllDishes();
        }

        public List<FoodCardViewModel> GetDishesByCategory(string categoria)
        {
            var platillosBD = _context.Platillos
               .Include(p => p.IdCategoriaNavigation)
               .Where(p =>
                 p.Disponible == true &&
                 p.IdCategoriaNavigation != null &&
                 p.IdCategoriaNavigation.NombreCategoria == categoria)
               .ToList();

            if (platillosBD.Any())
            {
              return platillosBD
                .Select(MapToFoodCard)
                .ToList();
            }

            return GetMockAllDishes()
               .Where(x => x.Categoria == categoria)
               .ToList();
        }


        public FoodCardViewModel? GetDishById(int id)
        {
            return GetMockAllDishes()
                .FirstOrDefault(x => x.Id == id);
        }
        
        
        // ===============================
        // DATOS DE DEMOSTRACIÓN
        // ===============================

        private List<FoodCardViewModel> GetMockFeaturedDishes()
        {
            return GetMockAllDishes()
                .Where(x => x.Categoria == "Especial")
                .ToList();
        }

        private List<FoodCardViewModel> GetMockAllDishes()
        {
            return new List<FoodCardViewModel>()
            {
                new FoodCardViewModel
                {
                    Id = 1,
                    Nombre = "Pupusas Herejes",
                    Categoria = "Especial",
                    Descripcion = "Elaboradas con maíz criollo y quesillo artesanal.",
                    Precio = 7.99m,
                    Imagen = "/images/platillos/pupusas.jpg",//    https://images.squarespace-cdn.com/content/v1/64c58e1fde62886ac69f49d8/598f3c19-39ec-43ca-9f05-b3e3f545bf07/pupusas-de-camaron-con-queso-cocinandose-capitan-marisco-mariscos-frescos-a-domicilio-el-salvador.jpg
                    Etiquetas = new List<string>
                    {
                        "Apto para niños",
                        "Sin picante"
                    }
                },

                new FoodCardViewModel
                {
                    Id = 2,
                    Nombre = "Tamal Centroamericano",
                    Categoria = "Especial",
                    Descripcion = "Preparado con ingredientes frescos provenientes de productores locales.",
                    Precio = 6.50m,
                    Imagen = "/images/platillos/tamal.jpg",
                    Etiquetas = new List<string>
                    {
                        "Tradicional",
                        "Textura suave"
                    }
                },

                new FoodCardViewModel
                {
                    Id = 3,
                    Nombre = "Horchata Artesanal",
                    Categoria = "Bebida",
                    Descripcion = "Bebida tradicional preparada con semillas seleccionadas.",
                    Precio = 3.50m,
                    Imagen = "/images/platillos/horchata.jpg",
                    Etiquetas = new List<string>
                    {
                        "Refrescante"
                    }
                },

                new FoodCardViewModel
                {
                   Id = 4,
                   Nombre = "Yuca Frita",
                   Categoria = "Entrada",
                   Descripcion = "Yuca frita acompañada de curtido y salsa de tomate.",
                   Precio = 5.25m,
                   Imagen = "/images/platillos/yuca.jpg",
                   Etiquetas = new List<string>
                   {
                      "Tradicional"
                   }
                },

                new FoodCardViewModel
                {
                 Id = 5,
                 Nombre = "Tres Leches",
                 Categoria = "Postre",
                  Descripcion = "Pastel artesanal bañado en tres leches.",
                  Precio = 4.75m,
                  Imagen = "/images/platillos/tresleches.jpg",
                    Etiquetas = new List<string>
                 {
                      "Postre"
                 }
                },
            };
        }
    }
}