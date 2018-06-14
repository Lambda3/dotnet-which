Push-Location $PSScriptRoot\..\dotnet-which
dotnet pack -c Release -o $PSScriptRoot
Pop-Location
