/* Ejecutar con permisos para crear base de datos. Compatible con SQL Server 2012+. */

PRINT 'CREANDO BD PruebaTecnicaValueTech'
IF DB_ID(N'PruebaTecnicaValueTech') IS NULL CREATE DATABASE PruebaTecnicaValueTech;
GO
USE PruebaTecnicaValueTech;
GO
IF OBJECT_ID('dbo.Region', 'U') IS NULL
BEGIN
	PRINT 'CREANDO TABLA Region'
    CREATE TABLE dbo.Region
    (
        IdRegion INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Region PRIMARY KEY,
        Region NVARCHAR(128) NOT NULL
    );
END;
GO
IF OBJECT_ID('dbo.Comuna', 'U') IS NULL
BEGIN
	PRINT 'CREANDO TABLA Comuna'
    CREATE TABLE dbo.Comuna
    (
        IdComuna INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Comuna PRIMARY KEY,
        IdRegion INT NOT NULL,
        Comuna NVARCHAR(128) NOT NULL,
        InformacionAdicional XML NULL,
        CONSTRAINT FK_Comuna_Region FOREIGN KEY (IdRegion) REFERENCES dbo.Region(IdRegion)
    );
    CREATE INDEX IX_Comuna_IdRegion ON dbo.Comuna(IdRegion);
END;
GO

PRINT 'EJECUCIÓN EXITOSA'