using System;
using System.Collections.Generic;

namespace herejes_del_sazon.Models;

public partial class DetallePedido
{
    public int IdDetalle { get; set; }

    public int? IdPedido { get; set; }

    public int? IdPlatillo { get; set; }

    public int? Cantidad { get; set; }

    public decimal? PrecioUnitario { get; set; }

    public decimal? Subtotal { get; set; }

    public virtual Pedido? IdPedidoNavigation { get; set; }

    public virtual Platillo? IdPlatilloNavigation { get; set; }
}
