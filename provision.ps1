param
(
    $DEPLOY_RESOURCEGROUP = "halcyon",
    $DEPLOY_LOCATION = "westeurope",
    $DEPLOY_APPSERVICEPLAN = "$DEPLOY_RESOURCEGROUP-plan",
    $DEPLOY_APPSERVICE = "$DEPLOY_RESOURCEGROUP-dotnet-api",
    $DEPLOY_HOSTNAME = "$DEPLOY_APPSERVICE.chrispoulter.com",
    $SQL_SERVER = "$DEPLOY_RESOURCEGROUP-sql",
    $SQL_DATABASE = "halcyon",
    $SQL_USER = "halcyon",
    [Parameter(Mandatory = $true)]$SQL_PASSWORD = "",
    [Parameter(Mandatory = $true)]$JWT_SECURITYKEY = "",
    [Parameter(Mandatory = $true)]$SEED_EMAILADDRESS = "",
    [Parameter(Mandatory = $true)]$SEED_PASSWORD = "",
    [Parameter(Mandatory = $true)]$EMAIL_HOST = "",
    [Parameter(Mandatory = $true)]$EMAIL_PORT = "",
    [Parameter(Mandatory = $true)]$EMAIL_USERNAME = "",
    [Parameter(Mandatory = $true)]$EMAIL_PASSWORD = "",
    [Parameter(Mandatory = $true)]$EMAIL_NOREPLY = "",
    [Parameter(Mandatory = $true)]$FACEBOOK_APPID = "",
    [Parameter(Mandatory = $true)]$FACEBOOK_APPSECRET = "",
    [Parameter(Mandatory = $true)]$GOOGLE_CLIENTID = ""
)

Write-Host "Creating Resource Group..." -ForegroundColor Green
az group create `
    -l "$DEPLOY_LOCATION" `
    -g "$DEPLOY_RESOURCEGROUP"

Write-Host "Creating SQL Server..." -ForegroundColor Green
az sql server create `
    -g "$DEPLOY_RESOURCEGROUP" `
    -l "$DEPLOY_LOCATION" `
    -n "$SQL_SERVER" `
    -u "$SQL_USER" `
    -p "$SQL_PASSWORD"

Write-Host "Setting SQL Server Firewall..." -ForegroundColor Green
az sql server firewall-rule create `
    -g "$DEPLOY_RESOURCEGROUP" `
    -s "$SQL_SERVER" `
    -n "all" `
    --start-ip-address "0.0.0.0" `
    --end-ip-address "255.255.255.255"

Write-Host "Creating SQL Database..." -ForegroundColor Green
az sql db create `
    -g "$DEPLOY_RESOURCEGROUP" `
    -s "$SQL_SERVER" `
    -n "$SQL_DATABASE" `
    --service-objective "Basic"

Write-Host "Creating App Service Plan..." -ForegroundColor Green
az appservice plan create `
    -g "$DEPLOY_RESOURCEGROUP" `
    -l "$DEPLOY_LOCATION" `
    -n "$DEPLOY_APPSERVICEPLAN" `
    --sku "B1" `
    --is-linux

Write-Host "Creating Web App..." -ForegroundColor Green
az webapp create `
    -g "$DEPLOY_RESOURCEGROUP" `
    -p "$DEPLOY_APPSERVICEPLAN" `
    -n "$DEPLOY_APPSERVICE" `
    --runtime "DOTNETCORE`"|`"2.1" `
    --startup-file "dotnet Halcyon.Api.dll"

Write-Host "Setting Host Name..." -ForegroundColor Green
az webapp config hostname add `
    -g "$DEPLOY_RESOURCEGROUP" `
    --webapp-name "$DEPLOY_APPSERVICE" `
    --hostname "$DEPLOY_HOSTNAME"

$SQL_CONNECTION_STRING = "Server=tcp:$SQL_SERVER.database.windows.net,1433;Database=$SQL_DATABASE;User ID=$SQL_USER;Password=$SQL_PASSWORD;Encrypt=true;Connection Timeout=30;"
Write-Host "SQL Connection String: $SQL_CONNECTION_STRING" -ForegroundColor Yellow

Write-Host "Setting Connection String..." -ForegroundColor Green
az webapp config connection-string set `
    -g "$DEPLOY_RESOURCEGROUP" `
    -n "$DEPLOY_APPSERVICE" `
    -t "SQLAzure" `
    --settings `
    DefaultConnection="$SQL_CONNECTION_STRING"

Write-Host "Setting App Settings..." -ForegroundColor Green
az webapp config appsettings set `
    -g "$DEPLOY_RESOURCEGROUP" `
    -n "$DEPLOY_APPSERVICE" `
    --settings `
    Jwt__SecurityKey="$JWT_SECURITYKEY" `
    Seed__EmailAddress="$SEED_EMAILADDRESS" `
    Seed__Password="$SEED_PASSWORD" `
    Email__Host="$EMAIL_HOST" `
    Email__Port="$EMAIL_PORT" `
    Email__Username="$EMAIL_USERNAME" `
    Email__Password="$EMAIL_PASSWORD" `
    Email__NoReplyAddress="$EMAIL_NOREPLY" `
    Authentication__Facebook__AppId="$FACEBOOK_APPID" `
    Authentication__Facebook__AppSecret="$FACEBOOK_APPSECRET" `
    Authentication__Google__ClientId="$GOOGLE_CLIENTID"
