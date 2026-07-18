#!/bin/bash
set -e

# Cargar .env si existe (entorno local)
if [ -f .env ]; then
    echo "Cargando variables desde .env (modo local)..."
    set -a
    source .env
    set +a
fi

# Colores
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
RED='\033[0;31m'
NC='\033[0m'

echo -e "${GREEN}Iniciando entorno de desarrollo (Kubernetes para la BD + Docker Compose para la webapp)...${NC}"

# 1. Validar que las credenciales existan como variables de entorno
#    (deben venir de GitHub Codespaces Secrets; no usamos .env)
: "${POSTGRES_USER:?POSTGRES_USER no está definida. Configúrala en Settings > Secrets and variables > Codespaces y reconstruye el Codespace.}"
: "${POSTGRES_PASSWORD:?POSTGRES_PASSWORD no está definida. Configúrala en Settings > Secrets and variables > Codespaces y reconstruye el Codespace.}"
: "${POSTGRES_DB:?POSTGRES_DB no está definida. Configúrala en Settings > Secrets and variables > Codespaces y reconstruye el Codespace.}"

# --- Detección de recursos del sistema y límites dinámicos ---
detect_resources() {
    # Inicializar variables
    TOTAL_CORES=0
    TOTAL_RAM_GB=0

    if [[ "$OSTYPE" == "linux-gnu"* ]]; then
        # Linux (incluye WSL2, Codespaces)
        if command -v nproc &> /dev/null; then
            TOTAL_CORES=$(nproc)
        else
            TOTAL_CORES=$(grep -c ^processor /proc/cpuinfo)
        fi
        if command -v free &> /dev/null; then
            TOTAL_RAM_GB=$(free -g | awk '/^Mem:/{print $2}')
        else
            TOTAL_RAM_KB=$(grep MemTotal /proc/meminfo | awk '{print $2}')
            TOTAL_RAM_GB=$((TOTAL_RAM_KB / 1024 / 1024))
        fi
    elif [[ "$OSTYPE" == "darwin"* ]]; then
        # macOS
        TOTAL_CORES=$(sysctl -n hw.ncpu)
        TOTAL_RAM_BYTES=$(sysctl -n hw.memsize)
        TOTAL_RAM_GB=$((TOTAL_RAM_BYTES / 1024 / 1024 / 1024))
    elif [[ "$OSTYPE" == "cygwin" ]] || [[ "$OSTYPE" == "msys" ]]; then
        # Git Bash / Windows (fallback con wmic si existe)
        if command -v wmic &> /dev/null; then
            TOTAL_CORES=$(wmic cpu get NumberOfCores | grep -Eo '[0-9]+' | head -1)
            TOTAL_RAM_MB=$(wmic OS get TotalVisibleMemorySize | grep -Eo '[0-9]+' | head -1)
            TOTAL_RAM_GB=$((TOTAL_RAM_MB / 1024))
        else
            # Fallback: asumir valores típicos de Windows (2 cores, 4GB)
            echo -e "${YELLOW}⚠️ No se pudo detectar hardware en Windows. Usando valores por defecto: 2 cores, 4GB RAM.${NC}"
            TOTAL_CORES=2
            TOTAL_RAM_GB=4
        fi
    else
        # Otros sistemas (desconocidos)
        echo -e "${YELLOW}⚠️ Sistema operativo no reconocido. Usando valores por defecto: 2 cores, 4GB RAM.${NC}"
        TOTAL_CORES=2
        TOTAL_RAM_GB=4
    fi

    # Asegurar que las variables tengan valores numéricos válidos
    if [[ -z "$TOTAL_CORES" || "$TOTAL_CORES" -eq 0 ]]; then
        TOTAL_CORES=2
    fi
    if [[ -z "$TOTAL_RAM_GB" || "$TOTAL_RAM_GB" -eq 0 ]]; then
        TOTAL_RAM_GB=4
    fi

    # Calcular límites: 25% de los recursos con tope máximo para no saturar
    CPU_LIMIT_M=$((TOTAL_CORES * 250))   # 250m por núcleo (0.25 núcleo)
    if [ $CPU_LIMIT_M -lt 250 ]; then
        CPU_LIMIT_M=250                  # mínimo 250m (¼ de núcleo)
    elif [ $CPU_LIMIT_M -gt 4000 ]; then
        CPU_LIMIT_M=4000                 # máximo 4 núcleos (4000m)
    fi
    POSTGRES_CPU_LIMIT="${CPU_LIMIT_M}m"

    # RAM: 25% de la RAM total, mínimo 512Mi, máximo 8Gi
    RAM_LIMIT_GB=$((TOTAL_RAM_GB / 4))
    if [ $RAM_LIMIT_GB -lt 1 ]; then
        POSTGRES_MEM_LIMIT="512Mi"
    elif [ $RAM_LIMIT_GB -gt 8 ]; then
        POSTGRES_MEM_LIMIT="8Gi"
    else
        POSTGRES_MEM_LIMIT="${RAM_LIMIT_GB}Gi"
    fi

    echo -e "${GREEN}🔍 Recursos detectados:${NC}"
    echo "   CPU: $TOTAL_CORES núcleos → asignando $POSTGRES_CPU_LIMIT"
    echo "   RAM: ${TOTAL_RAM_GB}GB → asignando $POSTGRES_MEM_LIMIT"
}

# Ejecutar la detección
detect_resources

# 2. Verificar/iniciar Minikube
if ! minikube status > /dev/null 2>&1; then
    echo -e "${YELLOW}Minikube no está corriendo. Iniciando...${NC}"
    minikube start --driver=docker
fi

# 3. Instalar (o verificar) el operador CloudNativePG
if ! kubectl get ns cnpg-system > /dev/null 2>&1; then
    echo -e "${GREEN}Instalando el operador CloudNativePG...${NC}"
    helm repo add cloudnative-pg https://cloudnative-pg.github.io/charts > /dev/null
    helm repo update > /dev/null
    helm upgrade --install cnpg cloudnative-pg/cloudnative-pg \
      --namespace cnpg-system --create-namespace
    kubectl wait --for=condition=Available deployment --all -n cnpg-system --timeout=180s
else
    echo -e "${GREEN}Operador CloudNativePG ya instalado.${NC}"
fi

# 4. Crear/actualizar el Secret con las credenciales
echo -e "${GREEN}Creando Secret de credenciales de PostgreSQL...${NC}"
kubectl create secret generic postgresql-credentials \
  --from-literal=username="$POSTGRES_USER" \
  --from-literal=password="$POSTGRES_PASSWORD" \
  --dry-run=client -o yaml | kubectl apply -f -

# 5. Generar los manifiestos finales a partir de las plantillas
echo -e "${GREEN}Generando manifiestos de Kubernetes...${NC}"

sed -e "s|__POSTGRES_USER__|$POSTGRES_USER|g" \
    -e "s|__POSTGRES_DB__|$POSTGRES_DB|g" \
    -e "s|__POSTGRES_CPU_LIMIT__|$POSTGRES_CPU_LIMIT|g" \
    -e "s|__POSTGRES_MEM_LIMIT__|$POSTGRES_MEM_LIMIT|g" \
    k8s/postgresql-cluster.yaml.template > k8s/postgresql-cluster.yaml

sed -e "s|__POSTGRES_USER__|$POSTGRES_USER|g" -e "s|__POSTGRES_DB__|$POSTGRES_DB|g" \
  k8s/flyway-job.yaml.template > k8s/flyway-job.yaml

# 6. Aplicar el clúster de PostgreSQL
echo -e "${GREEN}Aplicando el clúster de PostgreSQL (esto puede tardar 1-2 minutos la primera vez)...${NC}"
kubectl apply -f k8s/postgresql-cluster.yaml

# 7. Esperar a que las 3 instancias estén listas
echo -e "${YELLOW}Esperando a que las 3 instancias de PostgreSQL estén listas...${NC}"
kubectl wait --for=condition=Ready cluster/postgresql-ha --timeout=300s

# 8. Generar el ConfigMap con los scripts SQL de Flyway
echo -e "${GREEN}Sincronizando scripts de migraciones...${NC}"
kubectl create configmap flyway-sql-scripts \
  --from-file="$(pwd)/migrations" \
  --dry-run=client -o yaml | kubectl apply -f -

# 9. Ejecutar el Job de Flyway
echo -e "${GREEN}Ejecutando migraciones con Flyway...${NC}"
kubectl delete job flyway-migrate --ignore-not-found
kubectl apply -f k8s/flyway-job.yaml
if ! kubectl wait --for=condition=complete --timeout=120s job/flyway-migrate; then
    echo -e "${RED}Las migraciones fallaron. Logs del Job:${NC}"
    kubectl logs job/flyway-migrate
    exit 1
fi

# 10. Reenviar el Service de PostgreSQL (rw) a localhost:5432
#     Se reintenta automáticamente si el pod primario cambia (por ejemplo, durante un failover).
echo -e "${GREEN}Exponiendo PostgreSQL en localhost:5432...${NC}"
pkill -f "kubectl port-forward svc/postgresql-ha-rw" 2>/dev/null || true
nohup bash -c '
  while true; do
    kubectl port-forward svc/postgresql-ha-rw 5432:5432 --address 0.0.0.0
    echo "[port-forward] conexión perdida, reintentando en 2s..."
    sleep 2
  done
' > portforward.log 2>&1 &
disown
sleep 3

# 11. Levantar la webapp con Docker Compose
echo -e "${GREEN}Levantando la aplicación web con Docker Compose...${NC}"
docker compose up -d --build

echo -e "${GREEN}¡Entorno listo! App disponible en http://localhost:8080${NC}"
echo -e "${YELLOW}Tip: revisa 'kubectl get pods -l cnpg.io/cluster=postgresql-ha -L cnpg.io/instanceRole' para ver cuál instancia es la primaria.${NC}"