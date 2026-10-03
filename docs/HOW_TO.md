# How-to — Dimdim WebApp

## 1. Preparar ambiente

Instale:

- .NET 9 SDK
- Docker Desktop
- Azure CLI
- Uma ferramenta para executar SQL no Azure, como Azure Data Studio ou SSMS

## 2. Rodar localmente

```bash
dotnet restore
dotnet build
dotnet run --project src/Dimdim.Api
```

Abra:

`http://localhost:5080/swagger`

## 3. Criar banco local

Crie um banco chamado `DimdimDb` no SQL Server e execute:

```text
database/01_create_tables.sql
```

## 4. Testar CRUD

No Swagger:

### Criar pedido

`POST /api/pedidos`

```json
{
  "clienteNome": "João da Silva"
}
```

Copie o `id` retornado.

### Criar item

`POST /api/pedidos/{pedidoId}/itens`

```json
{
  "descricao": "Assinatura Dimdim",
  "quantidade": 2,
  "precoUnitario": 19.90
}
```

### Consultar

`GET /api/pedidos/{id}`

### Atualizar

`PUT /api/pedidos/{id}`

```json
{
  "clienteNome": "João Silva Atualizado"
}
```

### Excluir

`DELETE /api/pedidos/{id}`

A exclusão do pedido também remove os itens pela FK com `ON DELETE CASCADE`.

## 5. Conferir banco

Execute:

```text
database/03_verification.sql
```

A consulta final permite comparar `ValorTotal` do pedido com a soma dos itens.

## 6. Publicar no Azure

Antes de executar o script:

- faça `az login`;
- altere a senha SQL;
- confirme que o Docker Desktop está ativo.

Linux/macOS:

```bash
./scripts/azure-deploy.sh
```

PowerShell:

```powershell
.\scriptszure-deploy.ps1
```

## 7. Criar as tabelas no Azure SQL

Depois que o Azure SQL estiver criado, conecte-se usando o servidor informado no portal/Azure CLI e execute:

```text
database/01_create_tables.sql
```

## 8. Application Insights

No recurso criado pelo script, copie a connection string do Application Insights e configure-a como:

`APPLICATIONINSIGHTS_CONNECTION_STRING`

no App Service.

## 9. Evidências para a apresentação

Recomenda-se capturar:

- Swagger da API online;
- resposta do POST;
- resposta do GET;
- resposta do PUT;
- resposta do DELETE;
- consulta das tabelas no Azure SQL após cada operação;
- `/health`;
- Application Insights.

## 10. Observação

Não coloque senha real do Azure SQL no GitHub. O valor `<ALTERE_ANTES_DE_EXECUTAR>` deve ser substituído apenas no ambiente local.
