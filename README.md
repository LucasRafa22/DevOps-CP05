# 💰 Dimdim WebApp - Checkpoint 2

## 📌 Descrição do Projeto

Este projeto consiste no desenvolvimento de uma aplicação Web API em .NET 9, com o objetivo de gerenciar pedidos e seus respectivos itens, seguindo o modelo de relacionamento master-detail. A aplicação foi projetada para ser executada em ambiente de nuvem, utilizando serviços da Microsoft Azure.

A API foi publicada em um Web App na Azure e realiza operações completas de CRUD (Create, Read, Update e Delete), com persistência de dados em um banco Azure SQL Database. O banco de dados contém as tabelas **Pedidos** e **ItensPedido**, relacionadas por chave estrangeira, garantindo a integridade dos dados.

Todo o provisionamento da infraestrutura foi realizado via **Azure CLI**, incluindo a criação do Resource Group, Azure SQL Server, banco de dados e Web App. Além disso, foi configurado o **Application Insights** para monitoramento da aplicação.

---

## 👨‍💻 Integrantes

* Lucas Rafael Solimene – RM 565194

---

## 🌐 Link do Projeto

🔗 https://github.com/LucasRafa22/DevOps-CP05

---

## 🧱 Tecnologias Utilizadas

* .NET 9
* ASP.NET Web API
* Azure Web App
* Azure SQL Database
* Azure CLI
* Application Insights

---

## 🗄️ Banco de Dados

### Tabelas

* **Pedidos**
* **ItensPedido**

### Relacionamento

```text
Pedidos (1) → (N) ItensPedido
```

---

## 🔁 Endpoints Principais

### POST /api/pedidos

Cria um novo pedido

```json
{
  "clienteNome": "Lucas Rafael"
}
```

---

### GET /api/pedidos

Retorna todos os pedidos

---

### PUT /api/pedidos/{id}

```json
{
  "clienteNome": "Lucas Atualizado"
}
```

---

### DELETE /api/pedidos/{id}

Remove um pedido

---

## ❤️ Health Check

```
GET /health
```

---

## 📊 Monitoramento

A aplicação utiliza **Application Insights** para monitoramento de requisições, desempenho e falhas.

---

## ⚙️ Deploy na Azure

Resumo dos principais comandos:

```bash
az group create
az sql server create
az sql db create
az webapp create
az webapp deploy
```

📌 O passo a passo completo está disponível no arquivo **HOW_TO.md**

---

## 📂 Estrutura do Projeto

```
/src
/docs
/scripts
/database
README.md
HOW_TO.md
```

---

## 📘 Observações

* A aplicação foi validada com persistência real no Azure SQL Database
* Todas as operações CRUD foram testadas via Swagger
* Evidências encontram-se no PDF da entrega
