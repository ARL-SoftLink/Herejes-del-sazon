using System;
using System.Collections.Generic;

namespace herejes_del_sazon.Models;

/// <summary>
/// El campo ESTADO fue retirado de esta tabla por estar duplicado con SEGUIMIENTO_PEDIDO; se conserva únicamente en esta última, según actualización documentada en el diagrama fuente.
/// </summary>
public partial class Pedido
{
    public int IdPedido { get; set; }

    public int? IdUsuario { get; set; }

    public DateTime? FechaPedido { get; set; }

    public string? DireccionEntrega { get; set; }

    public decimal? Total { get; set; }

    public virtual ICollection<DetallePedido> DetallePedidos { get; set; } = new List<DetallePedido>();

    public virtual Usuario? IdUsuarioNavigation { get; set; }

    public virtual ICollection<SeguimientoPedido> SeguimientoPedidos { get; set; } = new List<SeguimientoPedido>();
}
