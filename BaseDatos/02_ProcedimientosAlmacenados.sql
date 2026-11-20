USE PruebaTecnicaRegiones
GO
/**************************************************************

Nombre: usp_PTR_Region_ObtenerTodos
Fecha: 18-11-2026
Creado por: Patricio Ortíz
Proyecto: Pruebas Técnicas Regiones (Valuetech)
Descripción: Obtiene Listado completo de las regiones 
Ejecución: EXEC usp_PTR_Region_ObtenerTodos
@prm1,
@prm2...

***************************************************************/
CREATE OR ALTER PROCEDURE dbo.usp_PTR_Region_ObtenerTodos
AS
BEGIN
    SET NOCOUNT ON
    SELECT IdRegion, Region AS RegionNombre 
	FROM dbo.Region 
	ORDER BY IdRegion
END
GO

/**************************************************************

Nombre: usp_PTR_Region_ObtenerPorId
Fecha: 18-11-2026
Creado por: Patricio Ortíz
Proyecto: Pruebas Técnicas Regiones (Valuetech)
Descripción: Obtiene Información de 1 Región
Ejecución: EXEC usp_PTR_Region_ObtenerPorId
@prm1,
@prm2...

***************************************************************/
CREATE OR ALTER PROCEDURE dbo.usp_PTR_Region_ObtenerPorId
    @IdRegion INT
AS
BEGIN
    SET NOCOUNT ON
    SELECT IdRegion, Region AS RegionNombre 
	FROM dbo.Region 
	WHERE 
	IdRegion = @IdRegion
END
GO

/**************************************************************

Nombre: usp_PTR_Comuna_ObtenerPorRegion
Fecha: 18-11-2026
Creado por: Patricio Ortíz
Proyecto: Pruebas Técnicas Regiones (Valuetech)
Descripción: Obtiene listado de Comunas de la región especificada 
Ejecución: EXEC usp_PTR_Comuna_ObtenerPorRegion
@prm1,
@prm2...

***************************************************************/
CREATE OR ALTER PROCEDURE dbo.usp_PTR_Comuna_ObtenerPorRegion
    @IdRegion INT
AS
BEGIN
    SET NOCOUNT ON
    SELECT IdComuna, IdRegion, Comuna AS ComunaNombre,
           CONVERT(NVARCHAR(MAX), InformacionAdicional) AS InformacionAdicional
    FROM dbo.Comuna 
	WHERE 
	IdRegion = @IdRegion 
	ORDER BY Comuna
END
GO

/**************************************************************

Nombre: usp_PTR_Comuna_ObtenerPorId
Fecha: 18-11-2026
Creado por: Patricio Ortíz
Proyecto: Pruebas Técnicas Regiones (Valuetech)
Descripción: Obtiene Información de 1 Comuna
Ejecución: EXEC usp_PTR_Comuna_ObtenerPorId
@prm1,
@prm2...

***************************************************************/
CREATE OR ALTER PROCEDURE dbo.usp_PTR_Comuna_ObtenerPorId
    @IdRegion INT,
    @IdComuna INT
AS
BEGIN
    SET NOCOUNT ON
    SELECT IdComuna, IdRegion, Comuna AS ComunaNombre,
           CONVERT(NVARCHAR(MAX), InformacionAdicional) AS InformacionAdicional
    FROM dbo.Comuna 
	WHERE 
	IdRegion = @IdRegion AND 
	IdComuna = @IdComuna
END
GO

/**************************************************************

Nombre: usp_PTR_Comuna_Actualizar
Fecha: 18-11-2026
Creado por: Patricio Ortíz
Proyecto: Pruebas Técnicas Regiones (Valuetech)
Descripción: Actualiza la información de la comuna dada
Ejecución: EXEC usp_PTR_Comuna_Actualizar
@prm1,
@prm2...

***************************************************************/
CREATE OR ALTER PROCEDURE dbo.usp_PTR_Comuna_Actualizar
    @IdRegion INT,
    @IdComuna INT,
    @Comuna NVARCHAR(128),
    @Superficie DECIMAL(18,2),
    @Poblacion INT
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON
    IF @Superficie <= 0 OR @Poblacion < 0 OR LEN(LTRIM(RTRIM(@Comuna))) = 0
        THROW 50001, 'Los datos de la comuna no son válidos.', 1

    DECLARE @Densidad DECIMAL(18,2) = ROUND(CONVERT(DECIMAL(18,2), @Poblacion) / @Superficie, 2)
    DECLARE @Info XML = (SELECT @Superficie AS Superficie,
                                (SELECT @Densidad AS [@Densidad], @Poblacion AS [text()] FOR XML PATH('Poblacion'), TYPE)
                         FOR XML PATH('Info'), TYPE)

    MERGE dbo.Comuna AS destino
    USING (SELECT @IdComuna AS IdComuna, @IdRegion AS IdRegion, @Comuna AS Comuna, @Info AS InformacionAdicional) AS origen
       ON destino.IdComuna = origen.IdComuna AND destino.IdRegion = origen.IdRegion
    WHEN MATCHED THEN UPDATE SET Comuna = origen.Comuna, InformacionAdicional = origen.InformacionAdicional;

    SELECT CASE WHEN @@ROWCOUNT > 0 THEN 1 ELSE 0 END
END
GO
