set windows-shell := ["powershell.exe", "-NoLogo", "-Command"]

default:
    @just --list

restore:
    dotnet tool restore
    dotnet restore

build:
    dotnet build --no-restore

run:
    dotnet run --project SmartHome.Api

watch:
    dotnet watch --project SmartHome.Api

test:
    dotnet test

db-up:
    docker compose up -d

db-down:
    docker compose down
    
db-reset:
	docker compose down -v
	docker compose up -d --wait

migrate name:
	dotnet ef migrations add {{name}} --project src/SmartHome.Api

db-update:
	dotnet ef database update --project src/SmartHome.Api