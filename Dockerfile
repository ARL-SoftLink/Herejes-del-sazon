# Imagen base con SDK (necesario para compilar en caliente)
FROM mcr.microsoft.com/dotnet/sdk:8.0

WORKDIR /webapp

# Instalar herramientas
RUN dotnet tool install --global dotnet-ef
RUN apt-get update && apt-get install -y postgresql-client && rm -rf /var/lib/apt/lists/*
ENV PATH="$PATH:/root/.dotnet/tools"

# Copiar script de entrada y dar permisos
COPY entrypoint.sh .
RUN chmod +x entrypoint.sh

ENTRYPOINT ["./entrypoint.sh"]
EXPOSE 8080