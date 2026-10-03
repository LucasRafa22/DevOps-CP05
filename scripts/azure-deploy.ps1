$ErrorActionPreference = "Stop"

$RESOURCE_GROUP = "rg-dimdim-devops"
$LOCATION = "brazilsouth"
$ACR_NAME = "dimdimacr$((Get-Random -Minimum 1000 -Maximum 9999))"
$PLAN_NAME = "dimdim-plan"
$APP_NAME = "dimdim-api-$((Get-Random -Minimum 1000 -Maximum 9999))"
$SQL_SERVER = "dimdim-sql-$((Get-Random -Minimum 1000 -Maximum 9999))"
$SQL_DB = "DimdimDb"
$SQL_ADMIN = "dimdimadmin"
$SQL_PASSWORD = "<ALTERE_ANTES_DE_EXECUTAR>"

az login
az group create --name $RESOURCE_GROUP --location $LOCATION
az acr create --resource-group $RESOURCE_GROUP --name $ACR_NAME --sku Basic
az acr login --name $ACR_NAME

docker build -f src/Dimdim.Api/Dockerfile -t "$ACR_NAME.azurecr.io/dimdim-api:latest" .
docker push "$ACR_NAME.azurecr.io/dimdim-api:latest"

az sql server create --name $SQL_SERVER --resource-group $RESOURCE_GROUP --location $LOCATION --admin-user $SQL_ADMIN --admin-password $SQL_PASSWORD
az sql db create --resource-group $RESOURCE_GROUP --server $SQL_SERVER --name $SQL_DB --service-objective S0
az sql server firewall-rule create --resource-group $RESOURCE_GROUP --server $SQL_SERVER --name AllowAzureServices --start-ip-address 0.0.0.0 --end-ip-address 0.0.0.0

az monitor app-insights component create --app dimdim-insights --location $LOCATION --resource-group $RESOURCE_GROUP --application-type web
az appservice plan create --name $PLAN_NAME --resource-group $RESOURCE_GROUP --is-linux --sku B1
az webapp create --resource-group $RESOURCE_GROUP --plan $PLAN_NAME --name $APP_NAME --deployment-container-image-name "$ACR_NAME.azurecr.io/dimdim-api:latest"
$ACR_USER = az acr credential show --name $ACR_NAME --query username -o tsv
$ACR_PASS = az acr credential show --name $ACR_NAME --query "passwords[0].value" -o tsv
az webapp config container set --resource-group $RESOURCE_GROUP --name $APP_NAME --docker-custom-image-name "$ACR_NAME.azurecr.io/dimdim-api:latest" --docker-registry-server-url "https://$ACR_NAME.azurecr.io" --docker-registry-server-user $ACR_USER --docker-registry-server-password $ACR_PASS
$INSIGHTS_CONNECTION = az monitor app-insights component show --app dimdim-insights --resource-group $RESOURCE_GROUP --query connectionString -o tsv
az webapp config appsettings set --resource-group $RESOURCE_GROUP --name $APP_NAME --settings WEBSITES_PORT=8080 APPLICATIONINSIGHTS_CONNECTION_STRING=$INSIGHTS_CONNECTION

$CONNECTION = "Server=tcp:$SQL_SERVER.database.windows.net,1433;Initial Catalog=$SQL_DB;Persist Security Info=False;User ID=$SQL_ADMIN;Password=$SQL_PASSWORD;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
az webapp config connection-string set --resource-group $RESOURCE_GROUP --name $APP_NAME --connection-string-type SQLAzure --settings DimdimDb=$CONNECTION

Write-Host "API: https://$APP_NAME.azurewebsites.net"
Write-Host "Swagger: https://$APP_NAME.azurewebsites.net/swagger"
Write-Host "Health: https://$APP_NAME.azurewebsites.net/health"
