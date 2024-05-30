using MinimalAPIsPeliculas.Entidades;

namespace MinimalAPIsPeliculas.Repositorios
{
    public interface IRepositorioErrores
    {
        Task Crear(Error error);
    }
}