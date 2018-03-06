#!/bin/sh
set -e
cd `dirname $0`

dotnet test --configuration Release --no-build Axoom.Extensions.Logging.UnitTests/Axoom.Extensions.Logging.UnitTests.csproj
