#!/usr/bin/env bash
set -euo pipefail

# ===== CONFIG =====
RG="rg-mottoth-frotas-devops"
LOCATION="eastus"
ACR_NAME="acrmottoth1759096657"
APP_NAME="mottoth-api-aci"
IMAGE_NAME="mottoth-api"
IMAGE_TAG="latest"
PORT="8080"
CPU="1"
MEMORY="1.5"
DNS_LABEL="mottoth-$RANDOM"   # opcional; precisa ser único na região

ENV_VARS=(
  "ASPNETCORE_ENVIRONMENT=Production"
)

log(){ echo -e "\033[1;34m[INFO]\033[0m $*"; }
err(){ echo -e "\033[1;31m[ERRO]\033[0m $*" >&2; }
need(){ command -v "$1" >/dev/null 2>&1 || { err "Comando '$1' não encontrado"; exit 1; }; }

need az

log "RG=$RG  ACR=$ACR_NAME  IMG=$IMAGE_NAME:$IMAGE_TAG  APP=$APP_NAME"

# valida ACR
az acr show -g "$RG" -n "$ACR_NAME" >/dev/null 2>&1 || { err "ACR '$ACR_NAME' não existe no RG '$RG'"; exit 1; }

# credenciais do ACR
USER="$(az acr credential show -n "$ACR_NAME" --query username -o tsv)"
PASS="$(az acr credential show -n "$ACR_NAME" --query passwords[0].value -o tsv)"
IMAGE_FQDN="${ACR_NAME}.azurecr.io/${IMAGE_NAME}:${IMAGE_TAG}"

# remove ACI antiga se existir
if az container show -g "$RG" -n "$APP_NAME" >/dev/null 2>&1; then
  log "Removendo ACI antiga '$APP_NAME'..."
  az container delete -g "$RG" -n "$APP_NAME" --yes
fi

# monta env
ENV_ARGS=()
for kv in "${ENV_VARS[@]}"; do
  ENV_ARGS+=( --environment-variables "$kv" )
done

# cria ACI com porta exposta e DNS público
log "Criando ACI '$APP_NAME' em $LOCATION..."
az container create \
  -g "$RG" -n "$APP_NAME" -l "$LOCATION" \
  --image "$IMAGE_FQDN" \
  --registry-login-server "${ACR_NAME}.azurecr.io" \
  --registry-username "$USER" \
  --registry-password "$PASS" \
  --os-type Linux \
  --ports "$PORT" \
  --dns-name-label "$DNS_LABEL" \
  --cpu "$CPU" --memory "$MEMORY" \
  "${ENV_ARGS[@]}"

# aguarda IP
log "Aguardando IP/FQDN..."
for i in {1..30}; do
  IP="$(az container show -g "$RG" -n "$APP_NAME" --query ipAddress.ip -o tsv || true)"
  FQDN="$(az container show -g "$RG" -n "$APP_NAME" --query ipAddress.fqdn -o tsv || true)"
  [[ -n "${IP}" && "${IP}" != "null" ]] && break
  sleep 4
done

if [[ -z "${IP:-}" || "$IP" == "null" ]]; then
  err "Sem IP ainda. Status:"
  az container show -g "$RG" -n "$APP_NAME" --query "{state:instanceView.state,ip:ipAddress.ip,ports:ipAddress.ports}" -o table || true
  log "Logs:"
  az container logs -g "$RG" -n "$APP_NAME" || true
  exit 1
fi

echo
echo "OK! Endpoints:"
printf "  http://%s:%s/swagger\n" "$IP" "$PORT"
if [[ -n "${FQDN:-}" && "$FQDN" != "null" ]]; then
  printf "  http://%s:%s/swagger\n" "$FQDN" "$PORT"
fi
