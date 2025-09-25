#!/bin/bash
echo "=== Testando API MottothTracking ==="

# ⚠️ SUBSTITUA pela URL do seu container (do script anterior)
API_URL="http://mottoth-api-1234567890.eastus.azurecontainer.io:8080"

echo "================================================"
echo "VERIFICAR: URL está correta?"
echo "Testando URL: $API_URL"
echo "================================================"
read -p "Pressione ENTER para continuar ou Ctrl+C para editar..."

echo ""
echo "🧪 Iniciando testes da API..."
echo ""

echo "1. Health Check:"
echo "GET $API_URL/health"
curl -X GET "$API_URL/health" -H "accept: application/json" -w "\n" --connect-timeout 10
echo ""

echo "2. Endpoint raiz:"
echo "GET $API_URL/"
curl -X GET "$API_URL/" -H "accept: application/json" -w "\n" --connect-timeout 10
echo ""

echo "3. Listar Usuários (CRUD - READ):"
echo "GET $API_URL/api/usuarios"
curl -X GET "$API_URL/api/usuarios" -H "accept: application/json" -w "\n" --connect-timeout 10
echo ""

echo "4. Criar Usuário (CRUD - CREATE):"
echo "POST $API_URL/api/usuarios"
CURRENT_DATE=$(date -u +"%Y-%m-%dT%H:%M:%S.000Z")
curl -X POST "$API_URL/api/usuarios" \
  -H "accept: application/json" \
  -H "Content-Type: application/json" \
  -d "{
    \"nome\": \"Teste Sprint3 $(date +%H%M%S)\",
    \"email\": \"teste$(date +%s)@sprint3.com\", 
    \"senha\": \"123456\",
    \"dataCadastro\": \"$CURRENT_DATE\"
  }" \
  -w "\n" --connect-timeout 10
echo ""

echo "5. Listar Usuários novamente (verificar se foi criado):"
echo "GET $API_URL/api/usuarios"
curl -X GET "$API_URL/api/usuarios" -H "accept: application/json" -w "\n" --connect-timeout 10
echo ""

echo "6. Buscar usuário por email:"
echo "GET $API_URL/api/usuarios/ByEmail/joao@teste.com"
curl -X GET "$API_URL/api/usuarios/ByEmail/joao@teste.com" -H "accept: application/json" -w "\n" --connect-timeout 10
echo ""

echo "================================================"
echo "✅ Testes concluídos!"
echo ""
echo "📋 URLs importantes para o vídeo:"
echo "   🌐 API Principal: $API_URL/api/usuarios"
echo "   📊 Swagger UI:    $API_URL/swagger"
echo "   ❤️  Health Check:  $API_URL/health"
echo ""
echo "🎬 Para o vídeo, use estes comandos CURL:"
echo ""
echo "# CREATE (criar usuário)"
echo "curl -X POST \"$API_URL/api/usuarios\" \\"
echo "  -H \"Content-Type: application/json\" \\"
echo "  -d '{\"nome\":\"João Silva\",\"email\":\"joao@demo.com\",\"senha\":\"123456\"}'"
echo ""
echo "# READ (listar usuários)"
echo "curl -X GET \"$API_URL/api/usuarios\""
echo ""
echo "# UPDATE (atualizar usuário ID 1)"
echo "curl -X PUT \"$API_URL/api/usuarios/1\" \\"
echo "  -H \"Content-Type: application/json\" \\"
echo "  -d '{\"id\":1,\"nome\":\"João Silva Santos\",\"email\":\"joao@demo.com\",\"senha\":\"123456\"}'"
echo ""
echo "# DELETE (excluir usuário ID 1)"
echo "curl -X DELETE \"$API_URL/api/usuarios/1\""
echo "================================================"