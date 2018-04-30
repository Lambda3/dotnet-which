Push-Location $PSScriptRoot\..\dotnet-which
dotnet pack -c Release -o ..\nupkg
if ($LASTEXITCODE -ne 0) { exit }
Pop-Location
if ((dotnet tool list -g | Where-Object { $_.Contains('dx') })) {
    dotnet tool uninstall -g dotnet-which
}
dotnet tool install -g dotnet-which
