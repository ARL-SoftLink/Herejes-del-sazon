using herejes_del_sazon.Models;
using herejes_del_sazon.Models.ViewModels;

namespace herejes_del_sazon.Services
{
    public class AuthService
    {
        private readonly MyDBContext _context;

        public AuthService(MyDBContext context)
        {
            _context = context;
        }

        public Usuario? Login(LoginViewModel model)
        {
           return _context.Usuarios
               .FirstOrDefault(x =>
                 x.Correo == model.Correo &&
                 x.Contraseña == model.Contraseña &&
                 x.Activo == true);
        }

        public bool Register(RegisterViewModel model)
        {
          if (_context.Usuarios.Any(x => x.Correo == model.Correo))
            return false;

          var usuario = new Usuario
          {
             Nombre = model.Nombre,
             Apellido = model.Apellido,
             Correo = model.Correo,
             Contraseña = model.Contraseña,
              Telefono = model.Telefono,
             FechaRegistro = DateTime.Now,
             Activo = true
            };

            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

         return true;
        }
    }
}