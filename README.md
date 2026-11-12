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

1. Clonar el repositorio.
2. Iniciar el contenedor MySQL.
3. Configurar la cadena de conexión en appsettings.json.
4. Configurar los parámetros JWT.
5. Restaurar los paquetes NuGet.

```bash
dotnet restore
```

6. Crear la base de datos mediante migraciones:

```bash
dotnet ef database update
```

7. Ejecutar el proyecto:

```bash
dotnet run
```

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