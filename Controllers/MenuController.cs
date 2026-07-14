using herejes_del_sazon.Models.ViewModels;
using herejes_del_sazon.Services;
using Microsoft.AspNetCore.Mvc;

namespace herejes_del_sazon.Controllers
{
    public class MenuController : Controller
    {
        private readonly MenuService _menuService;

        public MenuController(MenuService menuService)
        {
            _menuService = menuService;
        }

        public IActionResult Index()
        {
            var model = new MenuViewModel
            {
                Dishes = _menuService.GetAllDishes()
            };

            return View(model);
        }
    }
}