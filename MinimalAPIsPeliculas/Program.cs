using FluentValidation;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MinimalAPIsPeliculas.Endpoints;
using MinimalAPIsPeliculas.Entidades;
using MinimalAPIsPeliculas.Repositorios;
using MinimalAPIsPeliculas.Servicios;
using MinimalAPIsPeliculas.Swagger;
using MinimalAPIsPeliculas.Utilidades;
using System.ComponentModel;

var builder = WebApplication.CreateBuilder(args);

// Obtener los origenes permitidos desde el JSON de configuracion
var origenesPermitidos = builder.Configuration.GetValue<string>("origenesPermitidos")!;

//Inicio del área de servicios

// Configurar CORS
builder.Services.AddCors(opciones =>
{
    // Politica por defecto
    opciones.AddDefaultPolicy(configuracion =>
    {
        // Permitir origenes segun los origenes permitidos en el JSON, cualquier header, cualquier método
        configuracion.WithOrigins(origenesPermitidos).AllowAnyHeader().AllowAnyMethod();
    });

    // Politica libre
    opciones.AddPolicy("libre", configuracion =>
    {
        // Permitir cualquier origen, cualquier header, cualquier método
        configuracion.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

// Configurar Cache
//builder.Services.AddOutputCache();

// Configurar Cache con Redis
builder.Services.AddStackExchangeRedisOutputCache(opciones =>
{
    opciones.Configuration = builder.Configuration.GetConnectionString("redis");
});

// Configuracion de Swagger
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen( c =>
{
    // Información general de Swagger
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Peliculas APIs",
        Description = "Construyendo Minimal APIs con ASP.NET Core 8 y Dapper",
        Contact = new OpenApiContact
        {
            Email = "manuel.pasosbtc1@gmail.com",
            Name = "Manuel Alejandro Pasos Cupul",
            Url = new Uri("https://www.linkedin.com/in/manuelpasosc")
        },
        License = new OpenApiLicense
        {
            Name = "License MIT",
            Url = new Uri("https://opensource.org/license/mit")
        }
    });

    // Uso de JWT en Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header
    });

    c.OperationFilter<FiltroAutorizacion>();
});

// Repositorio de Generos
builder.Services.AddScoped<IRepositorioGeneros, RepositorioGeneros>();
// Repositorio de Actores
builder.Services.AddScoped<IRepositorioActores, RepositorioActores>();
// Repositorio de Peliculas
builder.Services.AddScoped<IRepositorioPeliculas, RepositorioPeliculas>();
// Repositorio de Comentarios
builder.Services.AddScoped<IRepositorioComentarios, RepositorioComentarios>();
// Repositorio de Errores
builder.Services.AddScoped<IRepositorioErrores, RepositorioErrores>();
// Repositorio de Usuarios
builder.Services.AddScoped<IRepositorioUsuarios, RepositorioUsuarios>();

// Almacenador de Archivos de Azure
//builder.Services.AddScoped<IAlmacenadorArchivos, AlmacenadorArchivosAzure>();

// Almacenador de Archivos Local
builder.Services.AddScoped<IAlmacenadorArchivos, AlmacenadorArchivosLocal>();
// Servicio de Usuarios
builder.Services.AddTransient<IServicioUsuarios, ServicioUsuarios>();

builder.Services.AddHttpContextAccessor(); // Poder tener acceso a IHttpContextAccessor

// Uso de AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// Validaciones - Fluent Validator
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// Manejador de excepciones
builder.Services.AddProblemDetails();

// Autenticación y autorización
builder.Services.AddAuthentication().AddJwtBearer(opciones => 
{
    opciones.MapInboundClaims = false;

    opciones.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true, // Validar el tiempo de vida del Token
        ValidateIssuerSigningKey = true, // Que el Token tenga llave secreta
                                         // IssuerSigningKey = Llaves.ObtenerLlave(builder.Configuration).First(), // Obtener una unica llave y que sea la que proporcione mi aplicación
        IssuerSigningKeys = Llaves.ObtenerTodasLasLlaves(builder.Configuration), // Obtener las llaves que yo proporciono y que tambien proporciona la herramienta de User JWT
        ClockSkew = TimeSpan.Zero // Evitar problemas de diferencia de tiempo
    };
});
builder.Services.AddAuthorization(opciones =>
{
    opciones.AddPolicy("admin", politica => politica.RequireClaim("admin"));
});

// Identity
builder.Services.AddTransient<IUserStore<IdentityUser>, UsuarioStore>();
builder.Services.AddIdentityCore<IdentityUser>();
builder.Services.AddTransient<SignInManager<IdentityUser>>();

//Fin del área de servicios

var app = builder.Build();

//Inicio del área de los middleware

// Uso de Swagger
app.UseSwagger();
app.UseSwaggerUI();

// Manejador de excepciones
app.UseExceptionHandler(exceptionHandlerApp => exceptionHandlerApp.Run(async context =>
{
    var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();

    var excepcion = exceptionHandlerFeature?.Error!;

    var error = new Error();
    error.Fecha = DateTime.UtcNow;
    error.MensajeDeError = excepcion.Message;
    error.StackTrace = excepcion.StackTrace;

    var repositorio = context.RequestServices.GetRequiredService<IRepositorioErrores>();
    await repositorio.Crear(error);

    await TypedResults.BadRequest(
        new { tipo = "Error", mensaje = "Ha ocurrido un mensaje de error inesperado", estatus = 500 }).ExecuteAsync(context);
}));
// Retornar códigos de status cuando haya errores
app.UseStatusCodePages();

// Permitir que los clientes puedan acceder a los archivos estaticos
app.UseStaticFiles();

// Antes de los endpoints colocamos el middleware para el CORS
app.UseCors();

// Activar cache para cualquier endpoint
app.UseOutputCache();

// Autenticación y autorización
app.UseAuthorization();

// Hacer que este endpoint utilice la segunda politica que creamos
app.MapGet("/", [EnableCors(policyName: "libre")]  () => "Hello World!");
app.MapGet("/error", () =>
{
    throw new InvalidOperationException("Error de ejemplo");
});

// Model Binding
app.MapPost("/modelbinding/{nombre}", ([FromRoute] string? nombre) =>
{
    if (nombre is null)
    {
        nombre = "Vacio";
    }

    return TypedResults.Ok(nombre);
});

// Generos 
app.MapGroup("/generos").MapGeneros();
// Actores
app.MapGroup("/actores").MapActores();
// Peliculas
app.MapGroup("/peliculas").MapPeliculas();
// Comentarios
app.MapGroup("/pelicula/{peliculaId:int}/comentarios").MapComentarios();
// Usuarios
app.MapGroup("/usuarios").MapUsuarios();

//Fin del área de los middleware

app.Run();