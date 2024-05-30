using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
using MinimalAPIsPeliculas.DTOs;
using MinimalAPIsPeliculas.Entidades;
using MinimalAPIsPeliculas.Filtros;
using MinimalAPIsPeliculas.Repositorios;
using MinimalAPIsPeliculas.Servicios;

namespace MinimalAPIsPeliculas.Endpoints
{
    public static class ComentariosEndpoints
    {
        public static RouteGroupBuilder MapComentarios(this RouteGroupBuilder group)
        {
            group.MapGet("/", ObtenerTodos)
                .CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("comentarios-get")
                .SetVaryByRouteValue(new string[] { "peliculaId" })); // El cache va a variar dependiendo la peliculaId
            group.MapGet("/{id:int}", ObtenerPorId);
            group.MapPost("/", Crear).AddEndpointFilter<FiltroValidaciones<CrearComentarioDTO>>()
                                     .RequireAuthorization();
            group.MapPut("/{id:int}", Actualizar).AddEndpointFilter<FiltroValidaciones<CrearComentarioDTO>>()
                                                 .RequireAuthorization();
            group.MapDelete("/{id:int}", Borrar).RequireAuthorization();

            return group;
        }

        // Obtener los comentarios de una pelicula
        static async Task<Results<Ok<List<ComentarioDTO>>, NotFound>> ObtenerTodos(int peliculaId, IRepositorioComentarios repositorioComentarios, IRepositorioPeliculas repositorioPeliculas, IMapper mapper)
        {
            // Si la pelicula no existe
            if (!await repositorioPeliculas.Existe(peliculaId))
            {
                return TypedResults.NotFound();
            }

            var comentarios = await repositorioComentarios.ObtenerTodos(peliculaId);
            var comentariosDTO = mapper.Map<List<ComentarioDTO>>(comentarios);

            return TypedResults.Ok(comentariosDTO);
        }

        // Obtener un comentario por su id
        static async Task<Results<Ok<ComentarioDTO>, NotFound>> ObtenerPorId(int peliculaId, int id, IRepositorioComentarios repositorio, IMapper mapper)
        {
            var comentario = await repositorio.ObtenerPorId(id);

            if (comentario is null)
            {
                return TypedResults.NotFound();
            }

            var comentarioDTO = mapper.Map<ComentarioDTO>(comentario);

            return TypedResults.Ok(comentarioDTO);
        }

        // Crear comentario para una pelicula
        static async Task<Results<Created<ComentarioDTO>, NotFound, BadRequest<string>>> Crear(int peliculaId, CrearComentarioDTO crearComentarioDTO, 
            IRepositorioComentarios repositorioComentarios, IRepositorioPeliculas repositorioPeliculas, 
            IMapper mapper, IOutputCacheStore outputCacheStore, IServicioUsuarios servicioUsuarios)
        {
            // Si la pelicula no existe
            if (!await repositorioPeliculas.Existe(peliculaId))
            {
                return TypedResults.NotFound();
            }

            var comentario = mapper.Map<Comentario>(crearComentarioDTO);
            comentario.PeliculaId = peliculaId;

            // Buscar el usuario logeado que hizo el comentario
            var usuario = await servicioUsuarios.ObtenerUsuario();

            if (usuario is null)
            {
                return TypedResults.BadRequest("Usuario no encontrado");
            }

            comentario.UsuarioId = usuario.Id;

            // Crear comentario
            var id = await repositorioComentarios.Crear(comentario);
            await outputCacheStore.EvictByTagAsync("comentarios-get", default);
            var comentarioDTO = mapper.Map<ComentarioDTO>(comentario);

            return TypedResults.Created($"/comentario/{id}", comentarioDTO);
        }

        // Actualizar comentario
        static async Task<Results<NoContent, NotFound, ForbidHttpResult>> Actualizar(int peliculaId, int id, 
            CrearComentarioDTO crearComentarioDTO, IOutputCacheStore outputCacheStore, 
            IRepositorioComentarios repositorioComentarios, IRepositorioPeliculas repositorioPeliculas, 
            IServicioUsuarios servicioUsuarios)
        {
            // Si la pelicula no existe
            if (!await repositorioPeliculas.Existe(peliculaId))
            {
                return TypedResults.NotFound();
            }

            var comentarioBD = await repositorioComentarios.ObtenerPorId(id);

            if (comentarioBD is null)
            {
                return TypedResults.NotFound();
            }

            var usuario = await servicioUsuarios.ObtenerUsuario();

            if (usuario is null)
            {
                return TypedResults.NotFound();
            }

            if (comentarioBD.UsuarioId != usuario.Id)
            {
                return TypedResults.Forbid();
            }

            comentarioBD.Cuerpo = crearComentarioDTO.Cuerpo;

            await repositorioComentarios.Actualizar(comentarioBD);
            await outputCacheStore.EvictByTagAsync("comentarios-get", default);

            return TypedResults.NoContent();
        }

        // Borrar comentarios
        static async Task<Results<NoContent, NotFound, ForbidHttpResult>> Borrar (int peliculaId, int id, 
            IRepositorioComentarios repositorio, IOutputCacheStore outputCacheStore,
            IServicioUsuarios servicioUsuarios)
        {
            var comentarioBD = await repositorio.ObtenerPorId(id);

            if (comentarioBD is null)
            {
                return TypedResults.NotFound();
            }

            var usuario = await servicioUsuarios.ObtenerUsuario();

            if (usuario is null)
            {
                return TypedResults.NotFound();
            }

            if (comentarioBD.UsuarioId != usuario.Id)
            {
                return TypedResults.Forbid();
            }

            await repositorio.Borrar(id);
            await outputCacheStore.EvictByTagAsync("comentarios-get", default);

            return TypedResults.NoContent();
        }
    }
}
