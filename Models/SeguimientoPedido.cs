using System;
using System.Collections.Generic;

namespace herejes_del_sazon.Models;

public partial class SeguimientoPedido
{
    public int IdSeguimiento { get; set; }

    public int? IdPedido { get; set; }

    public string? Estado { get; set; }

    public DateTime? FechaHora { get; set; }

    public string? Obsrvacion { get; set; }

    public virtual Pedido? IdPedidoNavigation { get; set; }
}
