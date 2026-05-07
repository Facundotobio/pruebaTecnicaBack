FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar archivos y restaurar
COPY *.csproj ./
RUN dotnet restore

# Copiar todo lo demas y publicar
COPY . ./
RUN dotnet publish PruebaTecnicaFacundoTobioBack.csproj -c Release -o out

# Imagen de ejecucion
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "PruebaTecnicaFacundoTobioBack.dll"]
