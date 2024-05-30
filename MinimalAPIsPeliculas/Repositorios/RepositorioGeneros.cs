using Dapper;
using Microsoft.Data.SqlClient;
using MinimalAPIsPeliculas.Entidades;
using System.Data;

namespace MinimalAPIsPeliculas.Repositorios
{
    public class RepositorioGeneros : IRepositorioGeneros
    {
        // Hacemos que el connectionString sea campo de solo lectura y disponible para toda la clase
        private readonly string? connectionString;

        // Constructor

        // IConfiguration permite interactuar con mis proovedores de configuracion (Connection String)
        public RepositorioGeneros(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // Método para obtener todos los Generos
        public async Task<List<Genero>> ObtenerTodos()
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                // Se usa QueryAsync por que espero multiples resultados
                var generos = await conexion.QueryAsync<Genero>("Generos_ObtenerTodos", commandType: CommandType.StoredProcedure);

                return generos.ToList();
            }
        }

        // Método para obtener Genero por su Id
        // Se coloca un Genero? con el signo de ? para decir que la respuesta podria ser nulo
        public async Task<Genero?> ObtenerPorId(int id)
        {
            using (var conexion = new SqlConnection (connectionString))
            {
                // QueryFirstOrDefaultAsync se usa para decir traeme el primer genero que coincida con ese Id sino existe retornara nullo
                var genero = await conexion.QueryFirstOrDefaultAsync<Genero>("Generos_ObtenerPorId", new {id}, commandType: CommandType.StoredProcedure); // Objeto anonimo - new {id}

                return genero;
            }
        }

        // Método para crear un Genero
        public async Task<int> CrearGenero(Genero genero)
        {
            // Instanciando conexion de SQL
            using (var conexion = new SqlConnection(connectionString))
            {
                // Se usa QuerySingleAsync por que se espera que sea solo un resultado
                var id = await conexion.QuerySingleAsync<int>("Generos_Crear", new { genero.Nombre }, commandType: CommandType.StoredProcedure);

                genero.Id = id;
                return id;
            }
        }

        public async Task<bool> Existe(int id)
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                var existe = await conexion.QuerySingleAsync<bool>("Generos_ExistePorId", new {id}, commandType: CommandType.StoredProcedure);

                return existe;
            }
        }

        public async Task Actualizar(Genero genero)
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                // ExecuteAsync ejecutara una query sin esperar nada como resultado
                await conexion.ExecuteAsync("Generos_Actualizar", genero, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task Borrar(int id)
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                await conexion.ExecuteAsync("Generos_Borrar", new {id}, commandType: CommandType.StoredProcedure);
            }
        }

        // Comprobar si existen varios generos
        public async Task<List<int>> Existen (List<int> ids)
        {
            var dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));

            foreach (var id in ids)
            {
                dt.Rows.Add(id);
            }

            using (var conexion = new SqlConnection(connectionString))
            {
                var idsGenerosExistentes = await conexion.QueryAsync<int>("Generos_ObtenerVariosPorId", new { generosIds = dt }, commandType: CommandType.StoredProcedure);

                return idsGenerosExistentes.ToList();
            }
        }

        // Existen por Id y Nombre
        public async Task<bool> Existe(int id, string nombre)
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                var existe = await conexion.QuerySingleAsync<bool>("Generos_ExistePorIdYNombre", new { id, nombre }, commandType: CommandType.StoredProcedure);
                return existe;
            }
        }
    }
}
