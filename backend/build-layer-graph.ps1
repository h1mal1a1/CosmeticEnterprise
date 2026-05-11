$projectRoot = Get-Location

$layers = @(
    "Api",
    "Application",
    "Domain",
    "Infrastructure"
)

$namespaceRoot = "CosmeticEnterpriseBack"

$goodDependencies = @(
    "Api|Application",
    "Application|Domain",
    "Infrastructure|Application",
    "Infrastructure|Domain"
)

$acceptableDependencies = @(
    "Api|Infrastructure",
    "Api|Domain"
)

$files = Get-ChildItem -Path $projectRoot -Recurse -Filter *.cs |
    Where-Object {
        $_.FullName -notmatch "\\bin\\" -and
        $_.FullName -notmatch "\\obj\\" -and
        $_.FullName -notmatch "\\Migrations\\"
    }

$dependencies = @{}

foreach ($file in $files) {
    $relativePath = $file.FullName.Replace($projectRoot.Path + "\", "")
    $sourceLayer = $null

    foreach ($layer in $layers) {
        if ($relativePath.StartsWith($layer + "\")) {
            $sourceLayer = $layer
            break
        }
    }

    if (-not $sourceLayer) {
        continue
    }

    $content = Get-Content $file.FullName

    foreach ($line in $content) {
        foreach ($targetLayer in $layers) {
            $pattern = "using $namespaceRoot.$targetLayer"

            if ($line.Trim().StartsWith($pattern)) {
                if ($sourceLayer -ne $targetLayer) {
                    $key = "$sourceLayer|$targetLayer"

                    if (-not $dependencies.ContainsKey($key)) {
                        $dependencies[$key] = 0
                    }

                    $dependencies[$key]++
                }
            }
        }
    }
}

$dotLines = @()
$dotLines += "digraph BackendLayerDependencies {"
$dotLines += "    rankdir=LR;"
$dotLines += "    graph [charset=""UTF-8""];"
$dotLines += "    node [shape=box, style=""rounded,filled"", fillcolor=""#f5f5f5"", fontname=""Arial""];"
$dotLines += "    edge [fontsize=10, fontname=""Arial""];"
$dotLines += ""

foreach ($layer in $layers) {
    $dotLines += '    "{0}";' -f $layer
}

$dotLines += ""

foreach ($dependency in $dependencies.GetEnumerator() | Sort-Object Name) {
    $parts = $dependency.Name -split "\|"
    $from = $parts[0]
    $to = $parts[1]
    $count = $dependency.Value

    if ($goodDependencies -contains $dependency.Name) {
        $color = "#2e7d32"
    }
    elseif ($acceptableDependencies -contains $dependency.Name) {
        $color = "#ef6c00"
    }
    else {
        $color = "#c62828"
    }

    $dotLines += '    "{0}" -> "{1}" [label="{2}", color="{3}", fontcolor="{3}", penwidth=2];' -f $from, $to, $count, $color
}

$dotLines += "}"

$dotPath = Join-Path $projectRoot "backend-layer-dependencies.dot"
$svgPath = Join-Path $projectRoot "backend-layer-dependencies.svg"

[System.IO.File]::WriteAllLines($dotPath, $dotLines, [System.Text.UTF8Encoding]::new($false))

dot -Tsvg $dotPath -o $svgPath

Write-Host ""
Write-Host "Graph created:"
Write-Host $svgPath
Write-Host ""

Write-Host "Found dependencies:"
$dependencies.GetEnumerator() |
    Sort-Object Name |
    ForEach-Object {
        Write-Host "$($_.Name.Replace('|', '->')): $($_.Value)"
    }