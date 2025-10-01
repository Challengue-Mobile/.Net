#!/usr/bin/env bash
set -euo pipefail

# ====== CONFIG ======
RG="${RG:-rg-mottoth-frotas-devops}"
APP="${APP:-mottoth-api-aci}"
LOC="${LOC:-eastus}"
ACR="${ACR:-acrmottoth1759096657.azurecr.io}"
IMG="${IMG:-$ACR/mottoth-api:oracle}"
SRV="${SRV:-oracle.fiap.com.br}"
USR="${USR:-RM555881}"
# Se ORACLE_PWD não existir, pede de forma silenciosa
PWD_ORA="${ORACLE_PWD:-}"
DNS="mottoth-$(date +%s)"

CONN="Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=${SRV})(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=ORCL)));User Id=${USR};Password=%PASSWORD_PLACEHOLDER%;"

# ====== FUNÇÕES ======
need() { command -v "$1" >/dev/null 2>&1 || { echo "Erro: '$1' não encontrado no PATH."; exit 127; }; }
usage() { echo "Uso: $0 {all|build|deploy|test|run-local}"; exit 1; }
ensure_pwd() {
  if [[ -z "$PWD_ORA" ]]; then
    read -rs -p "Senha Oracle para usuário ${USR}: " PWD_ORA; echo
  fi
}

build() {
  echo "[1/3] docker build..."
  docker build -t "$IMG" .
  echo "[2/3] az acr login..."
  az acr login -n "${ACR%%.*}"
  echo "[3/3] docker push..."
  docker push "$IMG"
  echo "OK -> $IMG"
}

deploy() {
  ensure_pwd
  echo "[0] removendo ACI anterior (se existir)..."
  az container delete -g "$RG" -n "$APP" -y >/dev/null 2>&1 || true

  echo "[1] criando ACI..."
  # injeta a senha somente na variável segura
  local CONN_SAFE="${CONN//%PASSWORD_PLACEHOLDER%/$PWD_ORA}"

  az container create \
    -g "$RG" -n "$APP" -l "$LOC" \
    --image "$IMG" \
    --os-type Linux --cpu 1 --memory 1.5 \
    --ports 8080 --ip-address Public \
    --dns-name-label "$DNS" \
    --registry-login-server "$ACR" \
    --registry-username "$(az acr credential show -n ${ACR%%.*} --query username -o tsv)" \
    --registry-password "$(az acr credential show -n ${ACR%%.*} --query passwords[0].value -o tsv)" \
    --environment-variables ASPNETCORE_ENVIRONMENT=Production ASPNETCORE_URLS="http://+:8080" \
    --secure-environment-variables "ConnectionStrings__OracleConnection=$CONN_SAFE"

  echo "[2] aguardando 10s..."
  sleep 10

  echo "[3] status/IP:"
  az container show -g "$RG" -n "$APP" --query "ipAddress.{ip:ip,fqdn:fqdn}" -o table
  az container show -g "$RG" -n "$APP" --query "instanceView.state" -o tsv
}

test_up() {
  local FQDN
  FQDN="$(az container show -g "$RG" -n "$APP" --query ipAddress.fqdn -o tsv)"
  if [[ -z "$FQDN" ]]; then echo "FQDN vazio. Verifique se o container subiu."; exit 1; fi

  echo "Testando: http://$FQDN:8080/"
  curl -i "http://$FQDN:8080/"

  echo
  echo "Testando: /api/ping"
  curl -i "http://$FQDN:8080/api/ping"

  echo
  echo "Testando: swagger json"
  curl -i "http://$FQDN:8080/swagger/v1/swagger.json"
}

run_local() {
  ensure_pwd
  local CONN_SAFE="${CONN//%PASSWORD_PLACEHOLDER%/$PWD_ORA}"
  docker run --rm -p 8080:8080 \
    -e ASPNETCORE_ENVIRONMENT=Development \
    -e ASPNETCORE_URLS="http://+:8080" \
    -e "ConnectionStrings__OracleConnection=$CONN_SAFE" \
    "$IMG"
}

# ====== MAIN ======
need docker; need az; need curl

ACTION="${1:-all}"
case "$ACTION" in
  all)
    build
    deploy
    test_up
    ;;
  build) build ;;
  deploy) deploy ;;
  test) test_up ;;
  run-local) run_local ;;
  *) usage ;;
esac
