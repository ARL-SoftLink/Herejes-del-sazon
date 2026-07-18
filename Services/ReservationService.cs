using herejes_del_sazon.Models;
using herejes_del_sazon.Models.ViewModels;

namespace herejes_del_sazon.Services
{
    public class ReservationService
    {
        private readonly MyDBContext _context;

        public ReservationService(MyDBContext context)
        {
            _context = context;
        }

        // ===========================
        // MÉTODOS PÚBLICOS
        // ===========================

        public List<ReservationViewModel> GetReservations()
        {
            return new List<ReservationViewModel>();
        }

        public ReservationViewModel? GetReservationById(int id)
        {
            return null;
        }

        public bool CreateReservation(ReservationViewModel reservation)
        {
            return true;
        }

        public List<TableCardViewModel> GetAvailableTables()
        {
          var mesasBD = _context.Mesas
            .Select(m => new TableCardViewModel
            {
                 Id = m.IdMesa,
                 Nombre = $"Mesa {m.NumeroMesa}",
                 Capacidad = m.Capacidad ?? 0,
                 TipoMesa = m.TipoMesa ?? "General",
                  Disponible = true
            })
              .OrderBy(m => m.Id)
              .ToList();

           if (mesasBD.Any())
           {
              return mesasBD;
           }

            return GetMockTables();
        }


        //datos de prueba

        private List<TableCardViewModel> GetMockTables()
        {
           return new List<TableCardViewModel>
           {
               new TableCardViewModel
               {
              Id = 1,
              Nombre = "Mesa 1",
              Capacidad = 4,
              TipoMesa = "Interior",
               Disponible = true
              },
              new TableCardViewModel
              {
               Id = 2,
               Nombre = "Mesa 2",
               Capacidad = 6,
               TipoMesa = "Terraza",
               Disponible = true
              },
              new TableCardViewModel
              {
               Id = 3,
              Nombre = "Mesa 3",
               Capacidad = 8,
               TipoMesa = "Familiar",
              Disponible = true
              }
            };
        }
    }
}