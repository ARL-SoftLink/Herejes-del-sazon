FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR webapp

EXPOSE 80
COPY ./*.csproj ./
RUN dotnet restore
COPY . . 
RUN dotnet publish -c Release -o out

# build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS runtime-env
WORKDIR /webapp
COPY --from=build /webapp/out .

#configurar entrypoint
ENTRYPOINT ["dotnet","herejes_del_sazon.dll"]