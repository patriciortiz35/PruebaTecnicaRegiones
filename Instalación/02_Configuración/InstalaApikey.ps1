
# ============================================================
# Construí este powershell para realizar la configuración
# automática API Key en las variables de entorno 
# para usarla luego en: ApiPvtRegiones
# Ejecútalo con los más altos permisos
# ============================================================

Write-Host ""
Write-Host "============================================" -ForegroundColor Cyan
Write-Host " CONFIGURACION API KEY" -ForegroundColor Cyan
Write-Host "============================================" -ForegroundColor Cyan
Write-Host ""

# API Key predeterminado
$ApiKeyDefault = "1528D9FCCCFC48C687C7ED7FC74EA7E6"

# Solicitar API Key al usuario (opcional)
$ApiKey = Read-Host "Ingrese el API Key (presione ENTER para utilizar el valor predeterminado)"

# Utilizar valor predeterminado si no se ingresó uno
if ([string]::IsNullOrWhiteSpace($ApiKey)) {
    $ApiKey = $ApiKeyDefault
    Write-Host "Se utilizara el API Key predeterminado." -ForegroundColor Yellow
}
else {
    Write-Host "Se utilizara el API Key ingresado." -ForegroundColor Green
}

# Variable de entorno
$NombreVariable = "ApiKey__Key"

# Confirmar configuración
Write-Host ""
Write-Host "Variable: $NombreVariable"
Write-Host "API Key: ********"
Write-Host ""

$Confirmacion = Read-Host "¿Desea guardar esta configuración? (S/N)"

if ($Confirmacion -ne "S" -and $Confirmacion -ne "s") {
    Write-Host "Configuración cancelada." -ForegroundColor Yellow
    exit
}

# Registrar variable de entorno a nivel de máquina
[Environment]::SetEnvironmentVariable(
    $NombreVariable,
    $ApiKey,
    [EnvironmentVariableTarget]::Machine
)

Write-Host ""
Write-Host "API Key configurado correctamente." -ForegroundColor Green
Write-Host "Variable de entorno: $NombreVariable"

Write-Host ""
Read-Host "Presione ENTER para finalizar"
