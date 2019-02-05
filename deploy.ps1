param
(
    $DEPLOY_RESOURCEGROUP = "halcyon",
    $DEPLOY_APPSERVICE = "$DEPLOY_RESOURCEGROUP-dotnet-api"
)

Write-Host "Building..." -ForegroundColor Green
dotnet publish -c "Release"

Write-Host "Packaging..." -ForegroundColor Green
New-Item `
    -ItemType "Directory" `
    -Path "artifacts" `
    -Force

Compress-Archive `
    -Path "Halcyon.Api/bin/Release/netcoreapp2.1/publish/*" `
    -DestinationPath "artifacts/Halcyon.Api.zip" `
    -CompressionLevel "Fastest" `
    -Force

Write-Host "Deploying..." -ForegroundColor Green
az webapp deployment source config-zip `
    -g "$DEPLOY_RESOURCEGROUP" `
    -n "$DEPLOY_APPSERVICE" `
    --src "artifacts/Halcyon.Api.zip"
