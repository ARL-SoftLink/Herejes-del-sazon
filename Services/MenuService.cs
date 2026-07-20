using herejes_del_sazon.Models.ViewModels;
using herejes_del_sazon.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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

            Etiquetas = new List<string>(),

            Ingredientes = platillo.IdIngredientes
              .Select(i => i.Nombre ?? "")
              .Where(nombre => !string.IsNullOrWhiteSpace(nombre))
                .ToList()
           };
        }

        private DishDetailViewModel MapToDishDetail(Platillo platillo)
        {
         return new DishDetailViewModel
         {
             Id = platillo.IdPlatillo,

             Nombre = platillo.Nombre ?? "Platillo sin nombre",

             Descripcion = platillo.Descripcion ?? "Sin descripción",

             Precio = platillo.Precio ?? 0,

             Imagen = platillo.ImagenUrl ?? "/images/platillos/default.jpg",

             Categoria = platillo.IdCategoriaNavigation?.NombreCategoria ?? "Sin categoría",

             Etiquetas = new List<string>(),

             Ingredientes = platillo.IdIngredientes
                 .Select(i => i.Nombre ?? "")
                 .Where(nombre => !string.IsNullOrWhiteSpace(nombre))
                 .ToList(),

             PerfilesFamiliares = platillo.IdPerfils
               .Select(p => p.NombrePerfil ?? "")
               .Where(nombre => !string.IsNullOrWhiteSpace(nombre))
               .ToList(),   

               Disponible = platillo.Disponible ?? false,
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


        public DishDetailViewModel? GetDishById(int id)
        {
          var platillo = _context.Platillos
             .Include(p => p.IdCategoriaNavigation)
             .Include(p => p.IdIngredientes)
             .FirstOrDefault(p => p.IdPlatillo == id);

          if (platillo != null)
          {
               return MapToDishDetail(platillo);
          }

            // Mientras la BD esté vacía usamos los datos simulados
          var mock = GetMockAllDishes()
             .FirstOrDefault(x => x.Id == id);

          if (mock == null)
              return null;

          return new DishDetailViewModel
         {
            Id = mock.Id,
            Nombre = mock.Nombre,
            Categoria = mock.Categoria,
            Descripcion = mock.Descripcion,
            Precio = mock.Precio,
            Imagen = mock.Imagen,
            Etiquetas = mock.Etiquetas,
            Ingredientes = new List<string>(),
            Disponible = true,
         };
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

        public List<string> GetCategories()
        {
         var categorias = _context.Categorias
            .Select(c => c.NombreCategoria!)
            .Distinct()
            .OrderBy(c => c)
            .ToList();

         if (categorias.Any())
          return categorias;

          // Datos Mock si la BD aún está vacía
          return new List<string>
          {
           "Especial",
            "Bebida"
           };
       }
    }
}