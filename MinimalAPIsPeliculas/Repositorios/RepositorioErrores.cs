using Dapper;
using Microsoft.Data.SqlClient;
using MinimalAPIsPeliculas.Entidades;

namespace MinimalAPIsPeliculas.Repositorios
{
    public class RepositorioErrores : IRepositorioErrores
    {
        private readonly string? connectionString;

        public RepositorioErrores(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // Crear error
        public async Task Crear(Error error)
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                await conexion.ExecuteAsync("Errores_Crear", new { error.MensajeDeError, error.StackTrace, error.Fecha }, 
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }
    }
}
