#!/usr/bin/env bash
set -euo pipefail

# ===== Variáveis =====
RG="rg-mottoth-frotas-devops"
LOC="eastus"
ACI="mottoth-api-aci"
ACR="acrmottoth1759096657"
IMAGE="$ACR.azurecr.io/mottoth-api:latest"
CPU=1
MEMORY=1.5
PORT=8080
DNS_LABEL="mottoth-$RANDOM"

# ⚠️ SENHA ATUALIZADA
SQL_SERVER_FQDN="mottoth-sqlserver25370.database.windows.net"
SQL_DB="mottothdb"
SQL_USER="sqladmin"
SQL_PASS="NovaSenhaSegura#2025"

CONN="Server=tcp:${SQL_SERVER_FQDN},1433;Initial Catalog=${SQL_DB};User ID=${SQL_USER};Password=${SQL_PASS};Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"

echo "[INFO] Deletando ACI antigo (se existir)..."
az container delete -g "$RG" -n "$ACI" --yes 2>/dev/null || true

echo "[INFO] Aguardando exclusão completa..."
sleep 15

echo "[INFO] Verificando se ACI foi deletado..."
if az container show -g "$RG" -n "$ACI" 2>/dev/null; then
    echo "[ERRO] ACI ainda existe. Aguardando mais 15 segundos..."
    sleep 15
fi

echo "[INFO] Criando novo ACI..."
set +H  # Evita problema com '!' no bash
az container create \
  -g "$RG" -n "$ACI" -l "$LOC" \
  --image "$IMAGE" \
  --os-type Linux --cpu $CPU --memory $MEMORY \
  --ports $PORT --ip-address Public --restart-policy OnFailure \
  --dns-name-label "$DNS_LABEL" \
  --registry-login-server "$ACR.azurecr.io" \
  --registry-username "$(az acr credential show -n $ACR --query username -o tsv)" \
  --registry-password "$(az acr credential show -n $ACR --query passwords[0].value -o tsv)" \
  --environment-variables \
    ASPNETCORE_ENVIRONMENT=Production \
    ASPNETCORE_URLS="http://+:$PORT" \
  --secure-environment-variables \
    ConnectionStrings__Default="$CONN"

echo "[INFO] Aguardando ACI inicializar..."
for i in {1..30}; do
    STATE=$(az container show -g "$RG" -n "$ACI" --query instanceView.state -o tsv 2>/dev/null || echo "Unknown")
    echo "[INFO] Tentativa $i/30: Estado = $STATE"
    if [ "$STATE" == "Running" ]; then
        break
    fi
    sleep 10
done

FQDN=$(az container show -g "$RG" -n "$ACI" --query ipAddress.fqdn -o tsv)
IP=$(az container show -g "$RG" -n "$ACI" --query ipAddress.ip -o tsv)

echo ""
echo "=========================================="
echo "✅ ACI CRIADO COM SUCESSO!"
echo "=========================================="
echo "IP:   $IP"
echo "FQDN: $FQDN"
echo ""
echo "📋 COMANDOS DE TESTE:"
echo "curl -i http://$IP:$PORT/"
echo "curl -i http://$IP:$PORT/api/ping"
echo "curl -i http://$FQDN:$PORT/swagger/index.html"
echo ""
echo "📋 VER LOGS:"
echo "az container logs -g $RG -n $ACI --follow"
echo "=========================================="
