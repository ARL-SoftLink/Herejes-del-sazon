#!/bin/bash
set -e

echo "Deteniendo el entorno de desarrollo..."

# Detener la webapp
docker compose down

# Detener el reenvío de puertos hacia PostgreSQL
pkill -f "kubectl port-forward svc/postgresql-ha-rw" 2>/dev/null || true

# (Opcional) eliminar el Job de migraciones ya ejecutado
kubectl delete job flyway-migrate --ignore-not-found

# (Opcional) apagar Minikube por completo
minikube stop # comentalo si cambias de parecer

echo "Entorno detenido."