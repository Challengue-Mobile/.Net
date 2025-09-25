# Dockerfile para MottothTracking
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar projeto
COPY *.csproj ./
RUN dotnet restore

# Copiar código e build
COPY . .
RUN dotnet publish -c Release -o /app/publish --no-restore

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Criar usuário não-root (OBRIGATÓRIO)
RUN groupadd -r mottothuser && useradd -r -g mottothuser mottothuser
RUN mkdir -p /app && chown -R mottothuser:mottothuser /app

# Copiar app
COPY --from=build /app/publish .
RUN chown -R mottothuser:mottothuser /app

# Configurações
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Executar como não-root
USER mottothuser

ENTRYPOINT ["dotnet", "MottothTracking.dll"]