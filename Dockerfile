# 1. Etapa de ejecución (Runtime de .NET 8)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

# 2. Etapa de compilación (SDK de .NET 8)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar proyecto y restaurar dependencias
COPY ["SpotifyClone.API.csproj", "./"]
RUN dotnet restore "SpotifyClone.API.csproj"

# Copiar todo el código y compilar
COPY . .
RUN dotnet build "SpotifyClone.API.csproj" -c Release -o /app/build

# 3. Publicación
FROM build AS publish
RUN dotnet publish "SpotifyClone.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 4. Imagen final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SpotifyClone.API.dll"]