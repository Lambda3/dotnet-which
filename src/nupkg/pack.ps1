Push-Location $PSScriptRoot\..\dotnet-which
dotnet pack -c Release -o ..\nupkg
Pop-Location
