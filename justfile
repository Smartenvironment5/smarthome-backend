set windows-shell := ["powershell.exe", "-NoLogo", "-Command"]

default:
    @just --list

restore:
    dotnet tool restore
    dotnet restore

build:
    dotnet build --no-restore

run:
    dotnet run --project src/SmartHome.Api

watch:
    dotnet watch --project src/SmartHome.Api

test:
    dotnet test

db-up:
    docker compose up -d

db-down:
    docker compose down