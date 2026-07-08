using System;
using System.Collections.Generic;

namespace herejes_del_sazon.Models;

public partial class Favorito
{
    /// <summary>
    /// Clave primaria añadida según actualización registrada en el diagrama original (&quot;se ha añadido la fila IdFavoritos&quot;).
    /// </summary>
    public int IdFavoritos { get; set; }

    public int? IdUsuario { get; set; }

    public int? IdPlatillo { get; set; }

    public DateTime? FechaAgregado { get; set; }

    public virtual Platillo? IdPlatilloNavigation { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }
}
