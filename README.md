# 🚀 MottothTracking — Sistema de Rastreamento de Frotas

Sistema empresarial para rastreamento e gestão de frotas de motocicletas com dispositivos beacon.

**Stack:** .NET 9, Docker, Azure (ACR + ACI) e Oracle Database (FIAP).

## 💼 Benefícios para o Negócio

O MottothTracking resolve problemas críticos na gestão de frotas de motocicletas através de rastreamento em tempo real com dispositivos beacon:

- **Otimização de Logística:** Monitoramento preciso da localização e status de cada veículo, permitindo melhor planejamento de rotas e distribuição de entregas
- **Redução de Perdas:** Rastreamento contínuo reduz riscos de furto, extravio e uso não autorizado dos veículos
- **Melhor Rastreabilidade:** Histórico completo de movimentações facilita auditorias, análises de desempenho e tomada de decisões baseada em dados
- **Manutenção Preventiva:** Controle centralizado permite identificar padrões de uso e programar manutenções antes de falhas críticas
- **Conformidade e Segurança:** Registro auditável de todas as operações garante compliance com regulamentações e políticas internas

## ℹ️ Status atual do ambiente

A aplicação está publicada em ACI e responde aos endpoints de health (`/`) e ping (`/api/ping`).

As rotas CRUD com Oracle podem falhar enquanto o usuário institucional estiver bloqueado (ORA-28000: account locked). O projeto já inclui script SQL e mapeamentos; basta re-deploy quando o acesso ao Oracle for liberado.

## 🧭 Sumário

- [Arquitetura](#-arquitetura)
- [Estrutura do Repositório](#-estrutura-do-repositório)
- [Pré-requisitos](#-pré-requisitos)
- [Como subir tudo de uma vez (All-in-One)](#-como-subir-tudo-de-uma-vez-all-in-one)
- [Testes rápidos (curl)](#-testes-rápidos-curl)
- [Endpoints principais](#-endpoints-principais)
- [Banco de Dados (Oracle)](#-banco-de-dados-oracle)
- [Execução local](#-execução-local)
- [Limpeza de recursos](#-limpeza-de-recursos)
- [Solução de problemas (FAQ)](#-solução-de-problemas-faq)
- [Checklist de Entrega](#-checklist-de-entrega)
- [Créditos](#-créditos)

## 🏗️ Arquitetura

```
GitHub → Dockerfile → ACR (Azure Container Registry) → ACI (Azure Container Instances) → Oracle FIAP
                           |                                    |
                        Imagem                            API exposta
```

- **ACR:** armazena a imagem Docker.
- **ACI:** executa a imagem (container) e publica a porta 8080 com IP/FQDN público.
- **Oracle:** base de dados acadêmica (FIAP).
- **Swagger:** interface e documentação da API.

## 📂 Estrutura do Repositório

```
MottothTracking/
├── API/                 # Controllers e lógica de API
├── Data/                # DbContext e repositórios
├── DTOs/                # Objetos de transferência
├── Models/              # Entidades do domínio
├── Migrations/          # Histórico EF Core
├── scripts/             # Shell scripts de build/deploy (Azure + Docker)
│   ├── 01-build-push.sh
│   ├── 02-deploy-aci.sh
│   ├── 03-test-aci.sh
│   ├── 04-run-local.sh
│   └── 06-teste-total.sh
├── docs/                # Documentação e script SQL
│   └── script_bd.sql
├── Dockerfile           # Build multi-stage (.NET 9)
├── .dockerignore
├── appsettings.json     # Configuração local
├── Program.cs
├── MottothTracking.csproj
└── README.md
```

## ✅ Pré-requisitos

- **Azure CLI** autenticada:
  ```bash
  az login
  az account show
  ```

- **Docker** instalado e em execução.

- **.NET SDK 9** (para builds locais, se necessário).

## ⚡ Como subir tudo de uma vez (All-in-One)

O repositório contém o script `06-teste-total.sh`, que faz:

1. Gera/atualiza o script do banco (`scripts/script_bd.sql`).
2. Build da imagem Docker e push para o ACR.
3. (Re)cria o ACI apontando para a imagem `:latest`.
4. Exibe IP/FQDN e comandos de teste prontos.

### Executar

```bash
chmod +x scripts/06-teste-total.sh
./scripts/06-teste-total.sh
```

Durante a execução, o script mostra o contexto (RG, ACR, APP, LOCATION, PORT) e termina imprimindo algo como:

```
[INFO] Endpoint público
Ip             Fqdn
40.88.226.185  mottoth-12285.eastus.azurecontainer.io

[INFO] Testes (cole no terminal para validar)
curl -i 'http://40.88.226.185:8080/'
curl -i 'http://40.88.226.185:8080/api/ping'
# Swagger: http://40.88.226.185:8080/swagger/index.html
curl -i 'http://40.88.226.185:8080/swagger/v1/swagger.json'
```

> 🔐 **Credenciais do ACR:** o script usa `az acr credential show` para obter username e password temporários e configurar o `az container create`.

## 🧪 Testes rápidos (curl)

Substitua `BASE` pelo IP/FQDN retornado no fim do script.

```bash
BASE="http://SEU-IP-OU-FQDN:8080"

# Health (200 + "MottothTracking API OK")
curl -i "$BASE/"

# Ping (200 + {"pong":true,...})
curl -i "$BASE/api/ping"

# Swagger UI
curl -i "$BASE/swagger/index.html"

# Swagger JSON
curl -i "$BASE/swagger/v1/swagger.json"
```

## 📡 Endpoints principais

| Método | Rota | Descrição | Retorno esperado |
|--------|------|-----------|------------------|
| GET | `/` | Health básico | `"MottothTracking API OK"` |
| GET | `/api/ping` | Ping JSON | `{ "pong": true, "when": "..." }` |
| GET | `/swagger/index.html` | Swagger UI | HTML |
| GET | `/swagger/v1/swagger.json` | Swagger JSON | JSON |
| CRUD | `/api/Beacons[/{id}]` | Cadastro de beacons (Oracle) | 2xx / 4xx / 5xx |

## 🗄️ Banco de Dados (Oracle)

### Script SQL (gerado/atualizado)

**Arquivo:** `docs/script_bd.sql`

```sql
-- Schema: RM555881 (ajuste para o seu usuário se necessário)
CREATE TABLE RM555881.BEACONS (
  ID          NUMBER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
  CODIGO      VARCHAR2(100) NOT NULL,  -- mapeia 'Uuid'
  DESCRICAO   VARCHAR2(255),           -- mapeia 'Modelo'
  CREATED_AT  DATE DEFAULT SYSDATE
  -- (Opcional) STATUS VARCHAR2(50)
);
```

### Mapeamentos principais (EF Core → Oracle)

- `Beacon.Id` → `ID` (IDENTITY)
- `Beacon.Uuid` → `CODIGO`
- `Beacon.Modelo` → `DESCRICAO`
- `Beacon.CreatedAt` → `CREATED_AT`

### 🔐 Connection string (ACI)

O script de deploy injeta a variável segura:

```
ConnectionStrings__OracleConnection="Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=oracle.fiap.com.br)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=ORCL)));User Id=RM555881;Password=270505;"
```

> 🛑 Se aparecer **ORA-28000** (account locked), é necessário solicitar o desbloqueio do usuário junto ao suporte FIAP.

## 💻 Execução local

```bash
# Restaurar e rodar
dotnet restore
dotnet run

# Swagger local
# http://localhost:5000/swagger
```

### Alterar porta:

```bash
ASPNETCORE_URLS="http://+:8080" dotnet run
```

## 🧹 Limpeza de recursos

Para remover todos os recursos criados no Azure:

```bash
az group delete --name rg-mottoth-frotas-devops --yes --no-wait
```

## 🆘 Solução de problemas (FAQ)

### 1) ORA-28000: The account is locked
→ Solicitar desbloqueio do usuário Oracle na FIAP. Depois, recriar o ACI.

### 2) Swagger retorna 404 com curl -I
→ Use GET em vez de HEAD.

### 3) Aviso do curl (snap)
→ É só aviso, não impacta requisições HTTP.

### 4) Ping dá AmbiguousMatchException nos logs
→ Mantenha apenas um método GET no PingController.

### 5) Preciso redeployar a imagem
→ Basta rodar novamente `01-build-push.sh` e `02-deploy-aci.sh` ou o `06-teste-total.sh`.

## ✅ Checklist de Entrega

- ✅ Deploy automatizado via script único (`06-teste-total.sh`)
- ✅ ACI com imagem `:oracle` do ACR
- ✅ Health (`/`) e Ping (`/api/ping`) funcionando
- ✅ Swagger UI e JSON disponíveis
- ✅ Script SQL pronto (`docs/script_bd.sql`)
- ✅ CRUD Beacons implementado (aguarda desbloqueio Oracle)
- ✅ Usuário não-root no container
- ✅ README completo com passo a passo

## 👥 Créditos

**Projeto:** MottothTracking (FIAP)  
**Área:** DevOps / Cloud / .NET

**Integrantes:**
- Robert Daniel da Silva Coimbra — RM555881

---

**Documentação gerada em:** Setembro de 2025