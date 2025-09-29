#!/usr/bin/env bash
set -euo pipefail

### =====================[ CONFIG ]=====================

# Assinatura atual (apenas para log informativo)
SUB="$(az account show --query name -o tsv 2>/dev/null || echo 'deslogado')"

# Azure
RG="rg-mottoth-frotas-devops"
LOCATION="eastus"
ACR="acrmottoth1759096657"

# App/Imagem
APP="mottoth-api-aci"
REPO="mottoth-api"
TAG="latest"
PORT="8080"

# Oracle FIAP  (<<< edite aqui se precisar trocar usuário/senha >>>)
ORACLE_HOST="oracle.fiap.com.br"
ORACLE_USER="rm558798"
ORACLE_PASS="fiap24"
CONN="Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=${ORACLE_HOST})(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=ORCL)));User Id=${ORACLE_USER};Password=${ORACLE_PASS};"

# Projeto .NET (csproj) esperado na raiz
CSPROJ="MottothTracking.csproj"

### =====================[ FUNÇÕES ]=====================

log() { echo -e "\n[INFO] $*"; }
warn() { echo -e "\n[AVISO] $*" >&2; }
die() { echo -e "\n[ERRO] $*" >&2; exit 1; }

ensure_in_repo() {
  [[ -f "$CSPROJ" ]] || die "Não achei ${CSPROJ} na pasta atual. Rode este script na RAIZ do projeto."
}

ensure_rg() {
  log "Garantindo Resource Group…"
  az group create -n "$RG" -l "$LOCATION" 1>/dev/null
}

ensure_acr() {
  log "Garantindo ACR '$ACR'…"
  if ! az acr show -g "$RG" -n "$ACR" &>/dev/null; then
    az acr create -g "$RG" -n "$ACR" -l "$LOCATION" --sku Basic --admin-enabled true 1>/dev/null
  else
    log "ACR já existe."
  fi
  if ! az acr check-health -n "$ACR" --yes &>/dev/null; then
    warn "acr check-health reportou alerta (ok prosseguir)."
  fi
}

ensure_docker_running() {
  log "Garantindo Docker ativo…"
  docker ps >/dev/null 2>&1 || { sudo systemctl start docker; sleep 3; docker ps >/dev/null || die "Docker não está rodando." ; }
}

ensure_dockerfile() {
  if [[ -f "Dockerfile" ]]; then
    return
  fi
  log "Gerando Dockerfile padrão…"
  cat > Dockerfile <<'EOF'
# ---------- build ----------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app
COPY *.csproj ./
RUN dotnet restore
COPY . ./
RUN dotnet publish -c Release -o /out

# ---------- runtime ----------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# usuário NÃO-root (requisito)
RUN addgroup --system mottothuser && adduser --system --ingroup mottothuser mottothuser \
  && chown -R mottothuser:mottothuser /app
USER mottothuser

COPY --from=build /out ./
# Porta será exposta pelo ACI, mas manter compatibilidade local:
ENV ASPNETCORE_URLS=http://+:8080
# ENTRYPOINT padrão do publish (.dll)
ENTRYPOINT ["dotnet","MottothTracking.dll"]
EOF
}

acr_login_docker() {
  log "Login no ACR (token)…"
  local token
  token="$(az acr login -n "$ACR" --expose-token --query accessToken -o tsv)"
  # login usando refresh token (usuário dummy exigido pelo Docker)
  echo "$token" | docker login "${ACR}.azurecr.io" \
    -u 00000000-0000-0000-0000-000000000000 --password-stdin 1>/dev/null
}

build_and_push() {
  local image="${ACR}.azurecr.io/${REPO}:${TAG}"
  log "Build da imagem local: ${image}"
  docker build -t "${image}" .
  log "Push da imagem para o ACR…"
  docker push "${image}"
}

delete_old_aci() {
  # sem --no-wait (compatível)
  if az container show -g "$RG" -n "$APP" &>/dev/null; then
    log "Deletando ACI '$APP'…"
    az container delete -g "$RG" -n "$APP" --yes
    echo -n "[INFO] Aguardando remoção"
    for i in {1..30}; do
      if ! az container show -g "$RG" -n "$APP" &>/dev/null; then
        echo; log "ACI removida."
        break
      fi
      echo -n "."
      sleep 5
    done
  fi
}

create_aci() {
  local image="${ACR}.azurecr.io/${REPO}:${TAG}"
  log "Criando ACI '$APP' em $LOCATION…"

  # credenciais do ACR para o ACI puxar a imagem
  local user pass
  user="$(az acr credential show -n "$ACR" --query username -o tsv)"
  pass="$(az acr credential show -n "$ACR" --query passwords[0].value -o tsv)"

  # rótulo DNS previsível (opcional)
  local dnslbl="mottoth-$RANDOM"

  az container create \
    -g "$RG" -n "$APP" -l "$LOCATION" \
    --image "$image" \
    --os-type Linux \
    --cpu 1 --memory 1.5 \
    --restart-policy OnFailure \
    --ports "$PORT" \
    --dns-name-label "$dnslbl" \
    --ip-address Public \
    --registry-login-server "${ACR}.azurecr.io" \
    --registry-username "$user" \
    --registry-password "$pass" \
    --environment-variables ASPNETCORE_ENVIRONMENT=Production ASPNETCORE_URLS="http://+:${PORT}" \
    --secure-environment-variables ConnectionStrings__OracleConnection="$CONN" \
    1>/dev/null

  # aguarda IP
  echo -n "[INFO] Aguardando IP público"
  for i in {1..40}; do
    local ip
    ip="$(az container show -g "$RG" -n "$APP" --query ipAddress.ip -o tsv || true)"
    if [[ -n "${ip}" && "${ip}" != "null" ]]; then
      echo
      break
    fi
    echo -n "."
    sleep 5
  done
}

print_endpoint_and_smoke() {
  log "Endpoint público:"
  az container show -g "$RG" -n "$APP" --query "ipAddress.{ip:ip,fqdn:fqdn}" -o table

  local ip fqdn base
  ip="$(az container show -g "$RG" -n "$APP" --query ipAddress.ip -o tsv || true)"
  fqdn="$(az container show -g "$RG" -n "$APP" --query ipAddress.fqdn -o tsv || true)"

  if [[ -n "${ip}" && "${ip}" != "null" ]]; then
    base="http://${ip}:${PORT}"
  elif [[ -n "${fqdn}" && "${fqdn}" != "null" ]]; then
    base="http://${fqdn}:${PORT}"
  else
    warn "Sem IP/FQDN ainda. Veja status: az container show -g \"$RG\" -n \"$APP\" -o json"
    return
  fi

  log "Testes rápidos (copie e cole no terminal):"
  echo "curl -i '${base}/'"
  echo "curl -I '${base}/swagger/index.html'"
  echo "curl -I '${base}/swagger/v1/swagger.json'"
}

### =====================[ MAIN ]=====================

echo "[INFO] Assinatura: ${SUB}"
echo "[INFO] RG=${RG} | LOCATION=${LOCATION} | ACR=${ACR}"
echo "[INFO] APP=${APP} | REPO=${REPO} | TAG=${TAG} | PORT=${PORT}"
echo "[INFO] Oracle: ${ORACLE_USER}@${ORACLE_HOST} (SERVICE_NAME=ORCL)"

ensure_in_repo
ensure_rg
ensure_acr
ensure_docker_running
ensure_dockerfile
acr_login_docker
build_and_push
delete_old_aci
create_aci
print_endpoint_and_smoke

echo -e "\n[OK] Deploy finalizado. 🚀"
