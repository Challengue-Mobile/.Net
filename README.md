# Moto Tracking API

API RESTful desenvolvida em ASP.NET Core para gerenciamento de motos, clientes, localização e rastreamento.

## 📋 Descrição do Projeto

Este projeto implementa uma API RESTful utilizando ASP.NET Core com controllers para fornecer um CRUD completo de entidades relacionadas ao rastreamento de motos. A API se integra com um banco de dados Oracle via Entity Framework Core e utiliza migrations para criação das tabelas.

### Principais Funcionalidades

- Cadastro e gerenciamento de motos
- Registro de clientes
- Monitoramento de localização em tempo real
- Rastreamento por beacons
- Registro de movimentações
- Gerenciamento de usuários e permissões

## 🚀 Tecnologias Utilizadas

- ASP.NET Core 9.0
- Entity Framework Core 9.0
- Oracle Database
- Swagger/OpenAPI para documentação
- Migrations para versionamento do banco de dados

## 📦 Estrutura do Banco de Dados

A API utiliza um modelo relacional complexo com as seguintes entidades principais:

- **TB_MOTO**: Armazena informações sobre motos
- **TB_CLIENTE**: Registra dados dos clientes
- **TB_LOCALIZACAO**: Armazena posições de motos ao longo do tempo
- **TB_MOVIMENTACAO**: Registra movimentações das motos
- **TB_BEACON**: Gerencia dispositivos de rastreamento
- **TB_USUARIO**: Controla usuários do sistema

## 🔧 Instalação

### Pré-requisitos

- .NET 9.0 SDK
- Oracle Database
- Visual Studio 2022/Visual Studio Code/Rider

### Passos para Instalação

1. Clone o repositório
```bash
git clone https://github.com/seu-usuario/moto-tracking-api.git
cd moto-tracking-api
```

2. Configure a string de conexão com o Oracle no arquivo `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "User Id=seu_usuario;Password=sua_senha;Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=seu_host)(PORT=1521))(CONNECT_DATA=(SID=seu_sid)));"
  }
}
```

3. Execute as migrations para criar o banco de dados:
```bash
dotnet ef database update
```

4. Execute o projeto:
```bash
dotnet run
```

5. Acesse a documentação da API:
```
https://localhost:5001/
```

## 📃 Rotas da API

### Motos

| Método | Rota | Descrição | Resposta |
|--------|------|-----------|----------|
| GET | /api/Motos | Retorna todas as motos | 200 OK |
| GET | /api/Motos/{id} | Busca uma moto pelo ID | 200 OK, 404 Not Found |
| GET | /api/Motos/placa/{placa} | Busca uma moto pela placa | 200 OK, 404 Not Found |
| GET | /api/Motos/cliente/{clienteId} | Busca motos por cliente | 200 OK |
| POST | /api/Motos | Cadastra uma nova moto | 201 Created, 400 Bad Request |
| PUT | /api/Motos/{id} | Atualiza uma moto | 204 No Content, 400 Bad Request, 404 Not Found |
| DELETE | /api/Motos/{id} | Remove uma moto | 204 No Content, 404 Not Found |

### Clientes

| Método | Rota | Descrição | Resposta |
|--------|------|-----------|----------|
| GET | /api/Clientes | Retorna todos os clientes | 200 OK |
| GET | /api/Clientes/{id} | Busca um cliente pelo ID | 200 OK, 404 Not Found |
| GET | /api/Clientes/cpf/{cpf} | Busca um cliente pelo CPF | 200 OK, 404 Not Found |
| POST | /api/Clientes | Cadastra um novo cliente | 201 Created, 400 Bad Request |
| PUT | /api/Clientes/{id} | Atualiza um cliente | 204 No Content, 400 Bad Request, 404 Not Found |
| DELETE | /api/Clientes/{id} | Remove um cliente | 204 No Content, 404 Not Found |

### Localizações

| Método | Rota | Descrição | Resposta |
|--------|------|-----------|----------|
| GET | /api/Localizacoes | Retorna todas as localizações | 200 OK |
| GET | /api/Localizacoes/{id} | Busca uma localização pelo ID | 200 OK, 404 Not Found |
| GET | /api/Localizacoes/moto/{motoId} | Busca localizações por moto | 200 OK |
| POST | /api/Localizacoes | Registra uma nova localização | 201 Created, 400 Bad Request |
| PUT | /api/Localizacoes/{id} | Atualiza uma localização | 204 No Content, 400 Bad Request, 404 Not Found |
| DELETE | /api/Localizacoes/{id} | Remove uma localização | 204 No Content, 404 Not Found |

### Beacons

| Método | Rota | Descrição | Resposta |
|--------|------|-----------|----------|
| GET | /api/Beacons | Retorna todos os beacons | 200 OK |
| GET | /api/Beacons/{id} | Busca um beacon pelo ID | 200 OK, 404 Not Found |
| GET | /api/Beacons/moto/{motoId} | Busca beacons por moto | 200 OK |
| GET | /api/Beacons/uuid/{uuid} | Busca um beacon pelo UUID | 200 OK, 404 Not Found |
| POST | /api/Beacons | Cadastra um novo beacon | 201 Created, 400 Bad Request |
| PUT | /api/Beacons/{id} | Atualiza um beacon | 204 No Content, 400 Bad Request, 404 Not Found |
| DELETE | /api/Beacons/{id} | Remove um beacon | 204 No Content, 404 Not Found |

### Movimentações

| Método | Rota | Descrição | Resposta |
|--------|------|-----------|----------|
| GET | /api/Movimentacoes | Retorna todas as movimentações | 200 OK |
| GET | /api/Movimentacoes/{id} | Busca uma movimentação pelo ID | 200 OK, 404 Not Found |
| GET | /api/Movimentacoes/moto/{motoId} | Busca movimentações por moto | 200 OK |
| POST | /api/Movimentacoes | Registra uma nova movimentação | 201 Created, 400 Bad Request |
| PUT | /api/Movimentacoes/{id} | Atualiza uma movimentação | 204 No Content, 400 Bad Request, 404 Not Found |
| DELETE | /api/Movimentacoes/{id} | Remove uma movimentação | 204 No Content, 404 Not Found |

## 📸 Screenshots

### Swagger UI
![Swagger UI](https://via.placeholder.com/800x400?text=Swagger+UI+Screenshot)

### Exemplo de Resposta JSON
```json
{
  "id_moto": 1,
  "placa": "ABC1234",
  "data_registro": "2025-05-20T10:30:00",
  "id_cliente": 2,
  "id_modelo_moto": 3,
  "cliente": {
    "id_cliente": 2,
    "nome": "João da Silva",
    "cpf": "123.456.789-00",
    "email": "joao@example.com"
  },
  "modeloMoto": {
    "id_modelo_moto": 3,
    "nome": "CB 500",
    "fabricante": "Honda"
  }
}
```




