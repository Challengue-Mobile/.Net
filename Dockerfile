# ---- Build (SDK .NET 9) ----
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia o csproj primeiro para aproveitar cache
COPY MottothTracking.csproj ./
RUN dotnet restore MottothTracking.csproj -r linux-x64

# Copia o restante e publica (sempre o .csproj, não a .sln)
COPY . ./
RUN dotnet publish MottothTracking.csproj -c Release -r linux-x64 \
    -o /app/out --no-self-contained

# ---- Runtime (ASP.NET 9) ----
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Porta de exposição do container
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Usuário não-root (opcional, mas recomendado)
RUN groupadd -r mottothuser && useradd -r -g mottothuser mottothuser
USER mottothuser

# Copia binários publicados
COPY --from=build /app/out ./

# Substitua pelo nome do seu assembly se for diferente
ENTRYPOINT ["dotnet", "MottothTracking.dll"]
