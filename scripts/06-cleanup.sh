#!/bin/bash
echo "⚠️  ATENÇÃO: Este script remove TODOS os recursos Azure!"
echo "Recursos que serão removidos:"
echo "  - Resource Group: rg-mottoth-devops"
echo "  - Azure Container Registry (ACR)"
echo "  - Azure Container Instance (ACI)"
echo "  - Todos os dados e configurações"
echo ""
echo "❌ ESTA OPERAÇÃO NÃO PODE SER DESFEITA!"
echo ""

RESOURCE_GROUP="rg-mottoth-devops"

# Verificar se o resource group existe
echo "Verificando se o resource group existe..."
az group show --name $RESOURCE_GROUP --output table 2>/dev/null

if [ $? -ne 0 ]; then
    echo "✅ Resource group '$RESOURCE_GROUP' não encontrado."
    echo "Nada para remover."
    exit 0
fi

echo ""
echo "📋 Recursos encontrados no grupo:"
az resource list --resource-group $RESOURCE_GROUP --output table

echo ""
echo "⚠️  Confirme que deseja EXCLUIR TUDO acima!"
read -p "Digite 'EXCLUIR TUDO' para confirmar: " confirm

if [[ $confirm == "EXCLUIR TUDO" ]]; then
    echo ""
    echo "🗑️  Iniciando remoção do grupo de recursos..."
    echo "Isso pode levar vários minutos..."
    
    # Mostrar progresso
    echo "Comando executado:"
    echo "az group delete --name $RESOURCE_GROUP --yes --no-wait"
    
    az group delete \
      --name $RESOURCE_GROUP \
      --yes \
      --no-wait
    
    echo ""
    echo "✅ Processo de remoção iniciado em background"
    echo ""
    echo "📊 Para acompanhar o progresso:"
    echo "   Portal Azure: https://portal.azure.com"
    echo "   Ou comando: az group show --name $RESOURCE_GROUP"
    echo ""
    echo "⏱️  O processo pode levar de 5 a 15 minutos."
    echo "Você receberá uma notificação quando concluído."
    
else
    echo ""
    echo "❌ Operação cancelada pelo usuário"
    echo "✅ Todos os recursos Azure foram mantidos"
    echo ""
    echo "💡 Para acessar seus recursos:"
    echo "   Portal: https://portal.azure.com"
    echo "   Resource Group: $RESOURCE_GROUP"
fi