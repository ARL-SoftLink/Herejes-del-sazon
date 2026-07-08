FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR webapp


COPY ./*.csproj ./
RUN dotnet restore
COPY . . 
RUN dotnet publish -c Release -o out

# build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS final
WORKDIR /webapp


# Instala herramientas necesarias
RUN dotnet tool install --global dotnet-ef
RUN apt-get update && apt-get install -y postgresql-client && rm -rf /var/lib/apt/lists/*
ENV PATH="$PATH:/root/.dotnet/tools"

# Copia el script de entrada
COPY entrypoint.sh .
RUN chmod +x entrypoint.sh

ENTRYPOINT ["./entrypoint.sh"]
EXPOSE 8080