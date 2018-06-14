. $PSScriptRoot\pack.ps1
if ($LASTEXITCODE -ne 0) { exit }
if ((dotnet tool list -g | Where-Object { $_.Contains('dotnet-which') })) {
    dotnet tool uninstall -g dotnet-which
}
dotnet tool install -g dotnet-which
