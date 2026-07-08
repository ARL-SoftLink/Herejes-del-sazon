using System;
using System.Collections.Generic;

namespace herejes_del_sazon.Models;

public partial class Platillo
{
    public int IdPlatillo { get; set; }

    public string? Nombre { get; set; }

    public string? Descripcion { get; set; }

    public decimal? Precio { get; set; }

    public string? ImagenUrl { get; set; }

    public int? IdCategoria { get; set; }

    public bool? Disponible { get; set; }

    public virtual ICollection<DetallePedido> DetallePedidos { get; set; } = new List<DetallePedido>();

    public virtual ICollection<Favorito> Favoritos { get; set; } = new List<Favorito>();

    public virtual Categoria? IdCategoriaNavigation { get; set; }

    public virtual ICollection<Reseña> Reseñas { get; set; } = new List<Reseña>();

    public virtual ICollection<Ingrediente> IdIngredientes { get; set; } = new List<Ingrediente>();

    public virtual ICollection<PerfilesFamiliare> IdPerfils { get; set; } = new List<PerfilesFamiliare>();
}
