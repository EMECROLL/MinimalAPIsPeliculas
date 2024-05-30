using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
using MinimalAPIsPeliculas.DTOs;
using MinimalAPIsPeliculas.Entidades;
using MinimalAPIsPeliculas.Filtros;
using MinimalAPIsPeliculas.Repositorios;

namespace MinimalAPIsPeliculas.Endpoints
{
    public static class GenerosEndpoints
    {
        public static RouteGroupBuilder MapGeneros (this RouteGroupBuilder group)
        {
            // Todos los generos
            group.MapGet("/", ObtenerGeneros)
                .CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60))
                .Tag("generos-get"));

            // Genero por Id
            group.MapGet("/{id:int}", ObtenerGeneroPorId);

            // Crear Generos
            group.MapPost("/", CrearGenero).AddEndpointFilter<FiltroValidaciones<CrearGeneroDTO>>().RequireAuthorization("admin");

            // Actualizar Generos
            group.MapPut("/{id:int}", ActualizarGenero).AddEndpointFilter<FiltroValidaciones<CrearGeneroDTO>>()
                                                       .RequireAuthorization("admin");

            // Eliminar Generos
            group.MapDelete("/{id:int}", BorrarGenero).RequireAuthorization("admin");

            return group;
        }

        // Métodos nombrados

        // Obtener todos los generos
        static async Task<Ok<List<GeneroDTO>>> ObtenerGeneros(IRepositorioGeneros repositorioGeneros, 
            IMapper mapper, ILoggerFactory loggerFactory)
        {
            // ILogger
            var tipo = typeof(GenerosEndpoints);
            var logger = loggerFactory.CreateLogger(tipo.FullName!); // Categoria del mensaje
            logger.LogTrace("Este es un mensage de trace");
            logger.LogDebug("Este es un mensage de debug");
            logger.LogInformation("Este es un mensage de information");
            logger.LogWarning("Este es un mensage de warning");
            logger.LogError("Este es un mensage de error");
            logger.LogCritical("Este es un mensage de critical");

            var generos = await repositorioGeneros.ObtenerTodos();
            //var generosDTO = generos.Select(x => new GeneroDTO { Id = x.Id, Nombre = x.Nombre }).ToList();
            var generosDTO = mapper.Map<List<GeneroDTO>>(generos);
            return TypedResults.Ok(generosDTO);
        }

        // Obtener genero por id
        static async Task<Results<Ok<GeneroDTO>, NotFound>> ObtenerGeneroPorId([AsParameters] ObtenerGeneroPorIdPeticionDTO modelo)
        {
            var genero = await modelo.repositorioGeneros.ObtenerPorId(modelo.id);

            if (genero is null)
            {
                return TypedResults.NotFound();
            }

            var generoDTO = modelo.mapper.Map<GeneroDTO>(genero);

            return TypedResults.Ok(generoDTO);
        }

        // Crear genero
        static async Task<Results<Created<GeneroDTO>, ValidationProblem>> CrearGenero(CrearGeneroDTO crearGeneroDTO, [AsParameters] CrearGeneroPeticionDTO modelo)
        {
            
            var genero = modelo.mapper.Map<Genero>(crearGeneroDTO);

            var id = await modelo.repositorioGeneros.CrearGenero(genero);
            // Limpiar la cache del get
            await modelo.outputCacheStore.EvictByTagAsync("generos-get", default);

            var generoDTO = modelo.mapper.Map<GeneroDTO>(genero);

            return TypedResults.Created($"/generos/{id}", generoDTO);
        }

        // Actualizar genero
        static async Task<Results<NoContent, NotFound, ValidationProblem>> ActualizarGenero(int id, CrearGeneroDTO crearGeneroDTO, IRepositorioGeneros repositorioGeneros, 
            IOutputCacheStore outputCacheStore, IMapper mapper)
        {

            // Verificar si existe ese genero
            var existe = await repositorioGeneros.Existe(id);

            if (!existe)
            {
                return TypedResults.NotFound();
            }

            var genero = mapper.Map<Genero>(crearGeneroDTO);
            genero.Id = id;

            await repositorioGeneros.Actualizar(genero);
            // Limpiar la cache del get
            await outputCacheStore.EvictByTagAsync("generos-get", default);
            return TypedResults.NoContent();
        }

        // Eliminar genero
        static async Task<Results<NoContent, NotFound>> BorrarGenero(int id, IRepositorioGeneros repositorioGeneros, IOutputCacheStore outputCacheStore)
        {
            var existe = await repositorioGeneros.Existe(id);

            if (!existe)
            {
                return TypedResults.NotFound();
            }

            await repositorioGeneros.Borrar(id);
            // Limpiar la cache del get
            await outputCacheStore.EvictByTagAsync("generos-get", default);
            return TypedResults.NoContent();
        }
    }
}
