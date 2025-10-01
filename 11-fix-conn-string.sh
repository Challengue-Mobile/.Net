#!/usr/bin/env bash
set -euo pipefail

RG="rg-mottoth-frotas-devops"
ACI="mottoth-api-aci"
ACR="acrmottoth1759096657"
IMAGE="$ACR.azurecr.io/mottoth-api:latest"
LOC="eastus"
CPU=1
MEMORY=1.5
PORT=8080
DNS_LABEL="mottoth-$RANDOM"

# Connection string CORRIGIDA (sem // depois de tcp:)
CONN_STRING="Server=tcp:mottoth-sqlserver25370.database.windows.net,1433;Initial Catalog=mottothdb;User ID=sqladmin;Password=NovaSenhaSegura#2025;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"

echo "[INFO] Deletando ACI..."
az container delete -g "$RG" -n "$ACI" --yes
sleep 20

echo "[INFO] Criando ACI com connection string corrigida..."
az container create \
  -g "$RG" -n "$ACI" -l "$LOC" \
  --image "$IMAGE" \
  --os-type Linux --cpu $CPU --memory $MEMORY \
  --ports $PORT --ip-address Public --restart-policy Always \
  --dns-name-label "$DNS_LABEL" \
  --registry-login-server "$ACR.azurecr.io" \
  --registry-username "$(az acr credential show -n $ACR --query username -o tsv)" \
  --registry-password "$(az acr credential show -n $ACR --query passwords[0].value -o tsv)" \
  --environment-variables \
    ASPNETCORE_ENVIRONMENT=Production \
    ASPNETCORE_URLS=http://+:8080 \
    "ConnectionStrings__Default=$CONN_STRING"

sleep 30

FQDN=$(az container show -g "$RG" -n "$ACI" --query ipAddress.fqdn -o tsv)
IP=$(az container show -g "$RG" -n "$ACI" --query ipAddress.ip -o tsv)

echo ""
echo "=========================================="
echo "✅ ACI RECRIADO"
echo "=========================================="
echo "IP:   $IP"
echo "FQDN: $FQDN"
echo ""
echo "Swagger: http://$FQDN:$PORT/swagger/index.html"
echo "=========================================="

# Testar
sleep 10
curl -i "http://$IP:$PORT/"
curl -i "http://$IP:$PORT/api/ping"
