
# ============================================================
# Configuración automática de SQL Server en variables
# de entorno para utilizarla en ApiPvtRegiones.
# Ejecutar con permisos de administrador.
# ============================================================

Write-Host ""
Write-Host "============================================" -ForegroundColor Cyan
Write-Host " CONFIGURACION DE CONEXION SQL SERVER" -ForegroundColor Cyan
Write-Host "============================================" -ForegroundColor Cyan
Write-Host ""

# Credenciales SQL Server
$Usuario = "usr_api_regiones"
$Password = "Regiones.Api#2026_Segura!"

# Base de datos
$BaseDatos = "PruebaTecnicaValueTech"

# Solicitar datos de conexión
$Servidor = Read-Host "Ingrese el nombre o IP del servidor SQL Server"

$Instancia = Read-Host "Ingrese la instancia SQL Server (deje vacio si es predeterminada)"

# Validar servidor
if ([string]::IsNullOrWhiteSpace($Servidor)) {
    Write-Host "Debe ingresar un servidor SQL Server." -ForegroundColor Red
    exit
}

# Construir servidor e instancia
if ([string]::IsNullOrWhiteSpace($Instancia)) {
    $ServidorInstancia = $Servidor
}
else {
    $ServidorInstancia = "$Servidor\$Instancia"
}

# Construir Connection String
$ConnectionString = "Server=$ServidorInstancia;Database=$BaseDatos;User Id=$Usuario;Password=$Password;TrustServerCertificate=True;"

# Mostrar configuración
Write-Host ""
Write-Host "============================================" -ForegroundColor Cyan
Write-Host " RESUMEN DE CONFIGURACION" -ForegroundColor Cyan
Write-Host "============================================" -ForegroundColor Cyan

Write-Host "Servidor: $ServidorInstancia"
Write-Host "Base de datos: $BaseDatos"
Write-Host "Usuario: $Usuario"
Write-Host "Contraseña: ********"
Write-Host ""

$Confirmacion = Read-Host "¿Desea guardar esta configuración? (S/N)"

if ($Confirmacion -ne "S" -and $Confirmacion -ne "s") {
    Write-Host "Configuración cancelada." -ForegroundColor Yellow
    exit
}

# Registrar variable de entorno a nivel de máquina
$NombreVariable = "ConnectionStrings__ConexionBD"

[Environment]::SetEnvironmentVariable(
    $NombreVariable,
    $ConnectionString,
    [EnvironmentVariableTarget]::Machine
)

Write-Host ""
Write-Host "Configuración guardada correctamente." -ForegroundColor Green
Write-Host "Variable: $NombreVariable"

Write-Host ""
Read-Host "Presione ENTER para finalizar"
