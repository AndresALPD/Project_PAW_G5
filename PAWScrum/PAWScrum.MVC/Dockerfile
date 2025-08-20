# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar toda la solución
COPY . .

# Restaurar dependencias
RUN dotnet restore PAWScrum.sln

# Publicar MVC
WORKDIR /src/PAWScrum.MVC
RUN dotnet publish -c Release -o /app/out

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["dotnet", "PAWScrum.MVC.dll"]
