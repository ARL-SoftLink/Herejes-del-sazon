using System;
using System.Collections.Generic;

namespace herejes_del_sazon.Models;

public partial class Categoria
{
    public int IdCategoria { get; set; }

    public string? NombreCategoria { get; set; }

    public virtual ICollection<Platillo> Platillos { get; set; } = new List<Platillo>();
}
