USE [master];
GO

-- 1. Crear Login SQL Server
CREATE LOGIN [usr_api_regiones]
WITH
    PASSWORD = 'Regiones.Api#2026_Segura!',
    CHECK_POLICY = ON,
    CHECK_EXPIRATION = OFF,
    DEFAULT_DATABASE = [PruebaTecnicaRegiones];
GO

-- 2. Crear usuario dentro de la base de datos
USE [PruebaTecnicaRegiones];
GO

CREATE USER [usr_api_regiones]
FOR LOGIN [usr_api_regiones];
GO

-- 3. Permisos de lectura
GRANT SELECT TO [usr_api_regiones];
GO

-- 4. Permisos de ejecución de procedimientos
GRANT EXECUTE TO [usr_api_regiones];
GO