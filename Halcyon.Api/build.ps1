dotnet tool install --global Amazon.Lambda.Tools

pushd Halcyon.Api

dotnet restore
dotnet lambda package --configuration release --framework netcoreapp2.1 --output-package bin/release/netcoreapp2.1/deploy-package.zip
serverless deploy

popd
