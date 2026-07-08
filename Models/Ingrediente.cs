using System;
using System.Collections.Generic;

namespace herejes_del_sazon.Models;

public partial class Ingrediente
{
    public int IdIngredientes { get; set; }

    public string? Nombre { get; set; }

    public string? Descripcion { get; set; }

    public string? Procedencia { get; set; }

    public virtual ICollection<Platillo> IdPlatillos { get; set; } = new List<Platillo>();
}
