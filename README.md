# 🚀 API .Net - Sistema de Rastreamento de Motocicletas

Uma API REST completa desenvolvida em .NET para gerenciamento e rastreamento de motocicletas, utilizando Entity Framework e Oracle Database.

## 📋 Índice

- [Sobre o Projeto](#sobre-o-projeto)
- [Funcionalidades](#funcionalidades)
- [Tecnologias Utilizadas](#tecnologias-utilizadas)
- [Arquitetura](#arquitetura)
- [Instalação](#instalação)
- [Configuração](#configuração)
- [Uso](#uso)
- [Endpoints da API](#endpoints-da-api)
- [Qualidade de Código](#qualidade-de-código)
- [Contribuição](#contribuição)
- [Licença](#licença)

## 🎯 Sobre o Projeto

Este sistema foi desenvolvido para gerenciar o rastreamento de motocicletas através de beacons, oferecendo controle completo sobre:

- **Cadastro de motocicletas** e seus proprietários
- **Rastreamento em tempo real** via beacons
- **Controle de movimentações** e histórico
- **Gestão de usuários** e permissões
- **Monitoramento de bateria** dos dispositivos
- **Localização geográfica** completa

## ⚡ Funcionalidades

### 🏍️ Gestão de Motocicletas
- Cadastro completo de motos com modelo, placa e cliente
- Associação com beacons de rastreamento
- Histórico de movimentações

### 📍 Rastreamento
- Localização em tempo real via coordenadas GPS
- Histórico de posições e trajetos
- Controle de entrada/saída de pátios

### 👥 Gestão de Usuários
- Sistema de autenticação e autorização
- Diferentes tipos de usuário (Admin, Operador, etc.)
- Controle de permissões por funcionalidade

### 🔋 Monitoramento de Dispositivos
- Status da bateria dos beacons em tempo real
- Alertas de bateria baixa
- Histórico de registros de bateria

### 🏢 Estrutura Organizacional
- Gestão de filiais e departamentos
- Controle de funcionários e suas responsabilidades
- Organização geográfica (País → Estado → Cidade → Bairro)

## 🛠️ Tecnologias Utilizadas

### Backend
- **.NET 9.0** - Framework principal
- **ASP.NET Core** - Web API
- **Entity Framework Core** - ORM
- **Oracle Database** - Banco de dados
- **AutoMapper** - Mapeamento de objetos
- **FluentValidation** - Validação de dados

### Documentação
- **Swagger/OpenAPI** - Documentação interativa da API
- **Swashbuckle** - Geração automática de documentação

### Qualidade de Código
- **SonarCloud** - Análise estática de código
- **Code Coverage** - Cobertura de testes
- **EditorConfig** - Padronização de código

### DevOps
- **Git** - Controle de versão
- **GitHub Actions** - CI/CD (se aplicável)
- **Docker** - Containerização (se aplicável)

## 🏗️ Arquitetura

O projeto segue uma arquitetura em camadas com separação de responsabilidades:

```
📦 API_.Net/
├── 📁 Controllers/          # Controladores da API
├── 📁 Models/              # Modelos de dados (Entities)
├── 📁 DTOs/                # Data Transfer Objects
├── 📁 Data/                # Contexto do Entity Framework
├── 📁 Examples/            # Exemplos para Swagger
├── 📁 Migrations/          # Migrações do banco de dados
└── 📄 Program.cs           # Configuração da aplicação
```

### Modelos Principais
- **Moto** - Informações das motocicletas
- **Beacon** - Dispositivos de rastreamento
- **Localizacao** - Coordenadas GPS e timestamps
- **Cliente** - Proprietários das motocicletas
- **Usuario** - Usuários do sistema
- **Movimentacao** - Histórico de ações

## 🚀 Instalação

### Pré-requisitos
- .NET 9.0 SDK
- Oracle Database (local ou cloud)
- Git

### Passos

1. **Clone o repositório**
```bash
git clone https://github.com/seu-usuario/api-net-rastreamento.git
cd api-net-rastreamento
```

2. **Restaure as dependências**
```bash
dotnet restore
```

3. **Configure o banco de dados**
```bash
# Edite o arquivo appsettings.json com sua connection string
# Execute as migrações
dotnet ef database update
```

4. **Execute a aplicação**
```bash
dotnet run
```

## ⚙️ Configuração

### Connection String

Configure sua connection string no `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=localhost:1521/XE;User Id=seu_usuario;Password=sua_senha;"
  }
}
```

### Swagger

A documentação da API estará disponível em:
- **Desenvolvimento**: `https://localhost:5001/swagger`
- **Produção**: `https://sua-api.com/swagger`

## 📖 Uso

### Exemplo de Requisição

```http
POST /api/motos
Content-Type: application/json

{
  "placa": "ABC-1234",
  "idModeloMoto": 1,
  "idCliente": 1
}
```

### Exemplo de Resposta

```json
{
  "id": 1,
  "placa": "ABC-1234",
  "dataRegistro": "2025-08-19T22:30:00Z",
  "idCliente": 1,
  "idModeloMoto": 1,
  "nomeCliente": "João Silva",
  "modeloMoto": "Honda CG 160",
  "fabricante": "Honda"
}
```

## 🛣️ Endpoints da API

### Motocicletas
- `GET /api/motos` - Lista todas as motos
- `GET /api/motos/{id}` - Busca moto por ID
- `POST /api/motos` - Cadastra nova moto
- `PUT /api/motos/{id}` - Atualiza moto
- `DELETE /api/motos/{id}` - Remove moto

### Rastreamento
- `GET /api/localizacoes` - Lista localizações
- `POST /api/localizacoes` - Registra nova localização
- `GET /api/localizacoes/moto/{idMoto}` - Histórico de uma moto

### Usuários
- `GET /api/usuarios` - Lista usuários
- `POST /api/usuarios` - Cadastra usuário
- `PUT /api/usuarios/{id}` - Atualiza usuário

### Clientes
- `GET /api/clientes` - Lista clientes
- `POST /api/clientes` - Cadastra cliente
- `PUT /api/clientes/{id}` - Atualiza cliente

*Para documentação completa, acesse `/swagger` quando a aplicação estiver rodando.*

## 🏆 Qualidade de Código

Este projeto mantém altos padrões de qualidade:

### SonarCloud
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=Challengue-Mobile_.Net&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=Challengue-Mobile_.Net)

- **Security**: A+ (0 vulnerabilidades)
- **Reliability**: A+ (0 bugs)
- **Maintainability**: A+ (0 code smells)
- **Coverage**: Configuração em andamento

### Padrões Implementados
- ✅ **Validação de entrada** com `[Required]` attributes
- ✅ **Culture-aware DateTime parsing** 
- ✅ **Eliminação de magic strings**
- ✅ **Naming conventions** consistentes
- ✅ **Separation of Concerns** com DTOs
- ✅ **Error handling** padronizado

## 🤝 Contribuição

Contribuições são sempre bem-vindas! Para contribuir:

1. **Fork** o projeto
2. Crie uma **branch** para sua feature (`git checkout -b feature/AmazingFeature`)
3. **Commit** suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. **Push** para a branch (`git push origin feature/AmazingFeature`)
5. Abra um **Pull Request**

### Diretrizes
- Mantenha o código limpo e bem documentado
- Adicione testes para novas funcionalidades
- Siga os padrões de código existentes
- Atualize a documentação quando necessário

## 📝 Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.

---

<div align="center">
  <h3>⭐ Se este projeto te ajudou, considere dar uma estrela!</h3>
  <p>Feito com ❤️ e muito ☕</p>
</div>
