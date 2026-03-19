# ETL Extract - Worker Service (.NET 8)

## Descripción
Este proyecto implementa la fase de extracción (ETL) utilizando un Worker Service en .NET 8.

## Arquitectura
- CSV (Customers, Orders, Products, OrderDetails)
- API REST
- Base de datos

## Salida
- Base de datos Staging (StagingDB)

## Tecnologías
- .NET 8
- SQL Server
- CsvHelper
- HttpClient
- ILogger

## Ejecución
1. Configurar cadena de conexión en appsettings.json
2. Ejecutar el proyecto
3. Verificar datos en tablas Staging
