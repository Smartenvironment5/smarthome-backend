# SmartHome backend API

## Tech stack
- Minimal API
- .NET versie 10.0.400

## Getting started

### 1. Clone de repository
```bash
git clone https://github.com/Smartenvironment5/SmartHome-backend
cd SmartHome-backend
```

### 2. Set de api key
```bash
dotnet user-secrets set "UrbanSense:Key" "the-key" --project src/SmartHome.Api
```

### 3. Installeer just (optioneel)
```bash
winget install Casey.Just
```
Hiermee kan je scripts runnen gedefineerd in de `justfile` file

### 4. Download docker
windows: https://docs.docker.com/desktop/setup/install/windows-install/

### 5. Start de database
```bash
just db-up
```

### 6. Start de API
```bash
just run
```

- Open de swagger UI: [**klik hier**](http://localhost:5100/swagger/index.html)
/..

packages commands:

dotnet add package Microsoft.Extensions.Validation --version 10.0.0
dotnet add package Microsoft.AspNetCore.OpenApi --version 10.0.0
dotnet add package Swashbuckle.AspNetCore.SwaggerUI --version 10.0.0
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL --version 10.0.0
dotnet add package Microsoft.EntityFrameworkCore.Design --version 10.0.0

maak smarthome aan in pgadmin en voer dit uit in de terminal
dotnet user-secrets set "ConnectionStrings:smarthome" "Host=localhost;Port=5432;Database=smarthome;Username=JOUW_USERNAME;Password=JOUW_WACHTWOORD"
