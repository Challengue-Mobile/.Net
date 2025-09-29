#!/usr/bin/env bash
# deploy_all.sh — MottothTracking Sprint 3 (ACR + ACI)
# Uso:
#   chmod +x deploy_all.sh
#   ./deploy_all.sh
# Personalize as variáveis da seção CONFIG se precisar.

set -euo pipefail

# ============================
#          CONFIG
# ============================
RG="${RG:-rg-mottoth-frotas-devops}"
LOCATION="${LOCATION:-eastus}"

# Seu ACR (NOME, sem .azurecr.io)
ACR="${ACR:-acrmottoth1759096657}"

# Nome/tag da imagem
REPO="${REPO:-mottoth-api}"
TAG="${TAG:-latest}"
FULL_IMAGE="${ACR}.azurecr.io/${REPO}:${TAG}"

# Nome do Container Group no ACI
APP="${APP:-mottoth-api-aci}"

# Porta exposta pela API
PORT="${PORT:-8080}"

# Oracle FIAP (ajuste se necessário)
SRV="${SRV:-oracle.fiap.com.br}"
USR="${USR:-rm558798}"
PWD="${PWD:-fiap24}"
CONN="Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=${SRV})(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=ORCL)));User Id=${USR};Password=${PWD};"

# Caminhos
ROOT_DIR="$(pwd)"
DOCS_SQL="${ROOT_DIR}/script_bd.sql"
README="${ROOT_DIR}/README.md"
DOCKERFILE="${ROOT_DIR}/Dockerfile"

banner() { echo -e "\n\033[1;36m[INFO]\033[0m $*"; }
warn()   { echo -e "\033[1;33m[AVISO]\033[0m $*"; }
err()    { echo -e "\033[1;31m[ERRO]\033[0m $*" >&2; }

banner "Sprint3 All-in-One — MottothTracking"
echo "[CTX] RG=${RG} | LOCATION=${LOCATION}"
echo "[CTX] ACR=${ACR} | IMAGE=${FULL_IMAGE}"
echo "[CTX] APP=${APP} | PORT=${PORT}"
echo "[CTX] Oracle: ${USR}@${SRV} (SERVICE_NAME=ORCL)"

# ============================
#   PRE-CHECKS
# ============================
command -v az >/dev/null || { err "Azure CLI não encontrado"; exit 1; }
command -v docker >/dev/null || { err "Docker não encontrado"; exit 1; }

# Garante docker rodando
if ! docker ps >/dev/null 2>&1; then
  warn "Docker parece parado. Tentando iniciar via systemd…"
  sudo systemctl start docker || true
  sleep 3
  docker ps >/dev/null || { err "Docker não iniciou."; exit 1; }
fi

# ============================
#   GERAR/ATUALIZAR ARQUIVOS
# ============================
banner "Gerando/atualizando script_bd.sql"
cat > "${DOCS_SQL}" <<'SQL'
-- script_bd.sql | Sprint 3 (DevOps & Cloud)
-- DDL mínimo para demonstrar CRUD (Oracle)

-- DROP TABLE BEACONS CASCADE CONSTRAINTS;

CREATE TABLE BEACONS (
  ID          NUMBER         PRIMARY KEY,
  CODIGO      VARCHAR2(50)   NOT NULL,
  DESCRICAO   VARCHAR2(200),
  CREATED_AT  DATE           DEFAULT SYSDATE
);

COMMENT ON TABLE BEACONS IS 'Tabela de beacons (rastreador) para demo CRUD';
COMMENT ON COLUMN BEACONS.ID IS 'Identificador único';
COMMENT ON COLUMN BEACONS.CODIGO IS 'Código do beacon';
COMMENT ON COLUMN BEACONS.DESCRICAO IS 'Descrição';
COMMENT ON COLUMN BEACONS.CREATED_AT IS 'Data de criação';

-- Inserções mínimas (exigência de 2 registros):
INSERT INTO BEACONS (ID, CODIGO, DESCRICAO) VALUES (1, 'B001', 'Beacon moto X');
INSERT INTO BEACONS (ID, CODIGO, DESCRICAO) VALUES (2, 'B002', 'Beacon moto Y');
COMMIT;
SQL

# README só cria se não existir (não sobrescreve seu conteúdo)
if [[ ! -f "${README}" ]]; then
  banner "Criando README.md"
  cat > "${README}" <<'MD'
# MottothTracking – Sprint 3 (DevOps & Cloud)

## Descrição
API .NET (Swagger) em Docker, imagem no **Azure Container Registry (ACR)** e execução no **Azure Container Instances (ACI)**.
Banco em nuvem: **Oracle FIAP**.

## Benefícios
- Visibilidade de ativos e rastreio (beacons/motos)
- Base para operações/relatórios
- Deploy rápido e reprodutível em nuvem

## Arquitetura
Dev → Docker build → Push no ACR → ACI puxa a imagem e expõe :8080.
Connection string Oracle via env `ConnectionStrings__OracleConnection`.

## Banco
Arquivo **script_bd.sql** (DDL + inserts). Conexão Oracle FIAP.

## Teste rápido (após deploy)
BASE="http://<IP-OU-FQDN>:8080"
curl -i "$BASE/" # 200 "MottothTracking API OK"
curl -i "$BASE/api/ping" # 200 {"pong":true,...}

Swagger UI:
$BASE/swagger/index.html
Swagger JSON:
curl -i "$BASE/swagger/v1/swagger.json"

bash
Copy code

## Entrega
- Código no GitHub + README
- `script_bd.sql`
- Vídeo (deploy e CRUD)
- PDF com nome/RM e links
MD
else
  banner "Mantendo README.md existente"
fi

# Gera Dockerfile mínimo se faltar (não sobrescreve se já existir)
if [[ ! -f "${DOCKERFILE}" ]]; then
  banner "Gerando Dockerfile padrão (.NET 9)"
  cat > "${DOCKERFILE}" <<'DOCK'
# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app
COPY *.csproj ./
RUN dotnet restore
COPY . ./
RUN dotnet publish -c Release -o /out

# Etapa de runtime (sem root)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
RUN addgroup --system mottothuser && adduser --system --ingroup mottothuser mottothuser \
    && chown -R mottothuser:mottothuser /app
USER mottothuser
COPY --from=build /out ./
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "MottothTracking.dll"]
DOCK
else
  banner "Usando Dockerfile existente"
fi

# ============================
#   BUILD + PUSH NO ACR
# ============================
banner "Build da imagem: ${FULL_IMAGE}"
docker build -t "${FULL_IMAGE}" .

banner "Login no ACR (token refresh)…"
TOKEN="$(az acr login -n "${ACR}" --expose-token --query accessToken -o tsv)"
echo "${TOKEN}" | docker login "${ACR}.azurecr.io" -u 00000000-0000-0000-0000-000000000000 --password-stdin

banner "Push para o ACR"
docker push "${FULL_IMAGE}"

# ============================
#   RECRIAR ACI
# ============================
banner "Removendo ACI anterior (se existir): ${APP}"
az container delete -g "${RG}" -n "${APP}" --yes || true

banner "Pegando credenciais do ACR"
USER_AZ="$(az acr credential show -n "${ACR}" --query username -o tsv)"
PASS_AZ="$(az acr credential show -n "${ACR}" --query passwords[0].value -o tsv)"

banner "Criando ACI '${APP}' em ${LOCATION}"
az container create \
  -g "${RG}" -n "${APP}" -l "${LOCATION}" \
  --image "${FULL_IMAGE}" \
  --os-type Linux --cpu 1 --memory 1.5 \
  --ports "${PORT}" --ip-address Public --restart-policy OnFailure \
  --dns-name-label "mottoth-$RANDOM" \
  --registry-login-server "${ACR}.azurecr.io" \
  --registry-username "${USER_AZ}" \
  --registry-password "${PASS_AZ}" \
  --environment-variables ASPNETCORE_ENVIRONMENT=Production ASPNETCORE_URLS="http://+:${PORT}" \
  --secure-environment-variables ConnectionStrings__OracleConnection="${CONN}"

banner "Aguardando IP/FQDN…"
sleep 8
IP="$(az container show -g "${RG}" -n "${APP}" --query "ipAddress.ip" -o tsv || echo "")"
FQDN="$(az container show -g "${RG}" -n "${APP}" --query "ipAddress.fqdn" -o tsv || echo "")"

if [[ -z "${IP}" || "${IP}" == "null" ]]; then
  err "Não foi possível obter IP do ACI. Detalhes:"
  az container show -g "${RG}" -n "${APP}"
  exit 1
fi

BASE="http://${IP}:${PORT}"
echo
banner "Endpoint público"
printf "Ip             Fqdn\n%-14s %s\n" "${IP}" "${FQDN}"

# ============================
#   TESTES RÁPIDOS
# ============================
echo
banner "Testes (cole no terminal para validar)"
cat <<EOF
# Health
curl -i '${BASE}/'

# Ping (min API)
curl -i '${BASE}/api/ping'

# Swagger UI (abra no navegador)
#   ${BASE}/swagger/index.html

# Swagger JSON
curl -i '${BASE}/swagger/v1/swagger.json'
EOF

echo
banner "SCRIPT CONCLUÍDO. 🚀  (README e script_bd.sql prontos)"