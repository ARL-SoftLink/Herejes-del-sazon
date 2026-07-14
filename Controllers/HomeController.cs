using herejes_del_sazon.Models;
using herejes_del_sazon.Models.ViewModels;
using herejes_del_sazon.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace herejes_del_sazon.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MenuService _menuService;

        public HomeController(
            ILogger<HomeController> logger,
            MenuService menuService)
        {
            _logger = logger;
            _menuService = menuService;
        }

        public IActionResult Index()
        {
            var model = new HomeViewModel
            {
                SpecialDishes = _menuService.GetFeaturedDishes()
            };

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}