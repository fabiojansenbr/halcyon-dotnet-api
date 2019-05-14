#!/bin/bash

# install aws cli
sudo pip3 install --upgrade pip
pip3 install awscli --upgrade --user
export PATH="/home/vsts/.local/bin:$PATH"

# install dotnet lambda tools
dotnet tool install --global Amazon.Lambda.Tools

pushd Halcyon.Api

dotnet restore
dotnet lambda package --configuration release --framework netcoreapp2.1 --output-package bin/deploy-package.zip

# deploy serverless project
npx serverless deploy --stage $AWS_STAGE --region $AWS_DEFAULT_REGION

popd
