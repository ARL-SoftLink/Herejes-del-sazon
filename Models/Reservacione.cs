using System;
using System.Collections.Generic;

namespace herejes_del_sazon.Models;

public partial class Reservacione
{
    public int IdReserva { get; set; }

    public int? IdUsuario { get; set; }

    public DateOnly? FechaReserva { get; set; }

    public TimeOnly? HoraReserva { get; set; }

    public int? NumeroComensales { get; set; }

    public string? Estado { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public int? IdMesa { get; set; }

    public virtual Mesa? IdMesaNavigation { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }
}
