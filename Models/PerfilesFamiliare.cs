using System;
using System.Collections.Generic;

namespace herejes_del_sazon.Models;

public partial class PerfilesFamiliare
{
    public int IdPerfil { get; set; }

    public string? NombrePerfil { get; set; }

    public string? Descripcion { get; set; }

    public virtual ICollection<Platillo> IdPlatillos { get; set; } = new List<Platillo>();
}
