# Prueba Técnica ValueTech - Regiones y Comunas de Chile

Solución .NET 9 compuesta por tres proyectos: `AccesoDatos`, `ApiPvtRegiones` y `WebRegiones`.

## Requisitos
- Visual Studio 2022 actualizado.
- .NET 9 SDK.
- SQL Server 2012 o superior.

## Base de datos
Ejecutar en SQL Server Management Studio, en este orden:
1. `Instalación/01_BaseDatos/01_CrearBaseDatos.sql`
2. `Instalación/01_BaseDatos/02_ProcedimientosAlmacenados.sql`
3. `Instalación/01_BaseDatos/03_CargaInicial.sql`
4. `Instalación/01_BaseDatos/04_UsuarioBaseDatos.sql`
Los nombres de los procedimientos cumplen con la nomenclatura "usp_PVT.." donde PVT es Prueba "ValueTech"

La carga incluye las 16 regiones y comunas de demostración. Los datos son referenciales tal como se indicó el documento de requerimiento.

## Configuración
Actualizar `ConnectionStrings:ConexionBD` en `ApiPvtRegiones/appsettings.json` con el servidor SQL correspondiente. Configurar una API Key propia en `ApiKey:Key` y la misma clave en `WebRegiones/appsettings.json`, sección `ApiRegiones:ApiKey`. 
En producción (un localhost por ejemplo o equipo similar), usar variables de entorno:

1. Abrir powershell como administrador.
2. Ejecutar `Instalación/02_Configuración/InstalaApikey.ps1` => puedes agregar la clave que desees o dejar vacío para que se cree una por defecto.
3. Ejecutar `Instalación/02_Configuración/InstalaConnectionstrings.ps1` => tienes que agregar el servidor e instancia, el usuario y clave de base de datos ya vienen en el shell, y son los mismos que se encuentran en el script sql `04_UsuarioBaseDatos.sql`
4. Reinicia WAS/IIS con powershell acá te dejo los comandos
Para el servicio WAS=> Restart-Service WAS -Force
Para el IIS iisreset

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

