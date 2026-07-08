-- ============================================================================
-- PROYECTO: Herejes del Sazón (ARL-SoftLink/Herejes-del-sazon)
-- SCRIPT:   Creación del esquema de base de datos
-- MOTOR:    PostgreSQL 17
-- ORIGEN:   Diagrama entidad-relación (ARL_HerejesSazon_DER_drawio.png)
-- ============================================================================
-- NOTA DE DISEÑO:
-- Este script preserva los nombres de tablas y columnas tal como aparecen
-- en el diagrama original, incluyendo mayúsculas/minúsculas mixtas
-- (p. ej. "Mesas", "IdMesa", "ImageURL", "IdFavoritos") y caracteres con
-- diacrítico ("Ñ"). Por esa razón, TODOS los identificadores se declaran
-- entre comillas dobles: PostgreSQL pliega a minúsculas cualquier
-- identificador no delimitado, y el plegado de caracteres no ASCII depende
-- del locale del servidor. El uso de comillas dobles garantiza que el
-- nombre se preserve exactamente, independientemente del locale.
--
-- IMPLICACIÓN PRÁCTICA: cualquier consulta posterior (psql, migraciones,
-- configuración de Entity Framework Core / Npgsql, etc.) deberá referenciar
-- estos identificadores exactamente con las mismas comillas y capitalización.
-- ============================================================================

-- ============================================================================
-- BLOQUE OPCIONAL DE LIMPIEZA (comentado por defecto)
-- Descomentar únicamente si se desea eliminar el esquema existente antes de
-- recrearlo (útil en entornos de desarrollo con reconstrucciones frecuentes
-- vía Docker Compose).
-- ============================================================================
-- DROP TABLE IF EXISTS "PLATILLO_PERFIL" CASCADE;
-- DROP TABLE IF EXISTS "RESEÑAS" CASCADE;
-- DROP TABLE IF EXISTS "PLATILLO_INGREDIENTE" CASCADE;
-- DROP TABLE IF EXISTS "DETALLE_PEDIDO" CASCADE;
-- DROP TABLE IF EXISTS "SEGUIMIENTO_PEDIDO" CASCADE;
-- DROP TABLE IF EXISTS "PEDIDOS" CASCADE;
-- DROP TABLE IF EXISTS "FAVORITOS" CASCADE;
-- DROP TABLE IF EXISTS "RESERVACIONES" CASCADE;
-- DROP TABLE IF EXISTS "PLATILLOS" CASCADE;
-- DROP TABLE IF EXISTS "INGREDIENTES" CASCADE;
-- DROP TABLE IF EXISTS "PERFILES_FAMILIARES" CASCADE;
-- DROP TABLE IF EXISTS "CATEGORIAS" CASCADE;
-- DROP TABLE IF EXISTS "Mesas" CASCADE;
-- DROP TABLE IF EXISTS "USUARIOS" CASCADE;

SET client_encoding = 'UTF8';

BEGIN;

-- ----------------------------------------------------------------------------
-- Tabla: USUARIOS
-- ----------------------------------------------------------------------------
CREATE TABLE "USUARIOS" (
    "ID_USUARIO"      SERIAL,
    "NOMBRE"          VARCHAR(100),
    "APELLIDO"        VARCHAR(100),
    "CORREO"          VARCHAR(150),
    "CONTRASEÑA"      VARCHAR(100),
    "TELEFONO"        VARCHAR(100),
    "FECHA_REGISTRO"  TIMESTAMP,
    "ACTIVO"          BOOLEAN,
    "ImageURL"        VARCHAR(200),
    CONSTRAINT "PK_USUARIOS" PRIMARY KEY ("ID_USUARIO"),
    CONSTRAINT "UQ_USUARIOS_CORREO" UNIQUE ("CORREO")
);

COMMENT ON COLUMN "USUARIOS"."ImageURL" IS
    'Columna añadida según nota del diagrama para permitir foto de perfil. El campo "RolID" para un futuro sistema de roles fue evaluado pero, según la última actualización registrada en el diagrama, aún no se implementa.';

-- ----------------------------------------------------------------------------
-- Tabla: Mesas
-- ----------------------------------------------------------------------------
CREATE TABLE "Mesas" (
    "IdMesa"      SERIAL,
    "NumeroMesa"  INTEGER,
    "Capacidad"   INTEGER,
    "TipoMesa"    VARCHAR(50),
    CONSTRAINT "PK_MESAS" PRIMARY KEY ("IdMesa")
);

COMMENT ON TABLE "Mesas" IS
    'Tabla añadida en lugar de agregar columnas de mesa (número de mesa / IdMesa) directamente en RESERVACIONES, según decisión documentada en el diagrama fuente.';

-- ----------------------------------------------------------------------------
-- Tabla: CATEGORIAS
-- ----------------------------------------------------------------------------
CREATE TABLE "CATEGORIAS" (
    "ID_CATEGORIA"      SERIAL,
    "NOMBRE_CATEGORIA"  VARCHAR(50),
    CONSTRAINT "PK_CATEGORIAS" PRIMARY KEY ("ID_CATEGORIA")
);

-- ----------------------------------------------------------------------------
-- Tabla: PERFILES_FAMILIARES
-- ----------------------------------------------------------------------------
CREATE TABLE "PERFILES_FAMILIARES" (
    "ID_PERFIL"      SERIAL,
    "NOMBRE_PERFIL"  VARCHAR(50),
    "DESCRIPCION"    TEXT,
    CONSTRAINT "PK_PERFILES_FAMILIARES" PRIMARY KEY ("ID_PERFIL")
);

-- ----------------------------------------------------------------------------
-- Tabla: INGREDIENTES
-- ----------------------------------------------------------------------------
CREATE TABLE "INGREDIENTES" (
    "ID_INGREDIENTES"  SERIAL,
    "NOMBRE"           VARCHAR(50),
    "DESCRIPCION"      TEXT,
    "PROCEDENCIA"      VARCHAR(100),
    CONSTRAINT "PK_INGREDIENTES" PRIMARY KEY ("ID_INGREDIENTES")
);

-- ----------------------------------------------------------------------------
-- Tabla: PLATILLOS
-- ----------------------------------------------------------------------------
CREATE TABLE "PLATILLOS" (
    "ID_PLATILLO"   SERIAL,
    "NOMBRE"        VARCHAR(100),
    "DESCRIPCION"   TEXT,
    "PRECIO"        NUMERIC(10,2),
    "IMAGEN_URL"    VARCHAR(255),
    "ID_CATEGORIA"  INTEGER,
    "DISPONIBLE"    BOOLEAN,
    CONSTRAINT "PK_PLATILLOS" PRIMARY KEY ("ID_PLATILLO"),
    CONSTRAINT "FK_PLATILLOS_CATEGORIAS" FOREIGN KEY ("ID_CATEGORIA")
        REFERENCES "CATEGORIAS" ("ID_CATEGORIA")
);

-- ----------------------------------------------------------------------------
-- Tabla: RESERVACIONES
-- ----------------------------------------------------------------------------
CREATE TABLE "RESERVACIONES" (
    "ID_RESERVA"         SERIAL,
    "ID_USUARIO"         INTEGER,
    "FECHA_RESERVA"      DATE,
    "HORA_RESERVA"       TIME,
    "NUMERO_COMENSALES"  INTEGER,
    "ESTADO"             VARCHAR(30),
    "FECHA_CREACION"     TIMESTAMP,
    "IdMesa"             INTEGER,
    CONSTRAINT "PK_RESERVACIONES" PRIMARY KEY ("ID_RESERVA"),
    CONSTRAINT "FK_RESERVACIONES_USUARIOS" FOREIGN KEY ("ID_USUARIO")
        REFERENCES "USUARIOS" ("ID_USUARIO"),
    CONSTRAINT "FK_RESERVACIONES_MESAS" FOREIGN KEY ("IdMesa")
        REFERENCES "Mesas" ("IdMesa")
);

-- ----------------------------------------------------------------------------
-- Tabla: FAVORITOS
-- ----------------------------------------------------------------------------
CREATE TABLE "FAVORITOS" (
    "IdFavoritos"     SERIAL,
    "ID_USUARIO"      INTEGER,
    "ID_PLATILLO"     INTEGER,
    "FECHA_AGREGADO"  TIMESTAMP,
    CONSTRAINT "PK_FAVORITOS" PRIMARY KEY ("IdFavoritos"),
    CONSTRAINT "FK_FAVORITOS_USUARIOS" FOREIGN KEY ("ID_USUARIO")
        REFERENCES "USUARIOS" ("ID_USUARIO"),
    CONSTRAINT "FK_FAVORITOS_PLATILLOS" FOREIGN KEY ("ID_PLATILLO")
        REFERENCES "PLATILLOS" ("ID_PLATILLO")
);

COMMENT ON COLUMN "FAVORITOS"."IdFavoritos" IS
    'Clave primaria añadida según actualización registrada en el diagrama original ("se ha añadido la fila IdFavoritos").';

-- ----------------------------------------------------------------------------
-- Tabla: PEDIDOS
-- ----------------------------------------------------------------------------
CREATE TABLE "PEDIDOS" (
    "ID_PEDIDO"          SERIAL,
    "ID_USUARIO"         INTEGER,
    "FECHA_PEDIDO"       TIMESTAMP,
    "DIRECCION_ENTREGA"  TEXT,
    "TOTAL"              NUMERIC(10,2),
    CONSTRAINT "PK_PEDIDOS" PRIMARY KEY ("ID_PEDIDO"),
    CONSTRAINT "FK_PEDIDOS_USUARIOS" FOREIGN KEY ("ID_USUARIO")
        REFERENCES "USUARIOS" ("ID_USUARIO")
);

COMMENT ON TABLE "PEDIDOS" IS
    'El campo ESTADO fue retirado de esta tabla por estar duplicado con SEGUIMIENTO_PEDIDO; se conserva únicamente en esta última, según actualización documentada en el diagrama fuente.';

-- ----------------------------------------------------------------------------
-- Tabla: SEGUIMIENTO_PEDIDO
-- ----------------------------------------------------------------------------
CREATE TABLE "SEGUIMIENTO_PEDIDO" (
    "ID_SEGUIMIENTO"  SERIAL,
    "ID_PEDIDO"       INTEGER,
    "ESTADO"          VARCHAR(30),
    "FECHA_HORA"      TIMESTAMP,
    "OBSRVACION"      TEXT,
    CONSTRAINT "PK_SEGUIMIENTO_PEDIDO" PRIMARY KEY ("ID_SEGUIMIENTO"),
    CONSTRAINT "FK_SEGUIMIENTO_PEDIDO_PEDIDOS" FOREIGN KEY ("ID_PEDIDO")
        REFERENCES "PEDIDOS" ("ID_PEDIDO")
);

-- ----------------------------------------------------------------------------
-- Tabla: DETALLE_PEDIDO
-- ----------------------------------------------------------------------------
CREATE TABLE "DETALLE_PEDIDO" (
    "ID_DETALLE"       SERIAL,
    "ID_PEDIDO"        INTEGER,
    "ID_PLATILLO"      INTEGER,
    "CANTIDAD"         INTEGER,
    "PRECIO_UNITARIO"  NUMERIC(10,2),
    "SUBTOTAL"         NUMERIC(10,5),
    CONSTRAINT "PK_DETALLE_PEDIDO" PRIMARY KEY ("ID_DETALLE"),
    CONSTRAINT "FK_DETALLE_PEDIDO_PEDIDOS" FOREIGN KEY ("ID_PEDIDO")
        REFERENCES "PEDIDOS" ("ID_PEDIDO"),
    CONSTRAINT "FK_DETALLE_PEDIDO_PLATILLOS" FOREIGN KEY ("ID_PLATILLO")
        REFERENCES "PLATILLOS" ("ID_PLATILLO")
);

-- ----------------------------------------------------------------------------
-- Tabla: PLATILLO_INGREDIENTE (tabla de asociación N:M)
-- ----------------------------------------------------------------------------
CREATE TABLE "PLATILLO_INGREDIENTE" (
    "ID_PLATILLO"     INTEGER,
    "ID_INGREDIENTE"  INTEGER,
    CONSTRAINT "PK_PLATILLO_INGREDIENTE" PRIMARY KEY ("ID_PLATILLO", "ID_INGREDIENTE"),
    CONSTRAINT "FK_PLATILLO_INGREDIENTE_PLATILLOS" FOREIGN KEY ("ID_PLATILLO")
        REFERENCES "PLATILLOS" ("ID_PLATILLO"),
    CONSTRAINT "FK_PLATILLO_INGREDIENTE_INGREDIENTES" FOREIGN KEY ("ID_INGREDIENTE")
        REFERENCES "INGREDIENTES" ("ID_INGREDIENTES")
);

-- ----------------------------------------------------------------------------
-- Tabla: RESEÑAS
-- ----------------------------------------------------------------------------
CREATE TABLE "RESEÑAS" (
    "ID_RESEÑA"     SERIAL,
    "ID_USUARIO"    INTEGER,
    "ID_PLATILLO"   INTEGER,
    "CALIFICACION"  TEXT,
    "COAMNTARIO"    TEXT,
    "FECHA_RESEÑA"  TIMESTAMP,
    CONSTRAINT "PK_RESEÑAS" PRIMARY KEY ("ID_RESEÑA"),
    CONSTRAINT "FK_RESEÑAS_USUARIOS" FOREIGN KEY ("ID_USUARIO")
        REFERENCES "USUARIOS" ("ID_USUARIO"),
    CONSTRAINT "FK_RESEÑAS_PLATILLOS" FOREIGN KEY ("ID_PLATILLO")
        REFERENCES "PLATILLOS" ("ID_PLATILLO")
);

-- ----------------------------------------------------------------------------
-- Tabla: PLATILLO_PERFIL (tabla de asociación N:M)
-- ----------------------------------------------------------------------------
CREATE TABLE "PLATILLO_PERFIL" (
    "ID_PLATILLO"  INTEGER,
    "ID_PERFIL"    INTEGER,
    CONSTRAINT "PK_PLATILLO_PERFIL" PRIMARY KEY ("ID_PLATILLO", "ID_PERFIL"),
    CONSTRAINT "FK_PLATILLO_PERFIL_PLATILLOS" FOREIGN KEY ("ID_PLATILLO")
        REFERENCES "PLATILLOS" ("ID_PLATILLO"),
    CONSTRAINT "FK_PLATILLO_PERFIL_PERFILES" FOREIGN KEY ("ID_PERFIL")
        REFERENCES "PERFILES_FAMILIARES" ("ID_PERFIL")
);

-- ----------------------------------------------------------------------------
-- Índices sobre columnas de clave foránea
-- PostgreSQL NO crea índices automáticamente sobre FK (solo sobre PK/UNIQUE),
-- por lo que se agregan explícitamente para evitar "sequential scans" en JOIN.
-- ----------------------------------------------------------------------------
CREATE INDEX "IDX_RESERVACIONES_ID_USUARIO" ON "RESERVACIONES" ("ID_USUARIO");
CREATE INDEX "IDX_RESERVACIONES_IDMESA" ON "RESERVACIONES" ("IdMesa");
CREATE INDEX "IDX_FAVORITOS_ID_USUARIO" ON "FAVORITOS" ("ID_USUARIO");
CREATE INDEX "IDX_FAVORITOS_ID_PLATILLO" ON "FAVORITOS" ("ID_PLATILLO");
CREATE INDEX "IDX_PEDIDOS_ID_USUARIO" ON "PEDIDOS" ("ID_USUARIO");
CREATE INDEX "IDX_SEGUIMIENTO_PEDIDO_ID_PEDIDO" ON "SEGUIMIENTO_PEDIDO" ("ID_PEDIDO");
CREATE INDEX "IDX_DETALLE_PEDIDO_ID_PEDIDO" ON "DETALLE_PEDIDO" ("ID_PEDIDO");
CREATE INDEX "IDX_DETALLE_PEDIDO_ID_PLATILLO" ON "DETALLE_PEDIDO" ("ID_PLATILLO");
CREATE INDEX "IDX_PLATILLO_INGREDIENTE_ID_INGREDIENTE" ON "PLATILLO_INGREDIENTE" ("ID_INGREDIENTE");
CREATE INDEX "IDX_RESEÑAS_ID_USUARIO" ON "RESEÑAS" ("ID_USUARIO");
CREATE INDEX "IDX_RESEÑAS_ID_PLATILLO" ON "RESEÑAS" ("ID_PLATILLO");
CREATE INDEX "IDX_PLATILLOS_ID_CATEGORIA" ON "PLATILLOS" ("ID_CATEGORIA");
CREATE INDEX "IDX_PLATILLO_PERFIL_ID_PERFIL" ON "PLATILLO_PERFIL" ("ID_PERFIL");

COMMIT;
