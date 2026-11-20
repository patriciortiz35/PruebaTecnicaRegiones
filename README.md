# Prueba Técnica ValueTech - Regiones y Comunas de Chile

Solución .NET 9 compuesta por tres proyectos: `AccesoDatos`, `ApiPvtRegiones` y `WebRegiones`.

## Requisitos
- Visual Studio 2022 actualizado.
- .NET 9 SDK.
- SQL Server 2012 o superior.

## Base de datos
Ejecutar en SQL Server Management Studio, en este orden:
1. `BaseDatos/01_CrearBaseDatos.sql`
2. `BaseDatos/02_ProcedimientosAlmacenados.sql`
3. `BaseDatos/03_CargaInicial.sql`
4. `BaseDatos/04_UsuarioBaseDatos.sql`
Los nombres de los procedimientos cumplen con la nomenclatura "usp_PTR.." donde PTR es Prueba Técnica Regiones

La carga incluye las 16 regiones y comunas de demostración. Los datos son referenciales tal como se indicó el documento de requerimiento.

## Configuración
Actualizar `ConnectionStrings:ConexionBD` en `ApiPvtRegiones/appsettings.json` con el servidor SQL correspondiente. Configurar una API Key propia en `Seguridad:ApiKey` y la misma clave en `WebRegiones/appsettings.json`, sección `ApiRegiones:ApiKey`. En producción, usar variables de entorno o un gestor de secretos; no almacenar claves reales en el repositorio.

Configurar `ApiRegiones:Url` con la URL HTTPS real de la API. Los puertos de desarrollo dependen del perfil de ejecución de Visual Studio; ajustar esa URL según `ApiRegiones/Properties/launchSettings.json` (si se crea) o la consola de ejecución.

## Ejecución
1. Abrir `PruebaTecnicaRegiones.sln`.
2. Restaurar paquetes NuGet.
3. Configurar `ApiPvtRegiones` y `WebRegiones` como proyectos de inicio múltiples.
4. Iniciar primero la API y luego MVC. Abrir Swagger en `/swagger` en ambiente Development.

## Endpoints
| Método | Ruta | Descripción |
|---|---|---|
| GET | `/api/region` | Listado de regiones |
| GET | `/api/region/{idRegion}` | Región por identificador |
| GET | `/api/region/{idRegion}/comuna` | Comunas de una región |
| GET | `/api/region/{idRegion}/comuna/{idComuna}` | Comuna por identificadores |
| POST | `/api/region/{idRegion}/comuna` | Actualiza nombre y datos adicionales |

El POST recibe JSON: `{"idComuna":1,"comuna":"Arica","superficie":4799.40,"poblacion":247552}` y requiere header `X-Api-Key`.

## Decisiones de diseño
- La aplicación MVC no accede directamente a SQL Server: consume únicamente la API.
- La capa `AccesoDatos` usa Dapper y ejecuta exclusivamente procedimientos almacenados.
- `InformacionAdicional` se conserva como XML en SQL Server. La API lo transforma a propiedades JSON (`superficie`, `poblacion`, `densidad`).
- La actualización se realiza con `MERGE` y la densidad se calcula en servidor.
- La API usa middleware para API Key y manejo global de excepciones; `ILogger` registra errores y operaciones relevantes.
- Los procedimientos y el esquema son compatibles con SQL Server 2012+. Se utiliza `THROW` y `MERGE` disponibles desde esa versión.
- Las consideraciones anteriores fueron tomadas en base a la documentación enviada por ValueTech.

