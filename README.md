# MottothTracking - Sistema de Rastreamento de Frotas

## 📋 Descrição da Solução
Sistema empresarial para rastreamento e gestão de frotas de motocicletas com dispositivos beacon GPS. Desenvolvido em .NET 8 com Oracle Database e deploy na Azure usando containers.

## 🏢 Benefícios para o Negócio
- **Redução de custos**: Otimização de rotas e controle de combustível
- **Segurança**: Rastreamento 24/7 e alertas de furto
- **Produtividade**: Dashboard gerencial e relatórios automáticos
- **Compliance**: Auditoria completa de movimentações

## 🚀 Deploy na Azure - Passo a Passo

### Pré-requisitos
```bash
# Azure CLI instalado e logado
az login
az account show
```

### Passo 1: Clone do Repositório
```bash
git clone https://github.com/[usuario]/mottoth-tracking-devops.git
cd mottoth-tracking-devops
```

### Passo 2: Executar Scripts Azure

#### 2.1. Criar Resource Group
```bash
chmod +x scripts/*.sh
./scripts/01-create-resource-group.sh
```

#### 2.2. Criar Azure Container Registry
```bash
./scripts/02-create-acr.sh
```
**⚠️ IMPORTANTE:** Anote o nome do ACR que será exibido!

#### 2.3. Build e Push da Imagem
```bash
# Edite o script e substitua ACR_NAME pelo seu
nano scripts/03-build-push.sh
./scripts/03-build-push.sh
```

#### 2.4. Criar Container Instance
```bash
# Edite o script e substitua ACR_NAME pelo seu
nano scripts/04-create-aci.sh
./scripts/04-create-aci.sh
```
**⚠️ IMPORTANTE:** Anote a URL final que será exibida!

### Passo 3: Testar Aplicação
```bash
# Edite o script e substitua API_URL pela sua
nano scripts/05-test-api.sh
./scripts/05-test-api.sh
```

## 🧪 Testes CRUD - Usuários

### 1. Health Check
```bash
curl -X GET "http://[sua-url]:8080/health"
```

### 2. CREATE - Criar Usuário
```bash
curl -X POST "http://[sua-url]:8080/api/usuarios" \
  -H "Content-Type: application/json" \
  -d '{
    "nome": "João Silva",
    "email": "joao@teste.com", 
    "senha": "123456"
  }'
```

### 3. READ - Listar Usuários
```bash
curl -X GET "http://[sua-url]:8080/api/usuarios"
```

### 4. UPDATE - Atualizar Usuário
```bash
curl -X PUT "http://[sua-url]:8080/api/usuarios/1" \
  -H "Content-Type: application/json" \
  -d '{
    "id": 1,
    "nome": "João Silva Santos",
    "email": "joao.santos@teste.com",
    "senha": "123456"
  }'
```

### 5. DELETE - Excluir Usuário
```bash
curl -X DELETE "http://[sua-url]:8080/api/usuarios/1"
```

## 🗄️ Verificação no Banco Oracle

Após cada operação CRUD, verificar no Oracle:
```sql
-- Conectar no Oracle Database
SELECT * FROM TB_USUARIO ORDER BY ID_USUARIO DESC;
```

## 📊 URLs Importantes

- **API Principal**: http://[sua-url]:8080/api/usuarios
- **Swagger**: http://[sua-url]:8080/swagger
- **Health Check**: http://[sua-url]:8080/health

## 🏗️ Arquitetura

```
GitHub → ACR (Docker Images) → ACI (Runtime) → Oracle FIAP
```

- **ACR**: Azure Container Registry (imagens Docker)
- **ACI**: Azure Container Instance (execução)
- **Oracle**: Banco de dados FIAP (existente)

## 🔧 Desenvolvimento Local

```bash
# Executar localmente
dotnet run

# Acessar Swagger
http://localhost:5000/swagger
```

## 🧹 Limpeza (Opcional)

Para remover todos os recursos Azure:
```bash
./scripts/06-cleanup.sh
```

## ✅ Checklist de Entrega

- [x] CRUD completo (Usuários)
- [x] Container com usuário não-root
- [x] Scripts Azure CLI funcionais
- [x] Swagger documentação
- [x] Health check endpoint
- [x] Banco Oracle integrado
- [x] README com passo-a-passo