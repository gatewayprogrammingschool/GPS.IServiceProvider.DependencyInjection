param(
    [string]$token=$null,
    [switch]$WhatIf=$false
)

$exitCode = 0;

function Process-ExitCode {
    param(
        [int]$LEC,
        [string]$ErrorDescription)

    $exitCode = $LEC

    if($exitCode -ne 0) {
        throw "${exitCode}: ${ErrorDescription}"
    } else {
        # "`$exitCode: $exitCode"
    }
}

Push-Location

try {
    if((-not $token) -or ($token.Length -eq 0)) {
        throw "No Nuget Token was supplied."
    }

    $project = Get-ChildItem -Path ./src *.csproj -Recurse -ErrorAction Stop

    if($project) {
        $csproj = $project.FullName
        "Building [$csproj]..."

        & dotnet build $csproj -c Release --no-restore #--nologo -v quiet

        Process-ExitCode $LASTEXITCODE "Failed to build [${project.FullName}]."

        "Getting Packages..."

        $packages = Get-ChildItem *.symbols.nupkg -Path ./src -Recurse -ErrorAction Stop -Verbose

        if((-not $packages) -or ($packages.Length -eq 0)) {
            throw "No packages were built for [$csproj]."
        }

        "Packages to publish..."

        $packages `
            | Sort-Object Directory, Name `
            | Format-Table Name, Directory

        $packages `
            | ForEach-Object -Verbose -Process {
                $package = $_

                try {
                    $fullName = $_.FullName

                    if(!$WhatIf) {
                        & dotnet nuget push $FullName -k "${token}" -s 'https://api.nuget.org/v3/index.json' --skip-duplicate
                    } else {
                        "WhatIf: & dotnet nuget push $FullName -k `"${token}`" -s 'https://api.nuget.org/v3/index.json' --skip-duplicate"
                    }

                    Process-ExitCode $LASTEXITCODE "Failed to build [${project.FullName}]."
                }
                catch {
                    throw $_
                }
            }

        "Publish Finished Successfully."
    } 
    else {
        throw "No csproj file in $PWD (recursive)."
    }
}
catch {
    if($exitCode -eq 0) { $exitCode = $LASTEXITCODE }
    $err = $_
    $err
    exit $exitCode
}
finally {
    Pop-Location
}