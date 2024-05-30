using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using MinimalAPIsPeliculas.DTOs;
using MinimalAPIsPeliculas.Entidades;
using MinimalAPIsPeliculas.Filtros;
using MinimalAPIsPeliculas.Repositorios;
using MinimalAPIsPeliculas.Servicios;
using MinimalAPIsPeliculas.Utilidades;

namespace MinimalAPIsPeliculas.Endpoints
{
    public static class PeliculasEndpoints
    {
        private static readonly string contenedor = "peliculas";
        public static RouteGroupBuilder MapPeliculas(this RouteGroupBuilder group)
        {
            group.MapGet("/", ObtenerTodos).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("peliculas-get"))
                                           .AgregarParametrosPaginacionOpenAPI();
            group.MapGet("/{id:int}", ObtenerPorId);
            group.MapPost("/", Crear).DisableAntiforgery().AddEndpointFilter<FiltroValidaciones<CrearPeliculaDTO>>().RequireAuthorization("admin").WithOpenApi();
            group.MapPut("/{id:int}", Actualizar).DisableAntiforgery().AddEndpointFilter<FiltroValidaciones<CrearPeliculaDTO>>().RequireAuthorization("admin").WithOpenApi();
            group.MapDelete("/{id:int}", Borrar).RequireAuthorization("admin");
            group.MapPost("/{id:int}/asignargeneros", AsignarGeneros).RequireAuthorization("admin");
            group.MapPost("/{id:int}/asignaractores", AsignarActores).RequireAuthorization("admin");
            group.MapGet("/filtrar", FiltrarPeliculas).AgregarParametrosPeliculasOpenAPI();

            return group;
        }

        // Obtener todas las peliculas
        static async Task<Ok<List<PeliculaDTO>>> ObtenerTodos(IRepositorioPeliculas repositorio, IMapper mapper, PaginacionDTO paginacion)
        {
            var peliculas = await repositorio.ObtenerTodos(paginacion);
            var peliculasDTO = mapper.Map<List<PeliculaDTO>>(peliculas);
            return TypedResults.Ok(peliculasDTO);
        }

        // Obtener peliculas por id
        static async Task<Results<Ok<PeliculaDTO>,NotFound>> ObtenerPorId(int id, IRepositorioPeliculas repositorio, IMapper mapper)
        {
            var pelicula = await repositorio.ObtenerPorId(id);

            if (pelicula is null)
            {
                return TypedResults.NotFound();
            }

            var peliculaDTO = mapper.Map<PeliculaDTO>(pelicula);
            return TypedResults.Ok(peliculaDTO);
        }

        // Crear peliculas
        static async Task<Created<PeliculaDTO>> Crear([FromForm] CrearPeliculaDTO crearPeliculaDTO, IRepositorioPeliculas repositorio, IAlmacenadorArchivos almacenadorArchivos, IOutputCacheStore outputCacheStore, IMapper mapper)
        {
            var pelicula = mapper.Map<Pelicula>(crearPeliculaDTO);
            
            if (crearPeliculaDTO.Poster is not null)
            {
                var url = await almacenadorArchivos.Almacenar(contenedor, crearPeliculaDTO.Poster);
                pelicula.Poster = url;
            }

            var id = await repositorio.Crear(pelicula);
            await outputCacheStore.EvictByTagAsync("peliculas-get", default);
            var peliculaDTO = mapper.Map<PeliculaDTO>(pelicula);
            return TypedResults.Created($"/peliculas/{id}", peliculaDTO);
        }

        // Actualizar peliculas
        static async Task<Results<NoContent, NotFound>> Actualizar (int id, [FromForm] CrearPeliculaDTO crearPeliculaDTO, IRepositorioPeliculas repositorio, IAlmacenadorArchivos almacenadorArchivos, IOutputCacheStore outputCacheStore, IMapper mapper)
        {
            var peliculaDB = await repositorio.ObtenerPorId(id);

            if (peliculaDB is null)
            {
                return TypedResults.NotFound();
            }

            var peliculasParaActualizar = mapper.Map<Pelicula>(crearPeliculaDTO);
            peliculasParaActualizar.Id = id;
            peliculasParaActualizar.Poster = peliculaDB.Poster;

            if(crearPeliculaDTO.Poster is not null)
            {
                var url = await almacenadorArchivos.Editar(peliculasParaActualizar.Poster, contenedor, crearPeliculaDTO.Poster);
                peliculasParaActualizar.Poster = url;
            }

            await repositorio.Actualizar(peliculasParaActualizar);
            await outputCacheStore.EvictByTagAsync("peliculas-get", default);
            return TypedResults.NoContent();
        }

        // Borrar pelicula
        static async Task<Results<NoContent, NotFound>> Borrar(int id, IRepositorioPeliculas repositorio, IOutputCacheStore outputCacheStore, IAlmacenadorArchivos almacenadorArchivos)
        {
            var peliculaDB = await repositorio.ObtenerPorId(id);

            if (peliculaDB is null)
            {
                return TypedResults.NotFound();
            }

            await repositorio.Borrar(id);
            await almacenadorArchivos.Borrar(peliculaDB.Poster, contenedor);
            await outputCacheStore.EvictByTagAsync("peliculas-get", default);
            return TypedResults.NoContent();
        }

        // Asignar un genero a una pelicula
        static async Task<Results<NoContent, NotFound, BadRequest<string>>> AsignarGeneros (int id, List<int> generosIds, IRepositorioPeliculas repositorioPeliculas, IRepositorioGeneros repositorioGeneros)
        {
            // Verificar si existe la pelicula
            if (!await repositorioPeliculas.Existe(id))
            {
                return TypedResults.NotFound();
            }

            var generosExistentes = new List<int>();

            // Comprobar los generos de la pelicula
            if (generosIds.Count != 0)
            {
                generosExistentes = await repositorioGeneros.Existen(generosIds);
            }

            // Si nos dan un genero que no existe
            if (generosExistentes.Count != generosIds.Count)
            {
                var generosNoExistentes = generosIds.Except(generosExistentes);

                return TypedResults.BadRequest($"Los géneros de id {string.Join(",", generosExistentes)} no existen.");
            }

            await repositorioPeliculas.AsignarGeneros(id, generosIds);

            return TypedResults.NoContent();
        }

        // Asignar actores a una pelicula
        static async Task<Results<NotFound, NoContent, BadRequest<string>>> AsignarActores (int id, List<AsignarActorPeliculaDTO> actoresDTO, IRepositorioPeliculas repositorioPeliculas, IRepositorioActores repositorioActores, IMapper mapper)
        {
            if (! await repositorioPeliculas.Existe(id))
            {
                return TypedResults.NotFound();
            }

            var actoresExistentes = new List<int>();
            var actoresIds = actoresDTO.Select(a => a.ActorId).ToList();

            if (actoresDTO.Count != 0)
            {
                actoresExistentes = await repositorioActores.Existen(actoresIds);
            }

            if (actoresExistentes.Count != actoresDTO.Count)
            {
                var actoresNoExistentes = actoresIds.Except(actoresExistentes);

                return TypedResults.BadRequest($"Los actores de id {string.Join(",", actoresNoExistentes)} no existen.");
            }

            var actores = mapper.Map<List<ActorPelicula>>(actoresDTO);

            await repositorioPeliculas.AsignarActores(id, actores);
            return TypedResults.NoContent();
        }

        // Filtrado
        static async Task<Ok<List<PeliculaDTO>>> FiltrarPeliculas (PeliculasFiltrarDTO peliculasFiltrarDTO, IRepositorioPeliculas repositorio,
            IMapper mapper)
        {
            var peliculas = await repositorio.Filtrar(peliculasFiltrarDTO);
            var peliculasDTO = mapper.Map<List<PeliculaDTO>>(peliculas);

            return TypedResults.Ok(peliculasDTO);
        }
    }
}
