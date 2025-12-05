# ---------------------------
# Runtime (imagen final)
# ---------------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
# USER $APP_UID   # lo dejamos comentado por ahora

# ---------------------------
# Build
# ---------------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# 1) Copiamos SOLO los .csproj usando LAS RUTAS REALES
COPY WebApi/Common/Common.csproj WebApi/Common/
COPY WebApi/Project.Application/Project.Application.csproj WebApi/Project.Application/
COPY WebApi/Project.Domain/Project.Domain.csproj WebApi/Project.Domain/
COPY WebApi/Project.Infrastructure/Project.Infrastructure.csproj WebApi/Project.Infrastructure/
COPY WebApi/Project.WebApi/Project.WebApi.csproj WebApi/Project.WebApi/

# 2) Restore del proyecto WebApi
RUN dotnet restore "WebApi/Project.WebApi/Project.WebApi.csproj"

# 3) Copiamos TODO el código
COPY . .

# 4) Build del proyecto WebApi
WORKDIR "/src/WebApi/Project.WebApi"
RUN dotnet build "Project.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# ---------------------------
# Publish
# ---------------------------
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR "/src/WebApi/Project.WebApi"
RUN dotnet publish "Project.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# ---------------------------
# Imagen final
# ---------------------------
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Project.WebApi.dll"]
