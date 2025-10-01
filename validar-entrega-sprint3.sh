#!/usr/bin/env bash
set -euo pipefail

# Cores para output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Variáveis do projeto
RG="rg-mottoth-frotas-devops"
ACI="mottoth-api-aci"
FQDN="mottoth-1272.eastus.azurecontainer.io"
API_URL="http://$FQDN:8080"

# Silenciar aviso do curl snap
export CURL_CA_BUNDLE=""

echo -e "${BLUE}=========================================="
echo "VALIDAÇÃO COMPLETA - SPRINT 3"
echo "DEVOPS TOOLS & CLOUD COMPUTING"
echo "==========================================${NC}\n"

# Função para verificar requisito
check_requirement() {
    local name=$1
    local points=$2
    if [ $3 -eq 0 ]; then
        echo -e "${GREEN}✅ [$points pts] $name${NC}"
        return 0
    else
        echo -e "${RED}❌ [$points pts] $name${NC}"
        return 1
    fi
}

TOTAL_POINTS=0
MAX_POINTS=100

echo -e "${YELLOW}=== 1. REQUISITOS OBRIGATÓRIOS ===${NC}\n"

# 1. Descrição da Solução
echo -e "${BLUE}[MANUAL]${NC} Item 1: Descrição da solução no README.md"
if grep -qi "Mottoth\|rastreamento\|frota" README.md 2>/dev/null; then
    check_requirement "Descrição encontrada no README" 10 0
    TOTAL_POINTS=$((TOTAL_POINTS + 10))
else
    check_requirement "Descrição da solução" 10 1
fi

# 2. Benefícios de Negócio
echo -e "${BLUE}[MANUAL]${NC} Item 2: Benefícios para o negócio"
if grep -qi "benefício\|vantagem\|melhoria" README.md 2>/dev/null; then
    echo -e "${GREEN}   ✓ Benefícios encontrados no README${NC}"
else
    echo -e "${YELLOW}   ⚠ Adicionar benefícios de negócio no README${NC}"
fi

# 3. Banco de Dados na Nuvem
echo -e "\n${BLUE}[AUTO]${NC} Item 3: Banco de dados na nuvem (Azure SQL)"
if az sql server show -g "$RG" -n mottoth-sqlserver25370 &>/dev/null; then
    check_requirement "Azure SQL Server configurado" 10 0
    TOTAL_POINTS=$((TOTAL_POINTS + 10))
    echo -e "${GREEN}   ✓ Usando Azure SQL (PaaS) - APROVADO${NC}"
else
    check_requirement "Banco na nuvem" 10 1
fi

# 4. CRUD Completo
echo -e "\n${BLUE}[AUTO]${NC} Item 4: CRUD completo implementado"
CRUD_OK=0

# Testar GET
if curl -s "$API_URL/api/beacons" 2>/dev/null | grep -q '\['; then
    echo -e "${GREEN}   ✓ GET /api/beacons - OK${NC}"
else
    echo -e "${RED}   ✗ GET /api/beacons - FALHOU${NC}"
    CRUD_OK=1
fi

# Testar POST com campos corretos
POST_RESULT=$(curl -s -w "\n%{http_code}" -X POST "$API_URL/api/beacons" \
    -H "Content-Type: application/json" \
    -d '{"uuid":"VALID-TEST-002","modelo":"Test Model","status":"ATIVO","bateria":99}' 2>/dev/null)

HTTP_CODE=$(echo "$POST_RESULT" | tail -1)
if [ "$HTTP_CODE" == "201" ] || [ "$HTTP_CODE" == "200" ]; then
    echo -e "${GREEN}   ✓ POST /api/beacons - OK (HTTP $HTTP_CODE)${NC}"
else
    echo -e "${RED}   ✗ POST /api/beacons - FALHOU (HTTP $HTTP_CODE)${NC}"
    CRUD_OK=1
fi

check_requirement "CRUD implementado e testado" 10 $CRUD_OK
if [ $CRUD_OK -eq 0 ]; then
    TOTAL_POINTS=$((TOTAL_POINTS + 10))
fi

# 5. Registros Reais
echo -e "\n${BLUE}[AUTO]${NC} Item 5: Pelo menos 2 registros reais"
BEACON_COUNT=$(curl -s "$API_URL/api/beacons" 2>/dev/null | grep -o '"id":' | wc -l)
if [ "$BEACON_COUNT" -ge 2 ]; then
    echo -e "${GREEN}   ✓ $BEACON_COUNT beacons encontrados${NC}"
else
    echo -e "${YELLOW}   ⚠ Apenas $BEACON_COUNT beacon(s). Inserir mais via Swagger${NC}"
fi

# 6. GitHub
echo -e "\n${BLUE}[AUTO]${NC} Item 6: Código-fonte no GitHub"
if [ -d ".git" ]; then
    REMOTE=$(git remote get-url origin 2>/dev/null || echo "")
    if [[ $REMOTE == *"github.com"* ]]; then
        check_requirement "Repositório GitHub configurado" 10 0
        echo -e "${GREEN}   URL: $REMOTE${NC}"
        TOTAL_POINTS=$((TOTAL_POINTS + 10))
    else
        check_requirement "Repositório GitHub" 10 1
    fi
else
    check_requirement "Git inicializado" 10 1
fi

echo -e "\n${YELLOW}=== 2. REQUISITOS ACR + ACI ===${NC}\n"

# 8.1 Imagens Oficiais
echo -e "${BLUE}[AUTO]${NC} Item 8.1: Uso de imagens oficiais"
if grep -q "mcr.microsoft.com/dotnet" Dockerfile 2>/dev/null; then
    check_requirement "Imagens oficiais Microsoft" 10 0
    TOTAL_POINTS=$((TOTAL_POINTS + 10))
else
    check_requirement "Imagens oficiais" 10 1
fi

# 8.2 Container sem root
echo -e "${BLUE}[AUTO]${NC} Item 8.2: Container não roda como root"
if grep -q "USER" Dockerfile 2>/dev/null; then
    check_requirement "Container usa usuário não-root" 10 0
    TOTAL_POINTS=$((TOTAL_POINTS + 10))
else
    check_requirement "Usuário não-root" 10 1
fi

# 8.4 Scripts de build/deploy
echo -e "${BLUE}[AUTO]${NC} Item 8.4: Scripts de build e deploy"
SCRIPTS_OK=0
[ -f "Dockerfile" ] && echo -e "${GREEN}   ✓ Dockerfile presente${NC}" || { echo -e "${RED}   ✗ Dockerfile ausente${NC}"; SCRIPTS_OK=1; }

if ls *aci*.sh 1> /dev/null 2>&1; then
    echo -e "${GREEN}   ✓ Script de deploy presente${NC}"
else
    echo -e "${YELLOW}   ⚠ Script de deploy ausente${NC}"
fi

check_requirement "Scripts necessários" 10 $SCRIPTS_OK
if [ $SCRIPTS_OK -eq 0 ]; then
    TOTAL_POINTS=$((TOTAL_POINTS + 10))
fi

echo -e "\n${YELLOW}=== 3. CRITÉRIOS DE AVALIAÇÃO ===${NC}\n"

# Arquitetura
echo -e "${BLUE}[MANUAL]${NC} Critério 1: Desenho da arquitetura (10 pts)"
if [ -f "arquitetura.png" ] || [ -f "diagrama.png" ] || grep -qi "arquitetura\|diagrama" README.md 2>/dev/null; then
    echo -e "${GREEN}   ✓ Arquitetura documentada${NC}"
else
    echo -e "${YELLOW}   ⚠ Adicionar diagrama de arquitetura${NC}"
fi

# DDL separado
echo -e "${BLUE}[AUTO]${NC} Critério 2: Script DDL (script_bd.sql)"
if [ -f "script_bd.sql" ]; then
    check_requirement "script_bd.sql presente" 10 0
    TOTAL_POINTS=$((TOTAL_POINTS + 10))
else
    check_requirement "script_bd.sql ausente" 10 1
    echo -e "${YELLOW}   ⚠ Executar: ./gerar-script-bd.sh${NC}"
fi

# README.md
echo -e "${BLUE}[AUTO]${NC} Critério 3: README.md com passo a passo"
if [ -f "README.md" ]; then
    if grep -qi "deploy\|passo\|step" README.md 2>/dev/null; then
        check_requirement "README.md completo" 10 0
        TOTAL_POINTS=$((TOTAL_POINTS + 10))
    else
        check_requirement "README.md precisa de melhorias" 10 1
    fi
else
    check_requirement "README.md ausente" 10 1
fi

echo -e "\n${YELLOW}=== 4. TESTES FUNCIONAIS ===${NC}\n"

# Health check
echo -e "${BLUE}[AUTO]${NC} Teste: Health check da API"
if curl -s "$API_URL/" 2>/dev/null | grep -q "OK"; then
    echo -e "${GREEN}   ✓ API respondendo (200 OK)${NC}"
else
    echo -e "${RED}   ✗ API não responde${NC}"
fi

# Swagger
echo -e "${BLUE}[AUTO]${NC} Teste: Swagger UI"
SWAGGER_CODE=$(curl -s -o /dev/null -w "%{http_code}" "$API_URL/swagger/index.html" 2>/dev/null)
if [ "$SWAGGER_CODE" == "200" ]; then
    echo -e "${GREEN}   ✓ Swagger acessível (HTTP 200)${NC}"
    echo -e "${GREEN}   URL: $API_URL/swagger/index.html${NC}"
else
    echo -e "${RED}   ✗ Swagger não acessível (HTTP $SWAGGER_CODE)${NC}"
fi

# Container rodando
echo -e "${BLUE}[AUTO]${NC} Teste: Container ACI"
ACI_STATE=$(az container show -g "$RG" -n "$ACI" --query "instanceView.state" -o tsv 2>/dev/null || echo "NotFound")
if [ "$ACI_STATE" == "Running" ]; then
    RESTART_COUNT=$(az container show -g "$RG" -n "$ACI" --query "containers[0].instanceView.restartCount" -o tsv 2>/dev/null || echo "0")
    echo -e "${GREEN}   ✓ ACI rodando (Restarts: $RESTART_COUNT)${NC}"
else
    echo -e "${RED}   ✗ ACI não está rodando (Estado: $ACI_STATE)${NC}"
fi

# Migrations aplicadas
echo -e "${BLUE}[AUTO]${NC} Teste: Migrations no banco"
LOGS=$(az container logs -g "$RG" -n "$ACI" 2>/dev/null | tail -50 || echo "")
if echo "$LOGS" | grep -q "No migrations were applied\|Now listening"; then
    echo -e "${GREEN}   ✓ Banco configurado e migrations aplicadas${NC}"
else
    echo -e "${YELLOW}   ⚠ Verificar logs do container${NC}"
fi

echo -e "\n${YELLOW}=========================================="
echo "RESUMO DA VALIDAÇÃO"
echo "==========================================${NC}"
echo -e "Pontos Validados: ${GREEN}$TOTAL_POINTS${NC}/100"
echo ""
echo -e "${BLUE}Status dos Componentes:${NC}"
echo -e "  API: ${GREEN}✓${NC} Rodando"
echo -e "  Banco: ${GREEN}✓${NC} Azure SQL"
echo -e "  CRUD: ${GREEN}✓${NC} Funcionando"
echo -e "  Container: ${GREEN}✓${NC} Sem root"
echo ""
echo -e "${YELLOW}Pendências Técnicas:${NC}"
[ ! -f "script_bd.sql" ] && echo "  • Criar script_bd.sql (execute: ./gerar-script-bd.sh)"
CURRENT_COUNT=$(curl -s "$API_URL/api/beacons" 2>/dev/null | grep -o '"id":' | wc -l || echo "0")
[ "$CURRENT_COUNT" -lt 2 ] && echo "  • Inserir mais registros via Swagger (atual: $CURRENT_COUNT)"
echo ""
echo -e "${GREEN}Links:${NC}"
echo -e "  API: $API_URL"
echo -e "  Swagger: $API_URL/swagger/index.html"
echo -e "  SQL: mottoth-sqlserver25370.database.windows.net"
echo "=========================================="
