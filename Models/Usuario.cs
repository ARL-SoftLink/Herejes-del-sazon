using System;
using System.Collections.Generic;

namespace herejes_del_sazon.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string? Nombre { get; set; }

    public string? Apellido { get; set; }

    public string? Correo { get; set; }

    public string? Contraseña { get; set; }

    public string? Telefono { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public bool? Activo { get; set; }

    /// <summary>
    /// Columna añadida según nota del diagrama para permitir foto de perfil. El campo &quot;RolID&quot; para un futuro sistema de roles fue evaluado pero, según la última actualización registrada en el diagrama, aún no se implementa.
    /// </summary>
    public string? ImageUrl { get; set; }

    public virtual ICollection<Favorito> Favoritos { get; set; } = new List<Favorito>();

    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();

    public virtual ICollection<Reservacione> Reservaciones { get; set; } = new List<Reservacione>();

    public virtual ICollection<Reseña> Reseñas { get; set; } = new List<Reseña>();
}
