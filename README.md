# API de Calidad del Aire

## Descripción
API REST desarrollada con ASP.NET Core .NET 10 para gestionar información proveniente de sensores de calidad del aire instalados en una planta industrial.
Permite registrar lecturas, generar alertas automáticas y consultar información meteorológica externa.

## Tecnologías utilizadas

- C# y ASP.NET Core Web API.
- .NET 10.
- Entity Framework Core.
- MySQL 8.4.
- Docker.
- Autenticación JWT.
- Swagger / OpenAPI.
- Open-Meteo API.

## Requisitos

- .NET SDK 10.
- Docker Desktop.
- MySQL 8.4.
- Visual Studio 2026 o editor compatible.


## Configuración

### 1. Clonar el repositorio

```bash
git clone https://github.com/JuanReyes1235/APICalidadAire.git
```

### 2. Iniciar MySQL

Iniciar un contenedor MySQL 8.4 mediante Docker Desktop y verificar que el puerto 3306 esté disponible.

### 3. Configurar User Secrets

Desde la carpeta del proyecto, ejecutar:

```bash
dotnet user-secrets init
```

Configurar la conexión a MySQL:

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Port=3306;Database=CalidadAireDB;User=root;Password=TU_CONTRASEÑA;"
```

Configurar los parámetros JWT:

```bash
dotnet user-secrets set "Jwt:Key" "TU_CLAVE_SECRETA"
dotnet user-secrets set "Jwt:Issuer" "APICalidadAire"
dotnet user-secrets set "Jwt:Audience" "APICalidadAireUsuarios"
dotnet user-secrets set "Jwt:AccessTokenMinutes" "60"
```

No publicar las credenciales reales.

### 4. Restaurar paquetes

```bash
dotnet restore
```

### 5. Crear las tablas

```bash
dotnet ef database update
```

### 6. Ejecutar la API

```bash
dotnet run
```

Swagger estará disponible en la dirección HTTPS indicada por la aplicación, agregando `/swagger`.

### 7. Script SQL

El script de creación de tablas y relaciones se encuentra en la carpeta `SQL`.

## Endpoints principales

### Autenticación
- POST /api/auth/registrar
- POST /api/auth/login

### Sensores

- GET /api/sensores
- GET /api/sensores/{id}
- POST /api/sensores
- PUT /api/sensores/{id}
- DELETE /api/sensores/{id}

### Lecturas

- GET /api/lecturas
- GET /api/lecturas/{id}
- POST /api/lecturas
- GET /api/lecturas/{id}/clima

### Alertas

- GET /api/alertas
- GET /api/alertas/{id}

## Seguridad

Los endpoints de sensores, lecturas y alertas requieren autenticación JWT.
Las contraseñas se almacenan mediante hashing.

## Alertas automáticas

La API evalúa las mediciones y genera alertas de nivel:
- Leve.
- Moderada.
- Crítica.
- Extrema.

## Servicio meteorológico

Se utiliza Open-Meteo para consultar temperatura y humedad mediante coordenadas geográficas.
Los datos meteorológicos corresponden a las condiciones actuales de la ubicación consultada.

## Base de datos
El proyecto utiliza MySQL y Entity Framework Core.
El script SQL se encuentra en la carpeta SQL.