using herejes_del_sazon.Models.ViewModels;
using herejes_del_sazon.Services;
using Microsoft.AspNetCore.Mvc;

namespace herejes_del_sazon.Controllers
{
    public class ReservationController : Controller
    {
        private readonly ReservationService _reservationService;

        public ReservationController(ReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        public IActionResult Index()
        {
          var model = new ReservationViewModel
          {
             Mesas = _reservationService.GetAvailableTables()
          };

          return View(model);
        }

       

        public IActionResult Create(int idMesa)
        {
            var model = new ReservationViewModel
            {
                IdMesa = idMesa,
                Fecha = DateTime.Today
            };

               return View(model);
        }

        [HttpPost]
        public IActionResult Create(ReservationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                  model.Mesas = _reservationService.GetAvailableTables();

                  return View("Index", model);
            }

            bool resultado = _reservationService.CreateReservation(model);

            if (!resultado)
            {
                model.Mesas = _reservationService.GetAvailableTables();

                ModelState.AddModelError("", "No fue posible realizar la reservación.");

                 return View("Index", model);
            }

            TempData["Success"] = "¡Reservación realizada correctamente!";

            return RedirectToAction(nameof(Index));
        }
    }
}