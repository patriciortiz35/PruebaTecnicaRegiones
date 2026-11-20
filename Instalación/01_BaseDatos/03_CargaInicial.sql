USE PruebaTecnicaValueTech
GO
PRINT 'CARGANDO DATOS REGIONES'
INSERT INTO dbo.Region (Region)
SELECT v.Region FROM (VALUES
(N'Arica y Parinacota'),(N'Tarapacá'),(N'Antofagasta'),(N'Atacama'),(N'Coquimbo'),
(N'Valparaíso'),(N'Región Metropolitana de Santiago'),(N'Libertador General Bernardo O’Higgins'),
(N'Maule'),(N'Ñuble'),(N'Biobío'),(N'La Araucanía'),(N'Los Ríos'),(N'Los Lagos'),
(N'Aysén del General Carlos Ibáñez del Campo'),(N'Magallanes y de la Antártica Chilena')
) v(Region)
WHERE NOT EXISTS (SELECT 1 FROM dbo.Region r WHERE r.Region = v.Region)
GO

PRINT 'CARGANDO DATOS DE COMUNA (MUESTRA)'
DECLARE @Datos TABLE (Region NVARCHAR(128), Comuna NVARCHAR(128), Superficie DECIMAL(18,2), Poblacion INT)
INSERT INTO @Datos VALUES
(N'Arica y Parinacota',N'Arica',4799.40,247552),(N'Arica y Parinacota',N'Putre',5902.50,2765),
(N'Tarapacá',N'Iquique',2242.10,191468),(N'Tarapacá',N'Alto Hospicio',572.90,108375),
(N'Antofagasta',N'Antofagasta',30718.10,361873),(N'Antofagasta',N'Calama',15596.90,165731),
(N'Atacama',N'Copiapó',16681.30,153937),(N'Atacama',N'Vallenar',7084.00,53047),
(N'Coquimbo',N'La Serena',1892.80,221054),(N'Coquimbo',N'Coquimbo',1429.30,227730),
(N'Valparaíso',N'Valparaíso',401.60,296655),(N'Valparaíso',N'Viña del Mar',121.60,334248),
(N'Región Metropolitana de Santiago',N'Santiago',22.40,404495),(N'Región Metropolitana de Santiago',N'Puente Alto',88.20,568106),
(N'Libertador General Bernardo O’Higgins',N'Rancagua',260.30,241774),(N'Libertador General Bernardo O’Higgins',N'San Fernando',2441.00,73818),
(N'Maule',N'Talca',231.50,220357),(N'Maule',N'Curicó',1328.00,149136),
(N'Ñuble',N'Chillán',511.2,184739),(N'Ñuble',N'San Carlos',874.0,53905),
(N'Biobío',N'Concepción',221.6,223574),(N'Biobío',N'Los Ángeles',1748.2,202331),
(N'La Araucanía',N'Temuco',464.0,282415),(N'La Araucanía',N'Villarrica',1291.0,55748),
(N'Los Ríos',N'Valdivia',1015.0,166080),(N'Los Ríos',N'La Unión',2136.7,38991),
(N'Los Lagos',N'Puerto Montt',1673.0,245902),(N'Los Lagos',N'Osorno',951.3,161460),
(N'Aysén del General Carlos Ibáñez del Campo',N'Coyhaique',7290.0,57818),(N'Aysén del General Carlos Ibáñez del Campo',N'Aysén',29926.0,23755),
(N'Magallanes y de la Antártica Chilena',N'Punta Arenas',17846.3,131592),(N'Magallanes y de la Antártica Chilena',N'Puerto Natales',48974.2,21000)
INSERT INTO dbo.Comuna (IdRegion, Comuna, InformacionAdicional)
SELECT r.IdRegion, d.Comuna,
       (SELECT d.Superficie AS Superficie,
               (SELECT ROUND(CONVERT(DECIMAL(18,2), d.Poblacion) / d.Superficie, 2) AS [@Densidad], d.Poblacion AS [text()]
                FOR XML PATH('Poblacion'), TYPE)
        FOR XML PATH('Info'), TYPE)
FROM @Datos d INNER JOIN dbo.Region r ON r.Region = d.Region
WHERE NOT EXISTS (SELECT 1 FROM dbo.Comuna c WHERE c.IdRegion = r.IdRegion AND c.Comuna = d.Comuna)
GO
