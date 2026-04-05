Write-Host "Stopping containers..."
docker compose down --remove-orphans

Write-Host "Removing volumes..."
docker compose down -v

Write-Host "Starting containers..."
docker compose up --build -d

Write-Host "Done!"