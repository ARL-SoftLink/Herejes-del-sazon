using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace herejes_del_sazon.Models;

public partial class MyDBContext : DbContext
{
    public MyDBContext(DbContextOptions<MyDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categoria> Categorias { get; set; }

    public virtual DbSet<DetallePedido> DetallePedidos { get; set; }

    public virtual DbSet<Favorito> Favoritos { get; set; }

    public virtual DbSet<FlywaySchemaHistory> FlywaySchemaHistories { get; set; }

    public virtual DbSet<Ingrediente> Ingredientes { get; set; }

    public virtual DbSet<Mesa> Mesas { get; set; }

    public virtual DbSet<Pedido> Pedidos { get; set; }

    public virtual DbSet<PerfilesFamiliare> PerfilesFamiliares { get; set; }

    public virtual DbSet<Platillo> Platillos { get; set; }

    public virtual DbSet<Reservacione> Reservaciones { get; set; }

    public virtual DbSet<Reseña> Reseñas { get; set; }

    public virtual DbSet<SeguimientoPedido> SeguimientoPedidos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.IdCategoria);

            entity.ToTable("CATEGORIAS");

            entity.Property(e => e.IdCategoria).HasColumnName("ID_CATEGORIA");
            entity.Property(e => e.NombreCategoria)
                .HasMaxLength(50)
                .HasColumnName("NOMBRE_CATEGORIA");
        });

        modelBuilder.Entity<DetallePedido>(entity =>
        {
            entity.HasKey(e => e.IdDetalle);

            entity.ToTable("DETALLE_PEDIDO");

            entity.HasIndex(e => e.IdPedido, "IDX_DETALLE_PEDIDO_ID_PEDIDO");

            entity.HasIndex(e => e.IdPlatillo, "IDX_DETALLE_PEDIDO_ID_PLATILLO");

            entity.Property(e => e.IdDetalle).HasColumnName("ID_DETALLE");
            entity.Property(e => e.Cantidad).HasColumnName("CANTIDAD");
            entity.Property(e => e.IdPedido).HasColumnName("ID_PEDIDO");
            entity.Property(e => e.IdPlatillo).HasColumnName("ID_PLATILLO");
            entity.Property(e => e.PrecioUnitario)
                .HasPrecision(10, 2)
                .HasColumnName("PRECIO_UNITARIO");
            entity.Property(e => e.Subtotal)
                .HasPrecision(10, 5)
                .HasColumnName("SUBTOTAL");

            entity.HasOne(d => d.IdPedidoNavigation).WithMany(p => p.DetallePedidos)
                .HasForeignKey(d => d.IdPedido)
                .HasConstraintName("FK_DETALLE_PEDIDO_PEDIDOS");

            entity.HasOne(d => d.IdPlatilloNavigation).WithMany(p => p.DetallePedidos)
                .HasForeignKey(d => d.IdPlatillo)
                .HasConstraintName("FK_DETALLE_PEDIDO_PLATILLOS");
        });

        modelBuilder.Entity<Favorito>(entity =>
        {
            entity.HasKey(e => e.IdFavoritos);

            entity.ToTable("FAVORITOS");

            entity.HasIndex(e => e.IdPlatillo, "IDX_FAVORITOS_ID_PLATILLO");

            entity.HasIndex(e => e.IdUsuario, "IDX_FAVORITOS_ID_USUARIO");

            entity.Property(e => e.IdFavoritos).HasComment("Clave primaria añadida según actualización registrada en el diagrama original (\"se ha añadido la fila IdFavoritos\").");
            entity.Property(e => e.FechaAgregado)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("FECHA_AGREGADO");
            entity.Property(e => e.IdPlatillo).HasColumnName("ID_PLATILLO");
            entity.Property(e => e.IdUsuario).HasColumnName("ID_USUARIO");

            entity.HasOne(d => d.IdPlatilloNavigation).WithMany(p => p.Favoritos)
                .HasForeignKey(d => d.IdPlatillo)
                .HasConstraintName("FK_FAVORITOS_PLATILLOS");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Favoritos)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK_FAVORITOS_USUARIOS");
        });

        modelBuilder.Entity<FlywaySchemaHistory>(entity =>
        {
            entity.HasKey(e => e.InstalledRank).HasName("flyway_schema_history_pk");

            entity.ToTable("flyway_schema_history");

            entity.HasIndex(e => e.Success, "flyway_schema_history_s_idx");

            entity.Property(e => e.InstalledRank)
                .ValueGeneratedNever()
                .HasColumnName("installed_rank");
            entity.Property(e => e.Checksum).HasColumnName("checksum");
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .HasColumnName("description");
            entity.Property(e => e.ExecutionTime).HasColumnName("execution_time");
            entity.Property(e => e.InstalledBy)
                .HasMaxLength(100)
                .HasColumnName("installed_by");
            entity.Property(e => e.InstalledOn)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("installed_on");
            entity.Property(e => e.Script)
                .HasMaxLength(1000)
                .HasColumnName("script");
            entity.Property(e => e.Success).HasColumnName("success");
            entity.Property(e => e.Type)
                .HasMaxLength(20)
                .HasColumnName("type");
            entity.Property(e => e.Version)
                .HasMaxLength(50)
                .HasColumnName("version");
        });

        modelBuilder.Entity<Ingrediente>(entity =>
        {
            entity.HasKey(e => e.IdIngredientes);

            entity.ToTable("INGREDIENTES");

            entity.Property(e => e.IdIngredientes).HasColumnName("ID_INGREDIENTES");
            entity.Property(e => e.Descripcion).HasColumnName("DESCRIPCION");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("NOMBRE");
            entity.Property(e => e.Procedencia)
                .HasMaxLength(100)
                .HasColumnName("PROCEDENCIA");
        });

        modelBuilder.Entity<Mesa>(entity =>
        {
            entity.HasKey(e => e.IdMesa).HasName("PK_MESAS");

            entity.ToTable(tb => tb.HasComment("Tabla añadida en lugar de agregar columnas de mesa (número de mesa / IdMesa) directamente en RESERVACIONES, según decisión documentada en el diagrama fuente."));

            entity.Property(e => e.TipoMesa).HasMaxLength(50);
        });

        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.HasKey(e => e.IdPedido);

            entity.ToTable("PEDIDOS", tb => tb.HasComment("El campo ESTADO fue retirado de esta tabla por estar duplicado con SEGUIMIENTO_PEDIDO; se conserva únicamente en esta última, según actualización documentada en el diagrama fuente."));

            entity.HasIndex(e => e.IdUsuario, "IDX_PEDIDOS_ID_USUARIO");

            entity.Property(e => e.IdPedido).HasColumnName("ID_PEDIDO");
            entity.Property(e => e.DireccionEntrega).HasColumnName("DIRECCION_ENTREGA");
            entity.Property(e => e.FechaPedido)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("FECHA_PEDIDO");
            entity.Property(e => e.IdUsuario).HasColumnName("ID_USUARIO");
            entity.Property(e => e.Total)
                .HasPrecision(10, 2)
                .HasColumnName("TOTAL");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK_PEDIDOS_USUARIOS");
        });

        modelBuilder.Entity<PerfilesFamiliare>(entity =>
        {
            entity.HasKey(e => e.IdPerfil);

            entity.ToTable("PERFILES_FAMILIARES");

            entity.Property(e => e.IdPerfil).HasColumnName("ID_PERFIL");
            entity.Property(e => e.Descripcion).HasColumnName("DESCRIPCION");
            entity.Property(e => e.NombrePerfil)
                .HasMaxLength(50)
                .HasColumnName("NOMBRE_PERFIL");
        });

        modelBuilder.Entity<Platillo>(entity =>
        {
            entity.HasKey(e => e.IdPlatillo);

            entity.ToTable("PLATILLOS");

            entity.HasIndex(e => e.IdCategoria, "IDX_PLATILLOS_ID_CATEGORIA");

            entity.Property(e => e.IdPlatillo).HasColumnName("ID_PLATILLO");
            entity.Property(e => e.Descripcion).HasColumnName("DESCRIPCION");
            entity.Property(e => e.Disponible).HasColumnName("DISPONIBLE");
            entity.Property(e => e.IdCategoria).HasColumnName("ID_CATEGORIA");
            entity.Property(e => e.ImagenUrl)
                .HasMaxLength(255)
                .HasColumnName("IMAGEN_URL");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("NOMBRE");
            entity.Property(e => e.Precio)
                .HasPrecision(10, 2)
                .HasColumnName("PRECIO");

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Platillos)
                .HasForeignKey(d => d.IdCategoria)
                .HasConstraintName("FK_PLATILLOS_CATEGORIAS");

            entity.HasMany(d => d.IdIngredientes).WithMany(p => p.IdPlatillos)
                .UsingEntity<Dictionary<string, object>>(
                    "PlatilloIngrediente",
                    r => r.HasOne<Ingrediente>().WithMany()
                        .HasForeignKey("IdIngrediente")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_PLATILLO_INGREDIENTE_INGREDIENTES"),
                    l => l.HasOne<Platillo>().WithMany()
                        .HasForeignKey("IdPlatillo")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_PLATILLO_INGREDIENTE_PLATILLOS"),
                    j =>
                    {
                        j.HasKey("IdPlatillo", "IdIngrediente");
                        j.ToTable("PLATILLO_INGREDIENTE");
                        j.HasIndex(new[] { "IdIngrediente" }, "IDX_PLATILLO_INGREDIENTE_ID_INGREDIENTE");
                        j.IndexerProperty<int>("IdPlatillo").HasColumnName("ID_PLATILLO");
                        j.IndexerProperty<int>("IdIngrediente").HasColumnName("ID_INGREDIENTE");
                    });

            entity.HasMany(d => d.IdPerfils).WithMany(p => p.IdPlatillos)
                .UsingEntity<Dictionary<string, object>>(
                    "PlatilloPerfil",
                    r => r.HasOne<PerfilesFamiliare>().WithMany()
                        .HasForeignKey("IdPerfil")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_PLATILLO_PERFIL_PERFILES"),
                    l => l.HasOne<Platillo>().WithMany()
                        .HasForeignKey("IdPlatillo")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_PLATILLO_PERFIL_PLATILLOS"),
                    j =>
                    {
                        j.HasKey("IdPlatillo", "IdPerfil");
                        j.ToTable("PLATILLO_PERFIL");
                        j.HasIndex(new[] { "IdPerfil" }, "IDX_PLATILLO_PERFIL_ID_PERFIL");
                        j.IndexerProperty<int>("IdPlatillo").HasColumnName("ID_PLATILLO");
                        j.IndexerProperty<int>("IdPerfil").HasColumnName("ID_PERFIL");
                    });
        });

        modelBuilder.Entity<Reservacione>(entity =>
        {
            entity.HasKey(e => e.IdReserva);

            entity.ToTable("RESERVACIONES");

            entity.HasIndex(e => e.IdMesa, "IDX_RESERVACIONES_IDMESA");

            entity.HasIndex(e => e.IdUsuario, "IDX_RESERVACIONES_ID_USUARIO");

            entity.Property(e => e.IdReserva).HasColumnName("ID_RESERVA");
            entity.Property(e => e.Estado)
                .HasMaxLength(30)
                .HasColumnName("ESTADO");
            entity.Property(e => e.FechaCreacion)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("FECHA_CREACION");
            entity.Property(e => e.FechaReserva).HasColumnName("FECHA_RESERVA");
            entity.Property(e => e.HoraReserva).HasColumnName("HORA_RESERVA");
            entity.Property(e => e.IdUsuario).HasColumnName("ID_USUARIO");
            entity.Property(e => e.NumeroComensales).HasColumnName("NUMERO_COMENSALES");

            entity.HasOne(d => d.IdMesaNavigation).WithMany(p => p.Reservaciones)
                .HasForeignKey(d => d.IdMesa)
                .HasConstraintName("FK_RESERVACIONES_MESAS");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Reservaciones)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK_RESERVACIONES_USUARIOS");
        });

        modelBuilder.Entity<Reseña>(entity =>
        {
            entity.HasKey(e => e.IdReseña);

            entity.ToTable("RESEÑAS");

            entity.HasIndex(e => e.IdPlatillo, "IDX_RESEÑAS_ID_PLATILLO");

            entity.HasIndex(e => e.IdUsuario, "IDX_RESEÑAS_ID_USUARIO");

            entity.Property(e => e.IdReseña).HasColumnName("ID_RESEÑA");
            entity.Property(e => e.Calificacion).HasColumnName("CALIFICACION");
            entity.Property(e => e.Coamntario).HasColumnName("COAMNTARIO");
            entity.Property(e => e.FechaReseña)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("FECHA_RESEÑA");
            entity.Property(e => e.IdPlatillo).HasColumnName("ID_PLATILLO");
            entity.Property(e => e.IdUsuario).HasColumnName("ID_USUARIO");

            entity.HasOne(d => d.IdPlatilloNavigation).WithMany(p => p.Reseñas)
                .HasForeignKey(d => d.IdPlatillo)
                .HasConstraintName("FK_RESEÑAS_PLATILLOS");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Reseñas)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK_RESEÑAS_USUARIOS");
        });

        modelBuilder.Entity<SeguimientoPedido>(entity =>
        {
            entity.HasKey(e => e.IdSeguimiento);

            entity.ToTable("SEGUIMIENTO_PEDIDO");

            entity.HasIndex(e => e.IdPedido, "IDX_SEGUIMIENTO_PEDIDO_ID_PEDIDO");

            entity.Property(e => e.IdSeguimiento).HasColumnName("ID_SEGUIMIENTO");
            entity.Property(e => e.Estado)
                .HasMaxLength(30)
                .HasColumnName("ESTADO");
            entity.Property(e => e.FechaHora)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("FECHA_HORA");
            entity.Property(e => e.IdPedido).HasColumnName("ID_PEDIDO");
            entity.Property(e => e.Obsrvacion).HasColumnName("OBSRVACION");

            entity.HasOne(d => d.IdPedidoNavigation).WithMany(p => p.SeguimientoPedidos)
                .HasForeignKey(d => d.IdPedido)
                .HasConstraintName("FK_SEGUIMIENTO_PEDIDO_PEDIDOS");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario);

            entity.ToTable("USUARIOS");

            entity.HasIndex(e => e.Correo, "UQ_USUARIOS_CORREO").IsUnique();

            entity.Property(e => e.IdUsuario).HasColumnName("ID_USUARIO");
            entity.Property(e => e.Activo).HasColumnName("ACTIVO");
            entity.Property(e => e.Apellido)
                .HasMaxLength(100)
                .HasColumnName("APELLIDO");
            entity.Property(e => e.Contraseña)
                .HasMaxLength(100)
                .HasColumnName("CONTRASEÑA");
            entity.Property(e => e.Correo)
                .HasMaxLength(150)
                .HasColumnName("CORREO");
            entity.Property(e => e.FechaRegistro)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("FECHA_REGISTRO");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(200)
                .HasComment("Columna añadida según nota del diagrama para permitir foto de perfil. El campo \"RolID\" para un futuro sistema de roles fue evaluado pero, según la última actualización registrada en el diagrama, aún no se implementa.")
                .HasColumnName("ImageURL");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("NOMBRE");
            entity.Property(e => e.Telefono)
                .HasMaxLength(100)
                .HasColumnName("TELEFONO");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
