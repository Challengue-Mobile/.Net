#!/bin/bash
echo "=== Criando Azure Container Registry ==="

RESOURCE_GROUP="rg-mottoth-devops"
ACR_NAME="acrmottoth$(date +%s)"
LOCATION="East US"

echo "Criando ACR: $ACR_NAME"

az acr create \
  --resource-group $RESOURCE_GROUP \
  --name $ACR_NAME \
  --sku Basic \
  --location "$LOCATION" \
  --admin-enabled true \
  --tags projeto="mottoth-sprint3"

echo "Obtendo credenciais..."

# Obter credenciais
ACR_SERVER=$(az acr show --name $ACR_NAME --resource-group $RESOURCE_GROUP --query loginServer --output tsv)
ACR_USERNAME=$(az acr credential show --name $ACR_NAME --resource-group $RESOURCE_GROUP --query username --output tsv)
ACR_PASSWORD=$(az acr credential show --name $ACR_NAME --resource-group $RESOURCE_GROUP --query passwords[0].value --output tsv)

echo ""
echo "✅ ACR criado com sucesso!"
echo "================================================"
echo "ACR Name: $ACR_NAME"
echo "Server: $ACR_SERVER"
echo "Username: $ACR_USERNAME"  
echo "Password: $ACR_PASSWORD"
echo "================================================"
echo ""
echo "⚠️  COPIE E COLE o ACR_NAME no próximo script!"
echo "   Edite o arquivo 03-build-push.sh"
echo "   Substitua: ACR_NAME=\"acrmottoth1234567890\""
echo "   Por:       ACR_NAME=\"$ACR_NAME\""
echo ""

# Login no ACR
echo "Fazendo login no ACR..."
az acr login --name $ACR_NAME

echo "✅ Login realizado! Pode executar o próximo script."