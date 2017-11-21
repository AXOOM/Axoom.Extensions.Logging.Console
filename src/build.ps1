Param ([string]$Version = "0.1-debug")
$ErrorActionPreference = "Stop"
pushd $(Split-Path -Path $MyInvocation.MyCommand.Definition -Parent)

dotnet clean "Axoom.Extensions.Logging.Console.sln"
dotnet msbuild /t:Restore /t:Build /p:Configuration=Release /p:Version=$Version "Axoom.Extensions.Logging.Console.sln"
dotnet test --configuration Release --no-build "Axoom.Extensions.Logging.Console.UnitTests/Axoom.Extensions.Logging.Console.UnitTests.csproj"

popd
