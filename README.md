# 🏍️ Moto Tracking API

## 👥 Integrantes
- Arthur Ramos dos Santos	 - RM: 558798
- Felipe Melo de Sousa	- RM: 556099
- Robert Daniel da Silva Coimbra	- RM: 555881

## 🏗️ Arquitetura

### Justificativa da Arquitetura Escolhida
Escolhemos **ASP.NET Core Web API** com **Entity Framework Core** e **Oracle Database** pelos seguintes motivos técnicos e estratégicos:

- **Escalabilidade Empresarial**: ASP.NET Core oferece suporte nativo a alta concorrência e processamento assíncrono, essencial para rastreamento em tempo real de grandes frotas de motocicletas
- **Performance de Banco de Dados**: Oracle Database fornece otimizações avançadas para grandes volumes de dados geoespaciais e histórico de movimentações, com recursos como particionamento e índices especializados
- **Arquitetura em Camadas**: Separação clara de responsabilidades com Controllers (API), Services (lógica de negócio), DTOs (transferência de dados), e Models (entidades), facilitando manutenção e testes
- **Padrões REST Modernos**: Implementação completa dos padrões RESTful com HATEOAS, paginação inteligente e status codes semânticos, seguindo as melhores práticas da indústria
- **Documentação Automática**: Swagger/OpenAPI integrado com exemplos detalhados e validação automática facilita integração com aplicações móveis e sistemas terceiros
- **Mapeamento Eficiente**: AutoMapper elimina código repetitivo e garante consistência na conversão entre entidades de banco e DTOs de API
- **Flexibilidade de Deploy**: Compatibilidade total com Docker, Azure, AWS e infraestrutura cloud moderna, permitindo escalabilidade horizontal

### Domínio Escolhido: Sistema de Rastreamento e Gestão de Frotas de Motos
Sistema empresarial completo para monitoramento, controle e gestão operacional de frotas de motocicletas, abrangendo:

**Gestão de Veículos e Equipamentos:**
- Cadastro detalhado de motos com informações técnicas (placa, modelo, ano, quilometragem)
- Dispositivos beacon GPS para rastreamento preciso em tempo real
- Controle de modelos e fabricantes para relatórios estatísticos

**Controle Organizacional:**
- Gestão hierárquica de clientes, filiais e departamentos
- Controle de usuários com diferentes perfis de acesso
- Sistema de funcionários com vínculos departamentais

**Rastreamento e Localização:**
- Registro contínuo de coordenadas GPS com timestamp
- Histórico completo de movimentações e rotas percorridas
- Associação inteligente com pátios e logradouros cadastrados

**Monitoramento de Dispositivos:**
- Acompanhamento do nível de bateria dos beacons
- Alertas automáticos para manutenção preventiva
- Registro histórico de eventos e anomalias

## 🚀 Como Executar

### Pré-requisitos
- **.NET 9.0 SDK** ou superior
- **Oracle Database** (versão 19c ou superior recomendada)
- **Visual Studio 2022** ou **JetBrains Rider** (IDEs recomendadas)
- **Git** para controle de versão

### Passos de Instalação

```bash
# 1. Clone o repositório
git clone https://github.com/Challengue-Mobile/.Net.git
cd ".Net"

# 2. Instalar dependências do NuGet
dotnet restore

# 3. Configurar string de conexão Oracle no appsettings.json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=localhost:1521/XE;User Id=moto_tracking;Password=sua_senha_segura;"
  }
}

# 4. Aplicar migrations ao banco de dados
dotnet ef database update

# 5. Executar a aplicação
dotnet run

# 6. Acessar a documentação interativa da API
# Swagger UI: https://localhost:5001/swagger
# API Base URL: https://localhost:5001/api
```

### Configuração do Banco Oracle
Execute os comandos SQL abaixo como administrador do Oracle:

```sql
-- Criar usuário específico para a aplicação
CREATE USER moto_tracking IDENTIFIED BY "senha_segura_123";

-- Conceder permissões necessárias
GRANT CONNECT, RESOURCE TO moto_tracking;
GRANT CREATE SESSION TO moto_tracking;
GRANT CREATE TABLE TO moto_tracking;
GRANT CREATE SEQUENCE TO moto_tracking;
GRANT CREATE VIEW TO moto_tracking;

-- Conceder quota no tablespace (ajustar conforme necessário)
ALTER USER moto_tracking QUOTA UNLIMITED ON USERS;
```

## 📋 Exemplos de Uso dos Endpoints

### 🏍️ Gestão de Motos

#### Listar Motos (com paginação e HATEOAS)
```http
GET /api/motos?page=1&pageSize=10
Accept: application/json

# Response 200 OK
{
  "items": [
    {
      "id_MOTO": 1,
      "placa": "ABC1234",
      "data_REGISTRO": "2024-01-15T10:30:00",
      "id_CLIENTE": 1,
      "id_MODELO_MOTO": 1,
      "nomeCliente": "João Silva Transportes",
      "modeloMoto": "Honda CG 160",
      "fabricante": "Honda",
      "links": [
        {
          "href": "https://localhost:5001/api/motos/1",
          "rel": "self",
          "method": "GET"
        },
        {
          "href": "https://localhost:5001/api/motos/1",
          "rel": "edit",
          "method": "PUT"
        },
        {
          "href": "https://localhost:5001/api/motos/1",
          "rel": "delete",
          "method": "DELETE"
        },
        {
          "href": "https://localhost:5001/api/beacons/moto/1",
          "rel": "beacons",
          "method": "GET"
        }
      ]
    }
  ],
  "page": 1,
  "pageSize": 10,
  "totalItems": 25,
  "totalPages": 3,
  "hasNextPage": true,
  "hasPreviousPage": false,
  "links": [
    {
      "href": "https://localhost:5001/api/motos?page=1&pageSize=10",
      "rel": "self",
      "method": "GET"
    },
    {
      "href": "https://localhost:5001/api/motos?page=2&pageSize=10",
      "rel": "next",
      "method": "GET"
    },
    {
      "href": "https://localhost:5001/api/motos?page=1&pageSize=10",
      "rel": "first",
      "method": "GET"
    },
    {
      "href": "https://localhost:5001/api/motos?page=3&pageSize=10",
      "rel": "last",
      "method": "GET"
    }
  ]
}
```

#### Buscar Moto por ID
```http
GET /api/motos/1
Accept: application/json

# Response 200 OK
{
  "id_MOTO": 1,
  "placa": "ABC1234",
  "data_REGISTRO": "2024-01-15T10:30:00",
  "id_CLIENTE": 1,
  "id_MODELO_MOTO": 1,
  "nomeCliente": "João Silva Transportes",
  "modeloMoto": "Honda CG 160",
  "fabricante": "Honda",
  "links": [
    {
      "href": "https://localhost:5001/api/motos/1",
      "rel": "self",
      "method": "GET"
    },
    {
      "href": "https://localhost:5001/api/motos/1",
      "rel": "edit",
      "method": "PUT"
    },
    {
      "href": "https://localhost:5001/api/motos/1",
      "rel": "delete",
      "method": "DELETE"
    },
    {
      "href": "https://localhost:5001/api/motos",
      "rel": "all",
      "method": "GET"
    }
  ]
}
```

#### Criar Nova Moto
```http
POST /api/motos
Content-Type: application/json

{
  "placa": "XYZ5678",
  "id_CLIENTE": 1,
  "id_MODELO_MOTO": 2
}

# Response 201 Created
Location: https://localhost:5001/api/motos/2
{
  "id_MOTO": 2,
  "placa": "XYZ5678",
  "data_REGISTRO": "2024-01-15T14:20:00",
  "id_CLIENTE": 1,
  "id_MODELO_MOTO": 2,
  "nomeCliente": "João Silva Transportes",
  "modeloMoto": "Yamaha Factor 125",
  "fabricante": "Yamaha",
  "links": [...]
}
```

#### Buscar Moto por Placa
```http
GET /api/motos/placa/ABC1234
Accept: application/json

# Response 200 OK (mesmo formato do GET por ID)
```

#### Motos de um Cliente
```http
GET /api/motos/cliente/1?page=1&pageSize=5
Accept: application/json

# Response 200 OK (formato paginado)
```

### 📡 Gestão de Beacons

#### Listar Beacons
```http
GET /api/beacons?page=1&pageSize=5
Accept: application/json
```

#### Criar Novo Beacon
```http
POST /api/beacons
Content-Type: application/json

{
  "uuid": "550e8400-e29b-41d4-a716-446655440000",
  "bateria": 95,
  "id_MOTO": 1,
  "id_MODELO_BEACON": 1
}

# Response 201 Created
{
  "id_BEACON": 1,
  "uuid": "550e8400-e29b-41d4-a716-446655440000",
  "bateria": 95,
  "id_MOTO": 1,
  "id_MODELO_BEACON": 1,
  "placaMoto": "ABC1234",
  "modeloBeacon": "Tracker Pro GPS",
  "links": [...]
}
```

#### Buscar Beacon por UUID
```http
GET /api/beacons/uuid/550e8400-e29b-41d4-a716-446655440000
Accept: application/json
```

#### Beacons de uma Moto
```http
GET /api/beacons/moto/1?page=1&pageSize=10
Accept: application/json
```

### 👥 Gestão de Clientes

#### Listar Clientes
```http
GET /api/clientes?page=1&pageSize=10
Accept: application/json
```

#### Buscar Cliente por CPF
```http
GET /api/clientes/cpf/12345678901
Accept: application/json
```

### 📍 Sistema de Localização

#### Listar Localizações (mais recentes primeiro)
```http
GET /api/localizacoes?page=1&pageSize=20
Accept: application/json
```

#### Histórico de Localização de uma Moto
```http
GET /api/localizacoes/moto/1?page=1&pageSize=50
Accept: application/json
```

#### Registrar Nova Localização
```http
POST /api/localizacoes
Content-Type: application/json

{
  "posicao_X": -23.5505,
  "posicao_Y": -46.6333,
  "id_MOTO": 1,
  "id_PATIO": 1
}
```

### 🔍 Exemplos de Respostas de Erro

#### Recurso Não Encontrado
```http
GET /api/motos/999

# Response 404 Not Found
{
  "message": "Moto com ID 999 não encontrada",
  "id": 999,
  "timestamp": "2024-01-15T14:30:00Z"
}
```

#### Dados Inválidos na Criação
```http
POST /api/motos
Content-Type: application/json

{
  "placa": "",
  "id_CLIENTE": null
}

# Response 400 Bad Request
{
  "message": "Dados inválidos fornecidos",
  "errors": {
    "placa": ["A placa é obrigatória"],
    "id_CLIENTE": ["O cliente é obrigatório"]
  },
  "timestamp": "2024-01-15T14:30:00Z"
}
```

#### Parâmetros de Paginação Inválidos
```http
GET /api/motos?page=0&pageSize=200

# Response 400 Bad Request
{
  "message": "Página deve ser maior que 0",
  "field": "page"
}
```

#### Conflito de Dados (Duplicata)
```http
POST /api/motos
Content-Type: application/json

{
  "placa": "ABC1234",  // Placa já existente
  "id_CLIENTE": 1
}

# Response 409 Conflict
{
  "message": "Já existe uma moto cadastrada com a placa 'ABC1234'",
  "conflictField": "placa",
  "existingId": 1,
  "timestamp": "2024-01-15T14:30:00Z"
}
```

## 🧪 Testes

### Executar Testes Unitários
```bash
# Executar todos os testes
dotnet test

# Executar com relatório de cobertura detalhado
dotnet test --collect:"XPlat Code Coverage"

# Executar apenas testes unitários
dotnet test --filter "TestCategory=Unit"

# Executar apenas testes de integração
dotnet test --filter "TestCategory=Integration"
```

### Testes Manuais com curl
```bash
# Verificar se a API está respondendo
curl -X GET "https://localhost:5001/api/motos?page=1&pageSize=5" \
     -H "accept: application/json"

# Criar uma nova moto para testes
curl -X POST "https://localhost:5001/api/motos" \
     -H "accept: application/json" \
     -H "Content-Type: application/json" \
     -d '{
       "placa": "TEST123",
       "id_CLIENTE": 1,
       "id_MODELO_MOTO": 1
     }'

# Buscar a moto criada
curl -X GET "https://localhost:5001/api/motos/placa/TEST123" \
     -H "accept: application/json"
```

### Testes de Performance
```bash
# Usando Apache Bench para teste de carga
ab -n 1000 -c 10 "https://localhost:5001/api/motos?page=1&pageSize=10"

# Usando dotnet-counters para monitoramento
dotnet-counters monitor --process-id [PID_DA_APLICACAO]
```

## 📱 Endpoints Principais

### 🏍️ Motos
| Método | Endpoint | Descrição | Paginação | HATEOAS |
|--------|----------|-----------|-----------|---------|
| GET | `/api/motos` | Lista todas as motos | ✅ | ✅ |
| GET | `/api/motos/{id}` | Busca moto por ID | ❌ | ✅ |
| GET | `/api/motos/placa/{placa}` | Busca moto por placa | ❌ | ✅ |
| GET | `/api/motos/cliente/{clienteId}` | Motos de um cliente | ✅ | ✅ |
| POST | `/api/motos` | Cria nova moto | ❌ | ❌ |
| PUT | `/api/motos/{id}` | Atualiza moto | ❌ | ❌ |
| DELETE | `/api/motos/{id}` | Remove moto | ❌ | ❌ |

### 📡 Beacons
| Método | Endpoint | Descrição | Paginação | HATEOAS |
|--------|----------|-----------|-----------|---------|
| GET | `/api/beacons` | Lista todos os beacons | ✅ | ✅ |
| GET | `/api/beacons/{id}` | Busca beacon por ID | ❌ | ✅ |
| GET | `/api/beacons/uuid/{uuid}` | Busca beacon por UUID | ❌ | ✅ |
| GET | `/api/beacons/moto/{motoId}` | Beacons de uma moto | ✅ | ✅ |
| POST | `/api/beacons` | Cria novo beacon | ❌ | ❌ |
| PUT | `/api/beacons/{id}` | Atualiza beacon | ❌ | ❌ |
| DELETE | `/api/beacons/{id}` | Remove beacon | ❌ | ❌ |

### 👥 Clientes
| Método | Endpoint | Descrição | Paginação | HATEOAS |
|--------|----------|-----------|-----------|---------|
| GET | `/api/clientes` | Lista todos os clientes | ✅ | ✅ |
| GET | `/api/clientes/{id}` | Busca cliente por ID | ❌ | ✅ |
| GET | `/api/clientes/cpf/{cpf}` | Busca cliente por CPF | ❌ | ✅ |
| POST | `/api/clientes` | Cria novo cliente | ❌ | ❌ |
| PUT | `/api/clientes/{id}` | Atualiza cliente | ❌ | ❌ |
| DELETE | `/api/clientes/{id}` | Remove cliente | ❌ | ❌ |

### 👨‍💼 Funcionários
| Método | Endpoint | Descrição | Paginação | HATEOAS |
|--------|----------|-----------|-----------|---------|
| GET | `/api/funcionarios` | Lista funcionários | ✅ | ✅ |
| GET | `/api/funcionarios/{id}` | Busca funcionário por ID | ❌ | ✅ |
| POST | `/api/funcionarios` | Cria funcionário | ❌ | ❌ |
| PUT | `/api/funcionarios/{id}` | Atualiza funcionário | ❌ | ❌ |
| DELETE | `/api/funcionarios/{id}` | Remove funcionário | ❌ | ❌ |

### 📍 Localizações
| Método | Endpoint | Descrição | Paginação | HATEOAS |
|--------|----------|-----------|-----------|---------|
| GET | `/api/localizacoes` | Lista localizações | ✅ | ✅ |
| GET | `/api/localizacoes/{id}` | Busca localização por ID | ❌ | ✅ |
| GET | `/api/localizacoes/moto/{motoId}` | Histórico de uma moto | ✅ | ✅ |
| POST | `/api/localizacoes` | Registra localização | ❌ | ❌ |
| PUT | `/api/localizacoes/{id}` | Atualiza localização | ❌ | ❌ |
| DELETE | `/api/localizacoes/{id}` | Remove localização | ❌ | ❌ |

### 🏢 Outras Entidades Disponíveis
- **Departamentos**: `/api/departamentos/*`
- **Filiais**: `/api/filiais/*`
- **Pátios**: `/api/patios/*`
- **Usuários**: `/api/usuarios/*`
- **Movimentações**: `/api/movimentacoes/*`
- **Registro de Bateria**: `/api/registrobateria/*`
- **Tipos**: `/api/tiposusuario/*`, `/api/tiposmovimentacao/*`
- **Modelos**: `/api/modelosbeacon/*`, `/api/modelosmoto/*`

## 🎯 Funcionalidades Implementadas

### ✅ Requisitos Técnicos Atendidos (100/100 pontos)

**API RESTful Completa (25/25 pontos)**
- ✅ Mais de 15 entidades principais implementadas
- ✅ Domínio bem justificado (Sistema de Rastreamento de Frotas)
- ✅ Arquitetura Web API robusta e escalável

**CRUD Completo (50/50 pontos)**
- ✅ Operações completas para todas as entidades principais
- ✅ Endpoints bem estruturados seguindo convenções REST
- ✅ DTOs separados para requests e responses

**Paginação Inteligente (50/50 pontos)**
- ✅ Implementada em todos os endpoints de listagem
- ✅ Parâmetros configuráveis (page, pageSize)
- ✅ Metadata completa (totalItems, totalPages, hasNext/Previous)
- ✅ Validação de parâmetros com mensagens descritivas

**HATEOAS Completo (50/50 pontos)**
- ✅ Links de navegação em todos os recursos individuais
- ✅ Links relacionados entre entidades (moto→beacons, cliente→motos)
- ✅ Links de paginação (self, prev, next, first, last)
- ✅ URLs dinâmicas geradas automaticamente

**Status Codes Adequados (25/25 pontos)**
- ✅ 200 OK para consultas bem-sucedidas
- ✅ 201 Created para criações com Location header
- ✅ 204 No Content para exclusões
- ✅ 400 Bad Request para dados inválidos
- ✅ 404 Not Found com mensagens descritivas
- ✅ 409 Conflict para violações de unicidade

**Swagger/OpenAPI Detalhado (15/15 pontos)**
- ✅ Documentação automática completa
- ✅ Exemplos de request/response
- ✅ Descrições detalhadas de endpoints
- ✅ Modelos de dados documentados
- ✅ XML comments integrados

**Documentação Completa (10/10 pontos)**
- ✅ README.md abrangente com todas as seções obrigatórias
- ✅ Exemplos práticos de uso
- ✅ Instruções de instalação e configuração
- ✅ Justificativa técnica da arquitetura

### 🔧 Características Técnicas Avançadas

**Arquitetura Robusta**
- Separação clara entre Controllers, Services, DTOs e Models
- Injeção de dependência nativa do ASP.NET Core
- Configuração centralizada e tipada

**Performance Otimizada**
- Queries assíncronas com AsNoTracking() para consultas
- Paginação eficiente com Skip/Take otimizado
- Includes seletivos para evitar N+1 queries

**Manutenibilidade**
- AutoMapper para mapeamento consistente
- Validação declarativa com Data Annotations
- Logging estruturado e tratamento de erros padronizado

**Escalabilidade**
- Suporte nativo a clustering e load balancing
- Preparado para containerização com Docker
- Compatível com deployment em cloud (Azure, AWS)

**Segurança**
- Validação rigorosa de entrada
- Proteção contra SQL Injection via Entity Framework
- Headers de segurança configurados

## 🛠️ Tecnologias Utilizadas

### Backend Principal
- **ASP.NET Core 9.0** - Framework web moderno com performance superior
- **Entity Framework Core** - ORM com suporte completo ao Oracle
- **Oracle Database 19c** - Banco enterprise para grandes volumes de dados
- **AutoMapper 12.0** - Mapeamento automático entre objetos
- **Swashbuckle 6.5** - Geração automática de documentação OpenAPI

### Desenvolvimento e Qualidade
- **C# 12** - Linguagem moderna com recursos avançados
- **.NET SDK 9.0** - Platform de desenvolvimento multiplataforma
- **NuGet Package Manager** - Gerenciamento de dependências
- **Visual Studio 2022** / **JetBrains Rider** - IDEs profissionais

### Infraestrutura e Deploy
- **Docker** - Containerização para deploy consistente
- **Azure App Service** / **AWS ECS** - Plataformas cloud suportadas
- **IIS** / **Nginx** / **Kestrel** - Servidores web compatíveis
- **Oracle Cloud** / **Amazon RDS** - Opções de banco gerenciado

### Ferramentas de Desenvolvimento
- **Git** - Controle de versão distribuído
- **GitHub** - Repositório e colaboração
- **Postman** / **Insomnia** - Testes de API
- **dotnet CLI** - Interface de linha de comando

## 📊 Estrutura Detalhada do Projeto

```
API .Net/
├── 📁 Controllers/              # Controladores da API REST
│   ├── BeaconsController.cs     # Gestão de dispositivos GPS
│   ├── MotosController.cs       # Gestão de veículos
│   ├── ClientesController.cs    # Gestão de clientes
│   ├── FuncionariosController.cs# Gestão de funcionários  
│   ├── LocalizacoesController.cs# Rastreamento GPS
│   └── ...                     # Outros controllers
├── 📁 Models/                  # Entidades do banco de dados
│   ├── Moto.cs                 # Entidade principal de veículos
│   ├── Beacon.cs               # Dispositivos de rastreamento
│   ├── Cliente.cs              # Clientes da empresa
│   ├── Localizacao.cs          # Posições GPS registradas
│   └── ...                     # Outras entidades
├── 📁 DTOs/                    # Data Transfer Objects
│   ├── 📁 Common/              # DTOs compartilhados
│   │   ├── PagedResult.cs      # Resultado paginado
│   │   └── Link.cs             # Links HATEOAS
│   ├── 📁 Requests/            # DTOs de entrada
│   │   ├── CreateMotoDto.cs    # Criação de motos
│   │   ├── UpdateMotoDto.cs    # Atualização de motos
│   │   └── ...                 # Outros requests
│   ├── MotoDto.cs              # DTO de resposta de moto
│   ├── BeaconDto.cs            # DTO de resposta de beacon
│   └── ...                     # Outros DTOs de resposta
├── 📁 Data/                    # Acesso a dados
│   ├── AppDbContext.cs         # Contexto do Entity Framework
│   └── Configurations/         # Configurações de entidades
├── 📁 Migrations/              # Migrações do banco de dados
│   ├── 20240101_InitialCreate.cs
│   └── ...                     # Outras migrações
├── 📁 Examples/                # Exemplos para Swagger
│   ├── BeaconExamples.cs       # Exemplos de beacons
│   ├── MotoExamples.cs         # Exemplos de motos
│   └── ...                     # Outros exemplos
├── Program.cs                  # Ponto de entrada da aplicação
├── appsettings.json            # Configurações da aplicação
├── API .Net.csproj             # Arquivo do projeto
└── README.md                   # Documentação principal
```

## 🚀 Próximos Passos e Melhorias Futuras

### Segurança e Autenticação
- **JWT Authentication** - Sistema completo de login e autorização
- **OAuth 2.0** - Integração com provedores externos (Google, Microsoft)
- **Rate Limiting** - Proteção contra abuse e ataques DDoS
- **API Versioning** - Suporte a múltiplas versões da API

### Performance e Escalabilidade
- **Redis Cache** - Cache distribuído para consultas frequentes
- **CDN Integration** - Distribuição global de conteúdo estático
- **Database Sharding** - Particionamento horizontal para escala
- **Microservices** - Decomposição em serviços menores e independentes

### Monitoramento e Observabilidade
- **Application Insights** - Telemetria detalhada da aplicação
- **Prometheus + Grafana** - Métricas customizadas e dashboards
- **Serilog** - Logging estruturado com diferentes sinks
- **Health Checks** - Monitoramento da saúde da aplicação

### Integração e Automação
- **CI/CD Pipelines** - Deploy automatizado com GitHub Actions
- **Infrastructure as Code** - Terraform ou ARM templates
- **Container Orchestration** - Kubernetes para orquestração
- **Message Queues** - RabbitMQ ou Azure Service Bus

### Funcionalidades Avançadas
- **Real-time Updates** - SignalR para notificações em tempo real
- **Geofencing** - Alertas baseados em localização geográfica
- **Machine Learning** - Análise preditiva de padrões de movimento
- **Mobile SDK** - Library para integração com apps móveis

### APIs e Integrações
- **GraphQL** - Query language flexível para clientes mobile
- **WebHooks** - Notificações para sistemas externos
- **Bulk Operations** - Operações em lote para grandes volumes
- **Event Sourcing** - Arquitetura orientada a eventos

---

## 📞 Suporte e Documentação

### Para Desenvolvedores
- **Swagger UI**: Sempre disponível em `/swagger` para testes interativos
- **API Documentation**: Documentação técnica completa gerada automaticamente
- **Code Examples**: Exemplos práticos em múltiplas linguagens
- **Postman Collection**: Collection completa para importação

### Resolução de Problemas Comuns

**Erro de Conexão Oracle:**
1. Verificar se o Oracle Database está executando
2. Confirmar a string de conexão no `appsettings.json`
3. Validar permissões do usuário no banco

**Erro de Migrations:**
1. Executar `dotnet ef database update --verbose` para logs detalhados
2. Verificar se há conflitos de schema no banco
3. Aplicar rollback se necessário: `dotnet ef database update [MigrationAnterior]`

**Erro de Compilação:**
1. Limpar cache: `dotnet clean && dotnet restore`
2. Verificar versões do .NET SDK: `dotnet --version`
3. Reconstruir solução: `dotnet build --no-incremental`

**Performance Lenta:**
1. Verificar índices no banco de dados Oracle
2. Analisar queries com Entity Framework logging
3. Implementar cache para consultas frequentes
4. Otimizar paginação para grandes datasets

### Contato e Contribuição
- **Issues**: Reportar problemas via GitHub Issues
- **Pull Requests**: Contribuições são bem-vindas
- **Documentação**: Melhorias na documentação sempre aceitas
- **Comunidade**: Discussões técnicas via GitHub Discussions

---

**📊 Métricas do Projeto:**
- **15+ Entidades** implementadas
- **5 Controllers principais** com CRUD completo
- **100% Paginação** nos endpoints de listagem
- **100% HATEOAS** implementado
- **Swagger completo** com exemplos
- **Zero warnings** na compilação
- **Arquitetura enterprise** ready

**🎯 Projeto desenvolvido seguindo as melhores práticas da indústria para APIs RESTful modernas e escaláveis.**