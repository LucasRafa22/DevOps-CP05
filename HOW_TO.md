# 🚀 How-To: Deploy do Projeto Dimdim na Azure

## 📌 Pré-requisitos

* Azure CLI instalado
* Conta Azure ativa
* .NET 9 instalado
* PowerShell ou terminal

---

## 🔹 1. Criar Resource Group

```bash
az group create --name rg-dimdim-webapp --location southafricanorth
```

---

## 🔹 2. Criar Azure SQL Server

```bash
az sql server create ^
  --name sql-server-dimdim-rm565194 ^
  --resource-group rg-dimdim-webapp ^
  --location southafricanorth ^
  --admin-user user-dimdim ^
  --admin-password "SUA_SENHA_FORTE" ^
  --enable-public-network true
```

---

## 🔹 3. Criar Banco de Dados

```bash
az sql db create ^
  --resource-group rg-dimdim-webapp ^
  --server sql-server-dimdim-rm565194 ^
  --name db-dimdim ^
  --service-objective Basic
```

---

## 🔹 4. Liberar Acesso (Firewall)

```bash
az sql server firewall-rule create ^
  --resource-group rg-dimdim-webapp ^
  --server sql-server-dimdim-rm565194 ^
  --name liberaGeral ^
  --start-ip-address 0.0.0.0 ^
  --end-ip-address 255.255.255.255
```

---

## 🔹 5. Criar Tabelas no Banco

```powershell
Invoke-Sqlcmd -ServerInstance "sql-server-dimdim-rm565194.database.windows.net" `
  -Database "db-dimdim" `
  -Username "user-dimdim" `
  -Password "SUA_SENHA_FORTE" `
  -Query @"
CREATE TABLE Pedidos (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    ClienteNome NVARCHAR(150),
    DataCriacao DATETIME,
    ValorTotal DECIMAL(10,2)
);

CREATE TABLE ItensPedido (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    PedidoId UNIQUEIDENTIFIER,
    Descricao NVARCHAR(200),
    Quantidade INT,
    PrecoUnitario DECIMAL(10,2),
    FOREIGN KEY (PedidoId) REFERENCES Pedidos(Id)
);
"@
```

---

## 🔹 6. Criar App Service Plan

```bash
az appservice plan create ^
  --name plan-dimdim ^
  --resource-group rg-dimdim-webapp ^
  --sku B1 ^
  --is-linux
```

---

## 🔹 7. Criar Web App

```bash
az webapp create ^
  --resource-group rg-dimdim-webapp ^
  --plan plan-dimdim ^
  --name dimdim-api-rm565194 ^
  --runtime "DOTNETCORE:9"
```

---

## 🔹 8. Configurar Connection String

```bash
az webapp config connection-string set ^
  --resource-group rg-dimdim-webapp ^
  --name dimdim-api-rm565194 ^
  --settings DimdimDb="Server=tcp:sql-server-dimdim-rm565194.database.windows.net,1433;Initial Catalog=db-dimdim;User ID=user-dimdim;Password=SUA_SENHA_FORTE;Encrypt=True;" ^
  --connection-string-type SQLAzure
```

---

## 🔹 9. Configurar Application Insights

```bash
az monitor app-insights component create ^
  --app insights-dimdim ^
  --location southafricanorth ^
  --resource-group rg-dimdim-webapp
```

Obter Connection String:

```bash
az monitor app-insights component show ^
  --app insights-dimdim ^
  --resource-group rg-dimdim-webapp ^
  --query connectionString ^
  --output tsv
```

Configurar no Web App:

```bash
az webapp config appsettings set ^
  --resource-group rg-dimdim-webapp ^
  --name dimdim-api-rm565194 ^
  --settings APPLICATIONINSIGHTS_CONNECTION_STRING="COLE_AQUI"
```

---

## 🔹 10. Publicar API

```bash
dotnet publish src\Dimdim.Api\Dimdim.Api.csproj -c Release -o .\dist
```

Compactar:

```bash
tar -a -cf deploy-linux.zip -C .\dist .
```

Deploy:

```bash
az webapp deploy ^
  --resource-group rg-dimdim-webapp ^
  --name dimdim-api-rm565194 ^
  --src-path deploy-linux.zip ^
  --type zip
```

---

## 🔹 11. Testar API

Swagger:

```
https://dimdim-api-rm565194.azurewebsites.net/swagger
```

Health Check:

```
https://dimdim-api-rm565194.azurewebsites.net/health
```

---

## 🔹 12. Validar Persistência

Executar no banco:

```sql
SELECT * FROM Pedidos;
SELECT * FROM ItensPedido;
```

Realizar:

* POST
* GET
* PUT
* DELETE

E verificar os dados no banco após cada operação.

---

## ✅ Conclusão

Após esses passos, a aplicação estará:

* Publicada na Azure
* Conectada ao banco
* Com CRUD funcional
* Monitorada via Application Insights
