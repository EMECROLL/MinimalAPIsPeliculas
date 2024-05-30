using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.OpenApi.Models;
using MinimalAPIsPeliculas.DTOs;
using MinimalAPIsPeliculas.Entidades;
using MinimalAPIsPeliculas.Filtros;
using MinimalAPIsPeliculas.Repositorios;
using MinimalAPIsPeliculas.Servicios;
using MinimalAPIsPeliculas.Utilidades;

namespace MinimalAPIsPeliculas.Endpoints
{
    public static class ActoresEndpoints
    {
        // Nombre de la carpeta
        private static readonly string contenedor = "actores";

        public static RouteGroupBuilder MapActores(this RouteGroupBuilder group)
        {
            group.MapGet("/", ObtenerTodos).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("actores-get"))
                                           .AgregarParametrosPaginacionOpenAPI();
            group.MapGet("/{id:int}", ObtenerPorId);
            group.MapGet("obtenerPorNombre/{nombre}", ObtenerPorNombre);
            group.MapPost("/", Crear).DisableAntiforgery().AddEndpointFilter<FiltroValidaciones<CrearActorDTO>>()
                                                          .RequireAuthorization("admin")
                                                          .WithOpenApi();
            group.MapPut("/{id:int}", Actualizar).DisableAntiforgery().AddEndpointFilter<FiltroValidaciones<CrearActorDTO>>()
                                                 .RequireAuthorization("admin")
                                                 .WithOpenApi();
            group.MapDelete("/{id:int}", Borrar).RequireAuthorization("admin");

            return group;
        }

        // Obtener todos los actores
        static async Task<Ok<List<ActorDTO>>> ObtenerTodos(IRepositorioActores repositorio, IMapper mapper, 
            PaginacionDTO paginacion)
        {
            // var paginacion = new PaginacionDTO { Pagina = pagina, RecordsPorPagina = recordsPorPagina };
            var actores = await repositorio.ObtenerTodos(paginacion);
            var actoresDTO = mapper.Map<List<ActorDTO>>(actores);
            return TypedResults.Ok(actoresDTO);
        }

        // Obtener actores por Id
        static async Task<Results<Ok<ActorDTO>, NotFound>> ObtenerPorId(int id, IRepositorioActores repositorio, IMapper mapper)
        {
            var actor = await repositorio.ObtenerPorId(id);

            if (actor is null)
            {
                return TypedResults.NotFound();
            }

            var actorDTO = mapper.Map<ActorDTO>(actor);
            return TypedResults.Ok(actorDTO);
        }

        // Obtener actores por Nombre
        static async Task<Ok<List<ActorDTO>>> ObtenerPorNombre(string nombre, IRepositorioActores repositorio, IMapper mapper)
        {
            var actores = await repositorio.ObtenerPorNombre(nombre);
            var actoresDTO = mapper.Map<List<ActorDTO>>(actores);
            return TypedResults.Ok(actoresDTO);
        }

        // Crear actor
        static async Task<Results<Created<ActorDTO>, ValidationProblem>> Crear([FromForm] CrearActorDTO crearActorDTO, 
            IRepositorioActores repositorio, IOutputCacheStore outputCacheStore, IMapper mapper, 
            IAlmacenadorArchivos almacenadorArchivos)
        {
          
            var actor = mapper.Map<Actor>(crearActorDTO);

            // Almacenar foto
            if (crearActorDTO.Foto is not null)
            {
                var url = await almacenadorArchivos.Almacenar(contenedor, crearActorDTO.Foto);
                actor.Foto = url;
            }

            var id = await repositorio.Crear(actor);
            await outputCacheStore.EvictByTagAsync("actores-get", default);
            var actorDTO = mapper.Map<ActorDTO>(actor);
            return TypedResults.Created($"/actores/{id}", actorDTO);
        }

        // Actualizar actores
        static async Task<Results<NoContent, NotFound>> Actualizar(int id, [FromForm] CrearActorDTO crearActorDTO, IRepositorioActores repositorio, 
            IAlmacenadorArchivos almacenadorArchivos, IOutputCacheStore outputCacheStore, IMapper mapper)
        {
            var actorDB = await repositorio.ObtenerPorId(id);

            if (actorDB is null)
            {
                return TypedResults.NotFound();
            }

            var actorParaActualizar = mapper.Map<Actor>(crearActorDTO);
            actorParaActualizar.Id = id;
            actorParaActualizar.Foto = actorDB.Foto;

            if (crearActorDTO.Foto is not null)
            {
                var url = await almacenadorArchivos.Editar(actorParaActualizar.Foto, contenedor, crearActorDTO.Foto);
                actorParaActualizar.Foto = url;
            }

            await repositorio.Actualizar(actorParaActualizar);
            await outputCacheStore.EvictByTagAsync("actores-get", default);

            return TypedResults.NoContent();
        }

        // Borrar actores
        static async Task<Results<NoContent, NotFound>> Borrar (int id, IRepositorioActores repositorio, IOutputCacheStore outputCacheStore, IAlmacenadorArchivos almacenadorArchivos)
        {
            var actorDB = await repositorio.ObtenerPorId(id);

            if (actorDB is null)
            {
                return TypedResults.NotFound();
            }

            await repositorio.Borrar(id);
            await almacenadorArchivos.Borrar(actorDB.Foto, contenedor);
            await outputCacheStore.EvictByTagAsync("actores-get", default);

            return TypedResults.NoContent();
        }
    }
}
