#!/bin/bash
echo "=== Criando Azure Container Instance ==="

RESOURCE_GROUP="rg-mottoth-devops"
CONTAINER_NAME="aci-mottoth-api"
ACR_NAME="acrmottoth1234567890"  # ⚠️ SUBSTITUA pelo seu ACR!
IMAGE_NAME="mottoth-api:latest"
LOCATION="East US"

echo "================================================"
echo "VERIFICAR: ACR_NAME está correto?"
echo "Usando ACR: $ACR_NAME"
echo "Resource Group: $RESOURCE_GROUP"
echo "================================================"
read -p "Pressione ENTER para continuar ou Ctrl+C para cancelar..."

# Sua connection string Oracle existente (do appsettings.json)
ORACLE_CONNECTION="Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=oracle.fiap.com.br)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=ORCL)));User Id=rm558798;Password=fiap24;"

echo "Obtendo credenciais do ACR..."

# Obter credenciais do ACR
ACR_SERVER=$(az acr show --name $ACR_NAME --resource-group $RESOURCE_GROUP --query loginServer --output tsv 2>/dev/null)
if [ -z "$ACR_SERVER" ]; then
    echo "❌ Erro: ACR '$ACR_NAME' não encontrado!"
    echo "Verifique se o nome do ACR está correto."
    exit 1
fi

ACR_USERNAME=$(az acr credential show --name $ACR_NAME --resource-group $RESOURCE_GROUP --query username --output tsv)
ACR_PASSWORD=$(az acr credential show --name $ACR_NAME --resource-group $RESOURCE_GROUP --query passwords[0].value --output tsv)

echo "✅ Credenciais obtidas:"
echo "   Server: $ACR_SERVER"
echo "   Username: $ACR_USERNAME"

echo ""
echo "Criando Azure Container Instance..."
echo "Isso pode levar alguns minutos..."

# Criar ACI
az container create \
  --resource-group $RESOURCE_GROUP \
  --name $CONTAINER_NAME \
  --image "$ACR_SERVER/$IMAGE_NAME" \
  --registry-login-server $ACR_SERVER \
  --registry-username $ACR_USERNAME \
  --registry-password $ACR_PASSWORD \
  --dns-name-label mottoth-api-$(date +%s) \
  --ports 8080 \
  --environment-variables \
    ASPNETCORE_ENVIRONMENT=Production \
    ConnectionStrings__OracleConnection="$ORACLE_CONNECTION" \
  --cpu 1 \
  --memory 1.5 \
  --location "$LOCATION" \
  --tags projeto="mottoth-sprint3"

if [ $? -eq 0 ]; then
    echo ""
    echo "Obtendo URL do container..."
    
    # Obter URL final
    CONTAINER_FQDN=$(az container show \
      --resource-group $RESOURCE_GROUP \
      --name $CONTAINER_NAME \
      --query ipAddress.fqdn \
      --output tsv)
    
    echo ""
    echo "✅ Container criado com sucesso!"
    echo "================================================"
    echo "🌐 URL da API: http://$CONTAINER_FQDN:8080"
    echo "📊 Swagger: http://$CONTAINER_FQDN:8080/swagger" 
    echo "❤️ Health: http://$CONTAINER_FQDN:8080/health"
    echo "👥 Usuários: http://$CONTAINER_FQDN:8080/api/usuarios"
    echo "================================================"
    echo ""
    echo "⚠️  PRÓXIMO PASSO:"
    echo "   Edite o arquivo 05-test-api.sh"
    echo "   Substitua: API_URL=\"http://mottoth-api-1234567890.eastus.azurecontainer.io:8080\""
    echo "   Por:       API_URL=\"http://$CONTAINER_FQDN:8080\""
    echo ""
    echo "Aguarde cerca de 2-3 minutos para o container inicializar completamente."
else
    echo "❌ Erro na criação do container! Verifique os logs acima."
    exit 1
fi