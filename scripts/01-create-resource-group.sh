echo "=== Criando Grupo de Recursos para MottothTracking ==="

RESOURCE_GROUP="rg-mottoth-frotas-devops"
LOCATION="East US"

az group create \
  --name $RESOURCE_GROUP \
  --location "$LOCATION" \
  --tags projeto="mottoth-sprint3" disciplina="devops-cloud" equipe="MottothTracking"

echo "✅ Grupo de recursos criado: $RESOURCE_GROUP"