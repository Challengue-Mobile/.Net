#!/bin/bash
echo "=== Build e Push da Imagem Docker ==="

# ⚠️ SUBSTITUA PELO NOME DO SEU ACR (do script anterior)
ACR_NAME="acrmottoth1759096657"  # ← SUBSTITUA AQUI!
IMAGE_NAME="mottoth-api"
IMAGE_TAG="latest"

echo "================================================"
echo "VERIFICAR: ACR_NAME está correto?"
echo "Usando ACR: $ACR_NAME"
echo "================================================"
read -p "Pressione ENTER para continuar ou Ctrl+C para cancelar..."

echo "Fazendo login no ACR..."
az acr login --name $ACR_NAME

if [ $? -ne 0 ]; then
    echo "❌ Erro no login! Verifique se o ACR_NAME está correto."
    exit 1
fi

echo "Verificando se o Dockerfile existe..."
if [ ! -f "Dockerfile" ]; then
    echo "❌ Dockerfile não encontrado na pasta atual!"
    echo "Certifique-se de estar na raiz do projeto MottothTracking."
    exit 1
fi

echo "✅ Dockerfile encontrado!"
echo "Iniciando build da imagem no Azure..."
echo "Isso pode levar alguns minutos..."

# Build da imagem direto no ACR (mais eficiente)
az acr build \
  --registry $ACR_NAME \
  --image $IMAGE_NAME:$IMAGE_TAG \
  --file Dockerfile \
  .

if [ $? -eq 0 ]; then
    echo ""
    echo "✅ Build e push concluídos com sucesso!"
    echo "================================================"
    echo "Imagem disponível em:"
    echo "$ACR_NAME.azurecr.io/$IMAGE_NAME:$IMAGE_TAG"
    echo "================================================"
    echo ""
    echo "⚠️  PRÓXIMO PASSO:"
    echo "   Edite o arquivo 04-create-aci.sh"
    echo "   Substitua: ACR_NAME=\"acrmottoth1234567890\""
    echo "   Por:       ACR_NAME=\"$ACR_NAME\""
    echo ""
else
    echo "❌ Erro no build! Verifique os logs acima."
    exit 1
fi