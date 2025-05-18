
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar toda la solución
COPY . .

# Restaurar dependencias y construir
ENV EnableSourceControlManagerQueries=false
RUN dotnet restore "src/Challengers.Api/Challengers.Api.csproj"
RUN dotnet build "src/Challengers.Api/Challengers.Api.csproj" -c Release -o /app/build

# Publicar
FROM build AS publish
RUN dotnet publish "src/Challengers.Api/Challengers.Api.csproj" -c Release -o /app/publish

# Imagen final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Challengers.Api.dll"]
