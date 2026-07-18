#!/bin/bash
set -e  # Detiene la ejecución si algo falla

echo "Esperando a que la base de datos esté lista..."
until PGPASSWORD="$POSTGRES_PASSWORD" psql -h "$DATABASE_HOST" -p "$DATABASE_PORT" -U "$POSTGRES_USER" -d "$POSTGRES_DB" -c '\q'; do
  echo "Esperando a que PostgreSQL esté listo en $DATABASE_HOST:$DATABASE_PORT..."
  sleep 2
done
echo "Base de datos disponible."

# === SCAFFOLD INTELIGENTE (solo si hay cambios en migraciones) ===
SHOULD_RUN_SCAFFOLD=false
HASH_FILE="/migrations/.last_scaffold_hash"  # Archivo persistente dentro de migrations, esto hara que en cada levantamiento valide si en ocasiones anteriores ya leyo archivos antiguos sql

# Caso 1: No existe el DbContext (primera vez que se monta el código)
if [ ! -f "Models/MyDBContext.cs" ]; then
  echo "Models/MyDBContext.cs no encontrado. Ejecutando scaffold inicial..."
  SHOULD_RUN_SCAFFOLD=true
# Caso 2: Existe la carpeta /migrations (donde guardas los scripts SQL)
elif [ -d "/migrations" ]; then
  echo "Verificando cambios en scripts SQL (carpeta /migrations)..."
  
  # Calcula el hash combinado de todos los archivos .sql en /migrations
  # awk '{print $1}' extrae solo el hash numérico, ignorando rutas de archivo
  NEW_HASH=$(find /migrations -name "*.sql" -exec md5sum {} \; | sort -k 2 | md5sum | awk '{print $1}')
  OLD_HASH=$(cat "$HASH_FILE" 2>/dev/null || echo "no_hash")
  
  if [ "$NEW_HASH" == "$OLD_HASH" ]; then
    echo "Sin cambios en migraciones. Saltando scaffold para acelerar el inicio."
  else
    echo "Cambios detectados en migraciones (o primera ejecución con hash). Ejecutando scaffold..."
    SHOULD_RUN_SCAFFOLD=true
  fi
# Caso 3: No existe /migrations (por si no la montaste)
else
  echo "Carpeta /migrations no encontrada. Ejecutando scaffold por seguridad (para mantener sincronía)."
  SHOULD_RUN_SCAFFOLD=true
fi

# === EJECUTAR SCAFFOLD (si es necesario) ===
if [ "$SHOULD_RUN_SCAFFOLD" = true ]; then
  CONNSTRING="Host=$DATABASE_HOST;Port=$DATABASE_PORT;Database=$POSTGRES_DB;Username=$POSTGRES_USER;Password=$POSTGRES_PASSWORD"
  
  # ===== NUEVO: Limpieza y restauración limpia =====
  echo "Eliminando cachés previas (obj/ y bin/) para evitar conflictos de plataforma..."
  rm -rf obj bin
  echo "Restaurando paquetes NuGet desde cero (sin caché)..."
  dotnet restore --no-cache
  # ================================================
    
  echo "Ejecutando dotnet ef dbcontext scaffold (sobrescribiendo con --force)..."
  dotnet ef dbcontext scaffold "$CONNSTRING" Npgsql.EntityFrameworkCore.PostgreSQL \
    --output-dir Models \
    --context MyDBContext \
    --force \
    --no-onconfiguring # comando agregado el 120726 tras hallazgo critico con exposicion de credenciales
  
  echo "Scaffold completado."
  
  # Guarda el nuevo hash para la próxima vez (solo si existe /migrations)
  if [ -d "/migrations" ]; then
    NEW_HASH=$(find /migrations -name "*.sql" -exec md5sum {} \; | sort -k 2 | md5sum | awk '{print $1}')
    echo "$NEW_HASH" > "$HASH_FILE"
    echo "Hash de migraciones guardado."
  fi
else
  echo "Scaffold omitido. Los modelos ya están sincronizados."
fi

echo "Iniciando la aplicación..."
# Usamos 'exec' para que el proceso de dotnet reemplace al script y reciba las señales (Ctrl+C, SIGTERM) correctamente
exec dotnet run --no-launch-profile