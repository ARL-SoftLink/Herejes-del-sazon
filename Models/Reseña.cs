using System;
using System.Collections.Generic;

namespace herejes_del_sazon.Models;

public partial class Reseña
{
    public int IdReseña { get; set; }

    public int? IdUsuario { get; set; }

    public int? IdPlatillo { get; set; }

    public string? Calificacion { get; set; }

    public string? Coamntario { get; set; }

    public DateTime? FechaReseña { get; set; }

    public virtual Platillo? IdPlatilloNavigation { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }
}
