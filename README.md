# Smarthome backend API

## Tech stack
- Minimal API
- .NET versie 10.0.400

## Getting started

### 1. Clone de repository
```bash
git clone https://github.com/Smartenvironment5/smarthome-backend
cd smarthome-backend
```

### 2. Installeer just (optioneel)
```bash
winget install Casey.Just
```
Hiermee kan je scripts runnen gedefineerd in de `justfile` file

### 3. Download docker
windows: https://docs.docker.com/desktop/setup/install/windows-install/

### 4. Start de database
```bash
just db-up
```

### 5. Start de API
```bash
just run
```

- Open de swagger UI: [**klik hier**](http://localhost:5100/swagger/index.html)
