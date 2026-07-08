using System;
using System.Collections.Generic;

namespace herejes_del_sazon.Models;

/// <summary>
/// Tabla añadida en lugar de agregar columnas de mesa (número de mesa / IdMesa) directamente en RESERVACIONES, según decisión documentada en el diagrama fuente.
/// </summary>
public partial class Mesa
{
    public int IdMesa { get; set; }

    public int? NumeroMesa { get; set; }

    public int? Capacidad { get; set; }

    public string? TipoMesa { get; set; }

    public virtual ICollection<Reservacione> Reservaciones { get; set; } = new List<Reservacione>();
}
