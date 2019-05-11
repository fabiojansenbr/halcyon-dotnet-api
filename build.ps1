dotnet tool install --global Amazon.Lambda.Tools

dotnet restore
dotnet lambda package --configuration release --framework netcoreapp2.1 --output-package Halcyon.Api/bin/release/netcoreapp2.1/deploy-package.zip
serverless deploy