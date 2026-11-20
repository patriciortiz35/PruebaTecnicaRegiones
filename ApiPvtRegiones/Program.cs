using AccesoDatos.Interfaces;
using AccesoDatos.Repositorios;
using ApiPvtRegiones.Middleware;
using ApiPvtRegiones.Utils;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opciones =>
{
    opciones.SwaggerDoc("v1", 
        new OpenApiInfo { 
            Title = "API PVT Regiones", 
            Version = "v1", 
            Description = "Consulta y actualización de regiones y comunas." });
    opciones.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Description = "Ingrese la API Key. Header: X-Api-Key",
        Name = "X-Api-Key",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });
    opciones.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { 
            new OpenApiSecurityScheme 
            { 
                Reference = new OpenApiReference 
                { 
                    Type = ReferenceType.SecurityScheme, 
                    Id = "ApiKey" 
                } 
            }, Array.Empty<string>() 
        }
    });
});

//definición scoped repositorio
builder.Services.AddScoped<IRegionRepositorio, RegionRepositorio>();
builder.Services.AddScoped<IComunaRepositorio, ComunaRepositorio>();
//utilitario
builder.Services.AddSingleton<Utilitario>();

builder.Services.AddProblemDetails();

var app = builder.Build();

//middleware excepciones
app.UseMiddleware<MiddlewareExcepciones>();
app.UseHttpsRedirection();
app.UseMiddleware<MiddlewareApiKey>();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapControllers();
app.Run();