#!/usr/bin/env bash
set -e

RESOURCE_GROUP="rg-dimdim-devops"
LOCATION="brazilsouth"
ACR_NAME="dimdimacr$RANDOM"
PLAN_NAME="dimdim-plan"
APP_NAME="dimdim-api-$RANDOM"
SQL_SERVER="dimdim-sql-$RANDOM"
SQL_DB="DimdimDb"
SQL_ADMIN="dimdimadmin"
SQL_PASSWORD="<ALTERE_ANTES_DE_EXECUTAR>"
APPINSIGHTS_NAME="dimdim-insights"

az login

az group create --name "$RESOURCE_GROUP" --location "$LOCATION"

az acr create --resource-group "$RESOURCE_GROUP" --name "$ACR_NAME" --sku Basic
az acr login --name "$ACR_NAME"

docker build -f src/Dimdim.Api/Dockerfile -t "$ACR_NAME.azurecr.io/dimdim-api:latest" .
docker push "$ACR_NAME.azurecr.io/dimdim-api:latest"

az sql server create   --name "$SQL_SERVER"   --resource-group "$RESOURCE_GROUP"   --location "$LOCATION"   --admin-user "$SQL_ADMIN"   --admin-password "$SQL_PASSWORD"

az sql db create   --resource-group "$RESOURCE_GROUP"   --server "$SQL_SERVER"   --name "$SQL_DB"   --service-objective S0

az sql server firewall-rule create   --resource-group "$RESOURCE_GROUP"   --server "$SQL_SERVER"   --name AllowAzureServices   --start-ip-address 0.0.0.0   --end-ip-address 0.0.0.0

az monitor app-insights component create   --app "$APPINSIGHTS_NAME"   --location "$LOCATION"   --resource-group "$RESOURCE_GROUP"   --application-type web

az appservice plan create   --name "$PLAN_NAME"   --resource-group "$RESOURCE_GROUP"   --is-linux   --sku B1

az webapp create   --resource-group "$RESOURCE_GROUP"   --plan "$PLAN_NAME"   --name "$APP_NAME"   --deployment-container-image-name "$ACR_NAME.azurecr.io/dimdim-api:latest"

az webapp config appsettings set   --resource-group "$RESOURCE_GROUP"   --name "$APP_NAME"   --settings WEBSITES_PORT=8080              ApplicationInsightsAgent_EXTENSION_VERSION=~3

CONNECTION="Server=tcp:$SQL_SERVER.database.windows.net,1433;Initial Catalog=$SQL_DB;Persist Security Info=False;User ID=$SQL_ADMIN;Password=$SQL_PASSWORD;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"

az webapp config connection-string set   --resource-group "$RESOURCE_GROUP"   --name "$APP_NAME"   --connection-string-type SQLAzure   --settings DimdimDb="$CONNECTION"

echo "API: https://$APP_NAME.azurewebsites.net"
echo "Swagger: https://$APP_NAME.azurewebsites.net/swagger"
echo "Health: https://$APP_NAME.azurewebsites.net/health"
echo "Execute database/01_create_tables.sql no Azure SQL antes de testar a API."
