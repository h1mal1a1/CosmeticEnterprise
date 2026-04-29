param(
    [string]$env = "dev"
)

if ($env -ne "dev" -and $env -ne "prod") {
    Write-Host "Invalid environment. Use 'dev' or 'prod'"
    exit 1
}

$composeFile = if ($env -eq "prod") {
    "docker-compose.prod.yml"
} else {
    "docker-compose.dev.yml"
}

Write-Host "Using environment: $env"
Write-Host "Compose file: $composeFile"

Write-Host "Stopping containers..."
docker compose -f $composeFile down --remove-orphans

Write-Host "Removing volumes..."
docker compose -f $composeFile down -v

Write-Host "Starting containers..."
docker compose -f $composeFile up --build -d

Write-Host "Done!"