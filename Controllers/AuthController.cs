using herejes_del_sazon.Models.ViewModels;
using herejes_del_sazon.Services;
using Microsoft.AspNetCore.Mvc;

namespace herejes_del_sazon.Controllers
{
    public class AuthController : Controller
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        public IActionResult Login()
        {
          return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
           if (!ModelState.IsValid)
            return View(model);

           var usuario = _authService.Login(model);

           if (usuario == null)
           {
              ModelState.AddModelError("", "Correo o contraseña incorrectos.");
              return View(model);
           }

            HttpContext.Session.SetInt32("IdUsuario", usuario.IdUsuario);
            HttpContext.Session.SetString("Nombre", usuario.Nombre ?? "");
            HttpContext.Session.SetString("Correo", usuario.Correo ?? "");

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
         return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
          if (!ModelState.IsValid)
             return View(model);

           bool registrado = _authService.Register(model);

          if (!registrado)
          {
             ModelState.AddModelError("", "Ese correo ya está registrado.");
             return View(model);
          }

           return RedirectToAction(nameof(Login));
        }

        public IActionResult Logout()
        {
           HttpContext.Session.Clear();

             return RedirectToAction("Index", "Home");
        }
    }
}