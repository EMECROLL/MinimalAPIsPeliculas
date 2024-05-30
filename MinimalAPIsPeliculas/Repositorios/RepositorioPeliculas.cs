using Dapper;
using Microsoft.Data.SqlClient;
using MinimalAPIsPeliculas.DTOs;
using MinimalAPIsPeliculas.Entidades;
using System.Data;

namespace MinimalAPIsPeliculas.Repositorios
{
    public class RepositorioPeliculas : IRepositorioPeliculas
    {
        private readonly string? connectionString;
        private readonly HttpContext httpContext;

        public RepositorioPeliculas(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection")!;
            httpContext = httpContextAccessor.HttpContext!;
        }

        // Obtener todas las peliculas
        public async Task<List<Pelicula>> ObtenerTodos(PaginacionDTO paginacionDTO)
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                var peliculas = await conexion.QueryAsync<Pelicula>("Peliculas_ObtenerTodos", new { paginacionDTO.Pagina, paginacionDTO.RecordsPorPagina }, commandType: CommandType.StoredProcedure);

                var cantidadPeliculas = await conexion.QuerySingleAsync<int>("Peliculas_Cantidad", commandType: CommandType.StoredProcedure);
                httpContext.Response.Headers.Append("cantidadTotalRegistros", cantidadPeliculas.ToString());

                return peliculas.ToList();
            }
        }

        // Obtener pelicula por Id
        public async Task<Pelicula?> ObtenerPorId(int id)
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                using (var multi = await conexion.QueryMultipleAsync("Peliculas_ObtenerPorId", new { id }, commandType: CommandType.StoredProcedure))
                {
                    var pelicula = await multi.ReadFirstAsync<Pelicula>();
                    var comentarios = await multi.ReadAsync<Comentario>();
                    var generos = await multi.ReadAsync<Genero>();
                    var actores = await multi.ReadAsync<ActorPeliculaDTO>();

                    foreach (var genero in generos)
                    {
                        pelicula.GenerosPeliculas.Add(new GeneroPelicula
                        {
                            GeneroId = genero.Id,
                            Genero = genero,
                        });
                    }

                    foreach (var actor in actores)
                    {
                        pelicula.ActoresPeliculas.Add(new ActorPelicula
                        {
                            ActorId = actor.Id,
                            Personaje = actor.Personaje,
                            Actor = new Actor { Nombre = actor.Nombre }
                        });
                    }

                    pelicula.Comentarios = comentarios.ToList();
                    return pelicula;
                }
            }
        }

        // Crear pelicula
        public async Task<int> Crear(Pelicula pelicula)
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                var id = await conexion.QuerySingleAsync<int>("Peliculas_Crear",
                    new
                    {
                        pelicula.Titulo,
                        pelicula.EnCines,
                        pelicula.FechaLanzamiento,
                        pelicula.Poster,
                    }, commandType: CommandType.StoredProcedure);

                pelicula.Id = id;

                return id;
            }
        }

        // Existe
        public async Task<bool> Existe(int id)
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                var existe = await conexion.QuerySingleAsync<bool>("Peliculas_ExistePorId", new { id }, commandType: CommandType.StoredProcedure);

                return existe;
            }
        }

        // Actualizar pelicula
        public async Task Actualizar(Pelicula pelicula)
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                await conexion.ExecuteAsync("Peliculas_Actualizar", pelicula, commandType: CommandType.StoredProcedure);
            }
        }

        // Borrar pelicula
        public async Task Borrar(int id)
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                await conexion.ExecuteAsync("Peliculas_Borrar", new { id }, commandType: CommandType.StoredProcedure);
            }
        }

        // Asignar genero a una pelicula
        public async Task AsignarGeneros (int id, List<int> generosIds)
        {
            // Representa una tabla de datos en memoria
            var dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));

            foreach (var generoId in generosIds)
            {
                dt.Rows.Add(generoId);
            }

            using (var conexion = new SqlConnection(connectionString))
            {
                await conexion.ExecuteAsync("Peliculas_AsignarGeneros", new { peliculaId = id, generosIds = dt }, commandType: CommandType.StoredProcedure);
            }
        }

        // Asignar actores
        public async Task AsignarActores (int id, List<ActorPelicula> actores)
        {
            for (int i = 1; i <= actores.Count; i++)
            {
                actores[i - 1].Orden = i;
            }

            var dt = new DataTable();
            dt.Columns.Add("ActorId", typeof(int));
            dt.Columns.Add("Personaje", typeof(string));
            dt.Columns.Add("Orden", typeof(int));

            foreach (var actorPelicula in actores)
            {
                dt.Rows.Add(actorPelicula.ActorId, actorPelicula.Personaje, actorPelicula.Orden);
            }

            using (var conexion = new SqlConnection(connectionString))
            {
                await conexion.ExecuteAsync("Peliculas_AsignarActores", new { peliculaId = id, actores = dt }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<List<Pelicula>> Filtrar(PeliculasFiltrarDTO peliculasFiltrarDTO)
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                var peliculas = await conexion.QueryAsync<Pelicula>("Peliculas_Filtrar",
                    new
                    {
                        peliculasFiltrarDTO.Pagina,
                        peliculasFiltrarDTO.RecordsPorPagina,
                        peliculasFiltrarDTO.Titulo,
                        peliculasFiltrarDTO.GeneroId,
                        peliculasFiltrarDTO.ProximosEstrenos,
                        peliculasFiltrarDTO.EnCines,
                        peliculasFiltrarDTO.OrdenAscendente,
                        peliculasFiltrarDTO.CampoOrdenar
                    }, commandType: CommandType.StoredProcedure);

                var cantidadPeliculas = await conexion.QuerySingleAsync<int>("Peliculas_Cantidad",
                    new
                    {
                        peliculasFiltrarDTO.Titulo,
                        peliculasFiltrarDTO.GeneroId,
                        peliculasFiltrarDTO.ProximosEstrenos,
                        peliculasFiltrarDTO.EnCines
                    }, commandType: CommandType.StoredProcedure);

                httpContext.Response.Headers.Append("cantidadTotalRegistros", cantidadPeliculas.ToString());

                return peliculas.ToList();
            }
        }
    }
}
