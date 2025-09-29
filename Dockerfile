# ===== build =====
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# copiar csproj e restaurar
COPY *.csproj ./
RUN dotnet restore

# copiar código e publicar
COPY . ./
RUN dotnet publish -c Release -o /out

# ===== runtime =====
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# criar usuário não-root
RUN groupadd -r mottothuser && useradd -r -g mottothuser mottothuser \
 && chown -R mottothuser:mottothuser /app
USER mottothuser

# copiar artefatos publicados
COPY --from=build /out ./

# expor e bindar 8080
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
EXPOSE 8080

# ajuste o nome do DLL se seu AssemblyName for diferente
ENTRYPOINT ["dotnet", "MottothTracking.dll"]
