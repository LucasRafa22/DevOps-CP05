# Dimdim WebApp — 2º Checkpoint

Projeto web desenvolvido em **.NET 9** para o checkpoint de Aplicações e Banco em Nuvem.

## Objetivo

Criar uma API REST simples para gerenciamento de pedidos e seus itens, usando uma estrutura master-detail:

- `Pedidos` — tabela master
- `ItensPedido` — tabela detail
- `ItensPedido.PedidoId` → `Pedidos.Id` por FK

A persistência prevista para a entrega em nuvem é **Azure SQL Database (PaaS)**.

## Tecnologias

- .NET 9 / ASP.NET Core Web API
- Entity Framework Core 9
- SQL Server / Azure SQL Database
- Swagger
- Docker
- Azure CLI
- Azure App Service
- Azure Container Registry
- Application Insights
- Health Check

## Estrutura

```text
src/
  Dimdim.Api/
    Controllers/
    Program.cs
    Dockerfile
  Dimdim.Application/
    DTOs/
    Interfaces/
  Dimdim.Domain/
    Entities/
  Dimdim.Infrastructure/
    Data/
    Repositories/
database/
  01_create_tables.sql
  02_sample_data.sql
  03_verification.sql
scripts/
  azure-deploy.sh
  azure-deploy.ps1
docs/
```

## Executar localmente

Pré-requisitos:

- .NET SDK 9
- SQL Server local ou Azure SQL
- Docker (opcional para execução em container)
- Azure CLI para deploy

```bash
dotnet restore
dotnet build
dotnet run --project src/Dimdim.Api
```

Swagger:

`http://localhost:5080/swagger`

Health:

`http://localhost:5080/health`

## Banco

Para SQL Server local, configure a connection string `DimdimDb` em:

`src/Dimdim.Api/appsettings.Development.json`

Para Azure, a connection string é configurada no App Service pelo script de deploy.

Execute o DDL:

`database/01_create_tables.sql`

Depois, se quiser dados de demonstração:

`database/02_sample_data.sql`

Para conferir a persistência:

`database/03_verification.sql`

## API

### Pedidos

- `GET /api/pedidos`
- `GET /api/pedidos/{id}`
- `POST /api/pedidos`
- `PUT /api/pedidos/{id}`
- `DELETE /api/pedidos/{id}`

### Itens de pedido

- `GET /api/itens-pedido`
- `GET /api/itens-pedido/{id}`
- `PUT /api/itens-pedido/{id}`
- `DELETE /api/itens-pedido/{id}`
- `GET /api/pedidos/{pedidoId}/itens`
- `POST /api/pedidos/{pedidoId}/itens`

O `POST` de item usa o pedido master na própria rota, garantindo a FK.

## JSON para demonstração

### POST Pedido

```json
{
  "clienteNome": "Maria Silva"
}
```

### PUT Pedido

```json
{
  "clienteNome": "Maria Souza"
}
```

### POST Item

```json
{
  "pedidoId": "ID_DO_PEDIDO_CRIADO",
  "descricao": "Plano mensal",
  "quantidade": 2,
  "precoUnitario": 29.90
}
```

### PUT Item

```json
{
  "descricao": "Plano mensal atualizado",
  "quantidade": 3,
  "precoUnitario": 31.90
}
```

## Deploy Azure

O deploy foi preparado via Azure CLI e Docker.

1. Faça login:

```bash
az login
```

2. Edite a senha do SQL no script.

3. Execute:

Linux/macOS:
```bash
chmod +x scripts/azure-deploy.sh
./scripts/azure-deploy.sh
```

PowerShell:
```powershell
.\scriptszure-deploy.ps1
```

O script cria:

- Resource Group
- Azure Container Registry
- SQL Server lógico
- Azure SQL Database
- Firewall para serviços Azure
- Application Insights
- App Service Plan
- Web App para o container

Após o provisionamento, execute `database/01_create_tables.sql` no Azure SQL e teste `/swagger` e `/health`.

## Application Insights

A API possui integração com Application Insights pelo pacote `Microsoft.ApplicationInsights.AspNetCore`. No ambiente Azure, configure a connection string do recurso de Application Insights como configuração da aplicação antes da demonstração.

## Demonstração

A avaliação deve mostrar:

1. API publicada no Azure.
2. Swagger funcionando.
3. `POST` criando um pedido.
4. `POST` criando itens vinculados ao pedido.
5. `GET` consultando os dados.
6. `PUT` alterando dados.
7. `DELETE` removendo dados.
8. Consulta direta no Azure SQL após as operações.
9. `/health` funcionando.
10. Application Insights registrando a aplicação.

## Link do vídeo

> **Vídeo da apresentação:** _______________________________________________

## Integrantes

- __________________________________
- __________________________________
- __________________________________
- __________________________________
