#!/usr/bin/env bash
set -euo pipefail

RG="rg-mottoth-frotas-devops"
LOC="eastus"
ACI="mottoth-api-aci"
ACR="acrmottoth1759096657"
IMAGE="$ACR.azurecr.io/mottoth-api:latest"
CPU=1
MEMORY=1.5
PORT=8080
DNS_LABEL="mottoth-$RANDOM"

# Connection string (SEM caracteres especiais problemáticos)
CONN_STRING="Server=tcp://mottoth-sqlserver25370.database.windows.net,1433;Initial Catalog=mottothdb;User ID=sqladmin;Password=NovaSenhaSegura#2025;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"

echo "[INFO] Deletando ACI com problema..."
az container delete -g "$RG" -n "$ACI" --yes
echo "[INFO] Aguardando exclusão..."
sleep 20

echo "[INFO] Criando ACI com connection string correta..."
az container create \
  -g "$RG" \
  -n "$ACI" \
  -l "$LOC" \
  --image "$IMAGE" \
  --os-type Linux \
  --cpu $CPU \
  --memory $MEMORY \
  --ports $PORT \
  --ip-address Public \
  --restart-policy Always \
  --dns-name-label "$DNS_LABEL" \
  --registry-login-server "$ACR.azurecr.io" \
  --registry-username "$(az acr credential show -n $ACR --query username -o tsv)" \
  --registry-password "$(az acr credential show -n $ACR --query passwords[0].value -o tsv)" \
  --environment-variables \
    ASPNETCORE_ENVIRONMENT=Production \
    ASPNETCORE_URLS=http://+:8080 \
    "ConnectionStrings__Default=$CONN_STRING"

echo "[INFO] Aguardando 30 segundos para inicialização..."
sleep 30

echo "[INFO] Verificando logs..."
az container logs -g "$RG" -n "$ACI" --tail 50

echo ""
echo "[INFO] Estado do container:"
az container show -g "$RG" -n "$ACI" --query "{State:instanceView.state, RestartCount:containers[0].instanceView.restartCount, DetailStatus:containers[0].instanceView.currentState.detailStatus}" -o table

FQDN=$(az container show -g "$RG" -n "$ACI" --query ipAddress.fqdn -o tsv)
IP=$(az container show -g "$RG" -n "$ACI" --query ipAddress.ip -o tsv)

echo ""
echo "=========================================="
echo "IP:   $IP"
echo "FQDN: $FQDN"
echo "=========================================="
echo "Teste: curl http://$IP:$PORT/"
