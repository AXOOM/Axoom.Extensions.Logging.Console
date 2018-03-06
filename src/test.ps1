$ErrorActionPreference = "Stop"
pushd $(Split-Path -Path $MyInvocation.MyCommand.Definition -Parent)

dotnet test --configuration Release --no-build Axoom.Extensions.Logging.Console.UnitTests\Axoom.Extensions.Logging.UnitTests.csproj

popd
